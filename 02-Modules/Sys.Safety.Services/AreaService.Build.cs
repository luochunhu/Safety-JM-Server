using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Area;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.Arearestrictedperson;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.Processing.Rpc;
using Sys.DataCollection.Common.Rpc;

namespace Sys.Safety.Services
{
    public partial class AreaService : IAreaService
    {
        private IAreaRepository _Repository;
        private IAreaCacheService _AreaCacheService;
        private IR_ArearestrictedpersonService _R_ArearestrictedpersonService;
        private IAreaRuleService _AreaRuleService;

        public AreaService(IAreaRepository _Repository, IAreaCacheService _AreaCacheService, IR_ArearestrictedpersonService _R_ArearestrictedpersonService, IAreaRuleService _AreaRuleService)
        {
            this._Repository = _Repository;
            this._AreaCacheService = _AreaCacheService;
            this._R_ArearestrictedpersonService = _R_ArearestrictedpersonService;
            this._AreaRuleService = _AreaRuleService;
        }
        public BasicResponse<AreaInfo> AddArea(AreaAddRequest arearequest)
        {
            var arearesponse = new BasicResponse<AreaInfo>();
            //判断缓存中是否存在
            AreaInfo oldArea = null;
            AreaCacheGetByKeyRequest AreaCacheRequest = new AreaCacheGetByKeyRequest();
            AreaCacheRequest.Areaid = arearequest.AreaInfo.Areaid;
            oldArea = _AreaCacheService.GetByKeyAreaCache(AreaCacheRequest).Data;
            if (oldArea != null)
            {
                //缓存中存在此测点
                arearesponse.Code = 1;
                arearesponse.Message = "当前添加的区域已存在！";
                return arearesponse;
            }

            //向网关同步数据  20180103
            if (!string.IsNullOrEmpty(arearequest.AreaInfo.Bz3) && arearequest.AreaInfo.Bz3 == "1")//如果是广播分区，则需要向网关同步数据
            {
                List<AreaInfo> SendItemList = new List<AreaInfo>();
                SendItemList.Add(arearequest.AreaInfo);
                var resultSync = SynchronousDataToGateway(SendItemList);
                if (!resultSync)
                {
                    arearesponse.Code = 1;
                    arearesponse.Message = "向网关同步数据失败！";
                    return arearesponse;
                }
            }

            var _area = ObjectConverter.Copy<AreaInfo, AreaModel>(arearequest.AreaInfo);
            var resultarea = _Repository.AddArea(_area);
            //更新区域限制、禁止进入人员信息
            SaveRestrictedperson(arearequest.AreaInfo.Areaid, arearequest.AreaInfo.RestrictedpersonInfoList);
            //更新区域识别器定义限制信息
            SaveAreaRule(arearequest.AreaInfo.Areaid, arearequest.AreaInfo.AreaRuleInfoList);

            //更新区域缓存  20171128
            AreaCacheAddRequest AreaCacheAddRequest = new Sys.Safety.Request.PersonCache.AreaCacheAddRequest();
            AreaCacheAddRequest.AreaInfo = arearequest.AreaInfo;
            _AreaCacheService.AddAreaCache(AreaCacheAddRequest);

            arearesponse.Data = ObjectConverter.Copy<AreaModel, AreaInfo>(resultarea);
            return arearesponse;
        }
        public BasicResponse<AreaInfo> UpdateArea(AreaUpdateRequest arearequest)
        {
            var arearesponse = new BasicResponse<AreaInfo>();
            //判断缓存中是否存在
            AreaInfo oldArea = null;
            AreaCacheGetByKeyRequest AreaCacheRequest = new AreaCacheGetByKeyRequest();
            AreaCacheRequest.Areaid = arearequest.AreaInfo.Areaid;
            oldArea = _AreaCacheService.GetByKeyAreaCache(AreaCacheRequest).Data;
            if (oldArea == null)
            {
                //缓存中存在此测点
                arearesponse.Code = 1;
                arearesponse.Message = "当前更新的区域不存在！";
                return arearesponse;
            }

            //向网关同步数据  20180103
            if (!string.IsNullOrEmpty(arearequest.AreaInfo.Bz3) && arearequest.AreaInfo.Bz3 == "1")//如果是广播分区，则需要向网关同步数据
            {
                List<AreaInfo> SendItemList = new List<AreaInfo>();
                SendItemList.Add(arearequest.AreaInfo);
                var resultSync = SynchronousDataToGateway(SendItemList);
                if (!resultSync)
                {
                    arearesponse.Code = 1;
                    arearesponse.Message = "向网关同步数据失败！";
                    return arearesponse;
                }
            }

            var _area = ObjectConverter.Copy<AreaInfo, AreaModel>(arearequest.AreaInfo);
            _Repository.UpdateArea(_area);
            //更新区域限制、禁止进入人员信息
            SaveRestrictedperson(arearequest.AreaInfo.Areaid, arearequest.AreaInfo.RestrictedpersonInfoList);
            //更新区域识别器定义限制信息
            SaveAreaRule(arearequest.AreaInfo.Areaid, arearequest.AreaInfo.AreaRuleInfoList);
            //更新区域缓存  20171128
            AreaCacheUpdateRequest AreaCacheUpdateRequest = new AreaCacheUpdateRequest();
            AreaCacheUpdateRequest.AreaInfo = arearequest.AreaInfo;
            _AreaCacheService.UpdateAreaCache(AreaCacheUpdateRequest);

            arearesponse.Data = ObjectConverter.Copy<AreaModel, AreaInfo>(_area);
            return arearesponse;
        }
        public BasicResponse DeleteArea(AreaDeleteRequest arearequest)
        {
            var arearesponse = new BasicResponse();
            //判断缓存中是否存在
            AreaInfo oldArea = null;
            AreaCacheGetByKeyRequest AreaCacheRequest = new AreaCacheGetByKeyRequest();
            AreaCacheRequest.Areaid = arearequest.Id;
            oldArea = _AreaCacheService.GetByKeyAreaCache(AreaCacheRequest).Data;
            if (oldArea == null)
            {
                //缓存中存在此测点
                arearesponse.Code = 1;
                arearesponse.Message = "当前删除的区域不存在！";
                return arearesponse;
            }

            //向网关同步数据  20180103
            if (!string.IsNullOrEmpty(oldArea.Bz3) && oldArea.Bz3 == "1")//如果是广播分区，则需要向网关同步数据
            {
                List<AreaInfo> SendItemList = new List<AreaInfo>();
                SendItemList.Add(oldArea);
                var resultSync = SynchronousDataToGateway(SendItemList);
                if (!resultSync)
                {
                    arearesponse.Code = 1;
                    arearesponse.Message = "向网关同步数据失败！";
                    return arearesponse;
                }
            }

            _Repository.DeleteArea(arearequest.Id);
            //删除区域限制、禁止进入人员信息
            SaveRestrictedperson(oldArea.Areaid, new List<R_ArearestrictedpersonInfo>());
            //删除区域识别器定义限制信息
            SaveAreaRule(oldArea.Areaid, new List<AreaRuleInfo>());
            //更新区域缓存  20171128
            AreaCacheDeleteRequest AreaCacheDelRequest = new Sys.Safety.Request.PersonCache.AreaCacheDeleteRequest();
            AreaCacheDelRequest.AreaInfo = oldArea;
            _AreaCacheService.DeleteAreaCache(AreaCacheDelRequest);

            return arearesponse;
        }
        /// <summary>
        /// 保存区域限制进入、禁止进入人员信息
        /// </summary>
        /// <param name="AreaId"></param>
        /// <param name="restrictedpersonInfoList"></param>
        private void SaveRestrictedperson(string AreaId, List<R_ArearestrictedpersonInfo> restrictedpersonInfoList)
        {
            if (restrictedpersonInfoList == null)
            {
                return;
            }
            //先删除原来的限制进入、禁止进入信息
            R_ArearestrictedpersonDeleteByAreaIdRequest restrictedpersonRequest = new R_ArearestrictedpersonDeleteByAreaIdRequest();
            restrictedpersonRequest.AreaId = AreaId;
            _R_ArearestrictedpersonService.DeleteArearestrictedpersonByAreaId(restrictedpersonRequest);
            //再进行添加操作
            foreach (R_ArearestrictedpersonInfo addRestrictedpersonInfo in restrictedpersonInfoList)
            {
                R_ArearestrictedpersonAddRequest add_restrictedpersonRequest = new R_ArearestrictedpersonAddRequest();
                add_restrictedpersonRequest.ArearestrictedpersonInfo = addRestrictedpersonInfo;
                _R_ArearestrictedpersonService.AddArearestrictedperson(add_restrictedpersonRequest);
            }
        }
        /// <summary>
        /// 保存区域设备定义限制信息
        /// </summary>
        /// <param name="AreaId"></param>
        /// <param name="areaRuleInfoList"></param>
        private void SaveAreaRule(string AreaId, List<AreaRuleInfo> areaRuleInfoList)
        {
            if (areaRuleInfoList == null)
            {
                return;
            }
            //先删除原来区域设备定义限制信息
            AreaRuleDeleteRequest areaRuleRequest = new AreaRuleDeleteRequest();
            areaRuleRequest.Id = AreaId;
            _AreaRuleService.DeleteAreaRuleByAreaID(areaRuleRequest);
            //再进行添加操作
            foreach (AreaRuleInfo addareaRuleInfo in areaRuleInfoList)
            {
                AreaRuleAddRequest areaRuleAddRequest = new AreaRuleAddRequest();
                areaRuleAddRequest.AreaRuleInfo = addareaRuleInfo;
                _AreaRuleService.AddAreaRule(areaRuleAddRequest);
            }
        }
        public BasicResponse<List<AreaInfo>> GetAreaList(AreaGetListRequest arearequest)
        {
            var arearesponse = new BasicResponse<List<AreaInfo>>();
            arearequest.PagerInfo.PageIndex = arearequest.PagerInfo.PageIndex - 1;
            if (arearequest.PagerInfo.PageIndex < 0)
            {
                arearequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var areaModelLists = _Repository.GetAreaList(arearequest.PagerInfo.PageIndex, arearequest.PagerInfo.PageSize, out rowcount);
            var areaInfoLists = new List<AreaInfo>();
            foreach (var item in areaModelLists)
            {
                var AreaInfo = ObjectConverter.Copy<AreaModel, AreaInfo>(item);
                areaInfoLists.Add(AreaInfo);
            }
            arearesponse.Data = areaInfoLists;
            return arearesponse;
        }
        public BasicResponse<AreaInfo> GetAreaById(AreaGetRequest arearequest)
        {
            var result = _Repository.GetAreaById(arearequest.Id);
            var areaInfo = ObjectConverter.Copy<AreaModel, AreaInfo>(result);
            var arearesponse = new BasicResponse<AreaInfo>();
            arearesponse.Data = areaInfo;
            return arearesponse;
        }


        public BasicResponse<List<AreaInfo>> GetAllAreaList(AreaGetListRequest arearequest)
        {
            var result = _Repository.GetAreaList();
            var areaInfo = ObjectConverter.CopyList<AreaModel, AreaInfo>(result);
            var arearesponse = new BasicResponse<List<AreaInfo>>();
            arearesponse.Data = areaInfo.ToList();
            return arearesponse;
        }
        /// <summary>
        /// 获取所有区域缓存
        /// </summary>
        /// <param name="arearequest"></param>
        /// <returns></returns>
        public BasicResponse<List<AreaInfo>> GetAllAreaCache(AreaCacheGetAllRequest arearequest)
        {
            return _AreaCacheService.GetAllAreaCache(arearequest);
        }

        /// <summary>
        /// 向网关同步数据
        /// </summary>
        /// <param name="SendItemList"></param>
        /// <returns></returns>
        private bool SynchronousDataToGateway(List<AreaInfo> SendItemList)
        {
            foreach (AreaInfo areaInfo in SendItemList)
            {
                PartitionControlRequest partitionControlRequest = new PartitionControlRequest();

                if (string.IsNullOrEmpty(areaInfo.Areadescribe))
                {
                    partitionControlRequest.zoneId = "";//分区标识
                }
                else
                {
                    partitionControlRequest.zoneId = areaInfo.Areadescribe;//分区标识
                }
                partitionControlRequest.paTaskDN = "";//分区广播接入号码[暂未使用]
                partitionControlRequest.zoneName = areaInfo.Areaname;//分区名称
                partitionControlRequest.almLinkUdn1 = "";//分区报警联动用户号码列表[暂未使用]
                partitionControlRequest.almLinkUdn2 = "";//分区报警联动用户号码列表[暂未使用]
                partitionControlRequest.almLinkUdn3 = "";//分区报警联动用户号码列表[暂未使用]

                partitionControlRequest.InfoState = areaInfo.InfoState;
                //调用RPC发送
                MasProtocol masProtocol = new MasProtocol(SystemType.Broadcast, DirectionType.Down, ProtocolType.PartitionControlRequest);
                masProtocol.Protocol = partitionControlRequest;
                PartitionControlResponse result = RpcService.Send<PartitionControlResponse>(masProtocol, RequestType.BusinessRequest);

                if (result == null && result.retCode != "0")
                {
                    Basic.Framework.Logging.LogHelper.Error("向网关同步广播分区信息失败！,分区名称：" + areaInfo.Areaname);
                    return false;
                }
                else
                {
                    //将返回的分区标识更新到缓存及数据库  20180103
                    areaInfo.Areadescribe = result.zoneId;
                    //更新数据库
                    var _area = ObjectConverter.Copy<AreaInfo, AreaModel>(areaInfo);
                    _Repository.UpdateArea(_area);                    
                    //更新区域缓存  20171128
                    AreaCacheUpdateRequest AreaCacheUpdateRequest = new AreaCacheUpdateRequest();
                    AreaCacheUpdateRequest.AreaInfo = areaInfo;
                    _AreaCacheService.UpdateAreaCache(AreaCacheUpdateRequest);
                }


            }
            return true;
        }

    }
}



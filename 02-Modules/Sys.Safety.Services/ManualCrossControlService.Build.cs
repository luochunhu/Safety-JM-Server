using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.ManualCrossControl;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Data;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.Processing.Rpc;
using Sys.Safety.ServiceContract.Driver;
using Sys.Safety.Request.Driver;
using Sys.DataCollection.Common.Rpc;
using Sys.Safety.Enums;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 手动/交叉控制模块服务
    /// </summary>
    public partial class ManualCrossControlService : IManualCrossControlService
    {
        private IManualCrossControlRepository _Repository;
        private IManualCrossControlCacheService _ManualCrossControlCacheService;
        private IDriverManualCrossControlService _DriverManualCrossControlService;

        public ManualCrossControlService(IManualCrossControlRepository _Repository, IManualCrossControlCacheService _ManualCrossControlCacheService
            , IDriverManualCrossControlService _DriverManualCrossControlService)
        {
            this._Repository = _Repository;
            this._ManualCrossControlCacheService = _ManualCrossControlCacheService;
            this._DriverManualCrossControlService = _DriverManualCrossControlService;
        }
        /// <summary>
        /// 添加手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        public BasicResponse AddManualCrossControl(ManualCrossControlAddRequest ManualCrossControlRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_JcsdkzInfo item = ManualCrossControlRequest.ManualCrossControlInfo;

            ManualCrossControlCacheGetByKeyRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByKeyRequest();
            manualCrossControlCacheRequest.ManualCrosControlId = item.ID;
            var result = _ManualCrossControlCacheService.GetByKeyManualCrossControlCache(manualCrossControlCacheRequest);
            if (result.Data != null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前添加的手动/交叉控制信息已存在！";
                return Result;
            }
            //向网关同步数据
            List<Jc_JcsdkzInfo> SendItemList = new List<Jc_JcsdkzInfo>();
            SendItemList.Add(item);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            //数据库操作
            var _jc_Jcsdkz = ObjectConverter.Copy<Jc_JcsdkzInfo, Jc_JcsdkzModel>(ManualCrossControlRequest.ManualCrossControlInfo);
            var resultjc_Jcsdkz = _Repository.AddManualCrossControl(_jc_Jcsdkz);

            //缓存操作
            ManualCrossControlCacheAddRequest AddManualCrossControlCacheRequest = new ManualCrossControlCacheAddRequest();
            AddManualCrossControlCacheRequest.ManualCrossControlInfo = item;
            _ManualCrossControlCacheService.AddManualCrossControlCache(AddManualCrossControlCacheRequest);

            //调用驱动重新加载控制信息
            DriverManualCrossControlReLoadRequest reLoadRequest = new DriverManualCrossControlReLoadRequest();
            _DriverManualCrossControlService.ReLoad(reLoadRequest);

            return Result;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        public BasicResponse AddManualCrossControls(ManualCrossControlsRequest ManualCrossControlRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_JcsdkzInfo> items = ManualCrossControlRequest.ManualCrossControlInfos;
            List<Jc_JcsdkzInfo> ManualCrossControlCaches = new List<Jc_JcsdkzInfo>();

            ManualCrossControlCacheGetAllRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetAllRequest();
            var result = _ManualCrossControlCacheService.GetAllManualCrossControlCache(manualCrossControlCacheRequest);
            ManualCrossControlCaches = result.Data;

            foreach (Jc_JcsdkzInfo item in items)
            {
                Jc_JcsdkzInfo itemCache = ManualCrossControlCaches.Find(a => a.ID == item.ID);
                if (itemCache != null)
                {
                    //缓存中存在此测点                
                    Result.Code = 1;
                    Result.Message = "当前添加的手动/交叉控制信息已存在！";
                    return Result;
                }
            }
            //向网关同步数据
            List<Jc_JcsdkzInfo> SendItemList = items;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            TransactionsManager.BeginTransaction(() =>
            {
                foreach (Jc_JcsdkzInfo item in items)
                {
                    //数据库操作
                    var _jc_Jcsdkz = ObjectConverter.Copy<Jc_JcsdkzInfo, Jc_JcsdkzModel>(item);
                    var resultjc_Jcsdkz = _Repository.AddManualCrossControl(_jc_Jcsdkz);

                    //缓存操作
                    ManualCrossControlCacheAddRequest AddManualCrossControlCacheRequest = new ManualCrossControlCacheAddRequest();
                    AddManualCrossControlCacheRequest.ManualCrossControlInfo = item;
                    _ManualCrossControlCacheService.AddManualCrossControlCache(AddManualCrossControlCacheRequest);
                }
            });

            //调用驱动重新加载控制信息
            DriverManualCrossControlReLoadRequest reLoadRequest = new DriverManualCrossControlReLoadRequest();
            _DriverManualCrossControlService.ReLoad(reLoadRequest);

            return Result;
        }
        /// <summary>
        /// 更新手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateManualCrossControl(ManualCrossControlUpdateRequest ManualCrossControlRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_JcsdkzInfo item = ManualCrossControlRequest.ManualCrossControlInfo;

            ManualCrossControlCacheGetByKeyRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByKeyRequest();
            manualCrossControlCacheRequest.ManualCrosControlId = item.ID;
            var result = _ManualCrossControlCacheService.GetByKeyManualCrossControlCache(manualCrossControlCacheRequest);
            if (result.Data == null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前更新的手动/交叉控制信息不存在！";
                return Result;
            }
            //向网关同步数据
            List<Jc_JcsdkzInfo> SendItemList = new List<Jc_JcsdkzInfo>();
            SendItemList.Add(item);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            //数据库操作
            var _jc_Jcsdkz = ObjectConverter.Copy<Jc_JcsdkzInfo, Jc_JcsdkzModel>(ManualCrossControlRequest.ManualCrossControlInfo);
            _Repository.UpdateManualCrossControl(_jc_Jcsdkz);

            //缓存操作
            ManualCrossControlCacheUpdateRequest UpdateManualCrossControlCacheRequest = new ManualCrossControlCacheUpdateRequest();
            UpdateManualCrossControlCacheRequest.ManualCrossControlInfo = item;
            _ManualCrossControlCacheService.UpdateManualCrossControlCache(UpdateManualCrossControlCacheRequest);

            //调用驱动重新加载控制信息
            DriverManualCrossControlReLoadRequest reLoadRequest = new DriverManualCrossControlReLoadRequest();
            _DriverManualCrossControlService.ReLoad(reLoadRequest);

            return Result;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateManualCrossControls(ManualCrossControlsRequest ManualCrossControlRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_JcsdkzInfo> items = ManualCrossControlRequest.ManualCrossControlInfos;
            List<Jc_JcsdkzInfo> ManualCrossControlCaches = new List<Jc_JcsdkzInfo>();

            ManualCrossControlCacheGetAllRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetAllRequest();
            var result = _ManualCrossControlCacheService.GetAllManualCrossControlCache(manualCrossControlCacheRequest);
            ManualCrossControlCaches = result.Data;

            foreach (Jc_JcsdkzInfo item in items)
            {
                Jc_JcsdkzInfo itemCache = ManualCrossControlCaches.Find(a => a.ID == item.ID);
                if (itemCache == null)
                {
                    //缓存中存在此测点                
                    Result.Code = 1;
                    Result.Message = "当前更新的手动/交叉控制信息不存在！";
                    return Result;
                }
            }
            //向网关同步数据
            List<Jc_JcsdkzInfo> SendItemList = items;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
            {


                foreach (Jc_JcsdkzInfo item in items)
                {
                    //数据库操作
                    var _jc_Jcsdkz = ObjectConverter.Copy<Jc_JcsdkzInfo, Jc_JcsdkzModel>(item);
                    _Repository.UpdateManualCrossControl(_jc_Jcsdkz);

                    //缓存操作
                    ManualCrossControlCacheUpdateRequest UpdateManualCrossControlCacheRequest = new ManualCrossControlCacheUpdateRequest();
                    UpdateManualCrossControlCacheRequest.ManualCrossControlInfo = item;
                    _ManualCrossControlCacheService.UpdateManualCrossControlCache(UpdateManualCrossControlCacheRequest);
                }
            });

            //调用驱动重新加载控制信息
            DriverManualCrossControlReLoadRequest reLoadRequest = new DriverManualCrossControlReLoadRequest();
            _DriverManualCrossControlService.ReLoad(reLoadRequest);

            return Result;
        }
        /// <summary>
        /// 删除手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteManualCrossControl(ManualCrossControlDeleteRequest ManualCrossControlRequest)
        {
            BasicResponse Result = new BasicResponse();
            ManualCrossControlCacheGetByKeyRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByKeyRequest();
            manualCrossControlCacheRequest.ManualCrosControlId = ManualCrossControlRequest.Id;
            var result = _ManualCrossControlCacheService.GetByKeyManualCrossControlCache(manualCrossControlCacheRequest);

            if (result.Data == null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前删除的手动/交叉控制不存在！";
                return Result;
            }

            //向网关同步数据
            Jc_JcsdkzInfo item = result.Data;
            item.InfoState = InfoState.Delete;
            List<Jc_JcsdkzInfo> SendItemList = new List<Jc_JcsdkzInfo>();
            SendItemList.Add(item);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            //数据库操作
            _Repository.DeleteManualCrossControl(ManualCrossControlRequest.Id);

            //缓存操作
            ManualCrossControlCacheDeleteRequest DeleteManualCrossControlCacheRequest = new ManualCrossControlCacheDeleteRequest();
            DeleteManualCrossControlCacheRequest.ManualCrossControlInfo = item;
            _ManualCrossControlCacheService.DeleteManualCrossControlCache(DeleteManualCrossControlCacheRequest);

            //调用驱动重新加载控制信息
            DriverManualCrossControlReLoadRequest reLoadRequest = new DriverManualCrossControlReLoadRequest();
            _DriverManualCrossControlService.ReLoad(reLoadRequest);

            return Result;
        }
        /// <summary>
        /// 批量删除手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteManualCrossControls(ManualCrossControlsRequest ManualCrossControlRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_JcsdkzInfo> DeleteManualCrossControlInfos = ManualCrossControlRequest.ManualCrossControlInfos;

            ManualCrossControlCacheGetAllRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetAllRequest();
            var result = _ManualCrossControlCacheService.GetAllManualCrossControlCache(manualCrossControlCacheRequest);
            List<Jc_JcsdkzInfo> DeleteManualCrossControlCaches = result.Data;

            if (DeleteManualCrossControlInfos.Count < 1)
            {
                Result.Code = 1;
                Result.Message = "当前删除列表中无数据！";
                return Result;
            }
            foreach (Jc_JcsdkzInfo item in DeleteManualCrossControlInfos)
            {
                Jc_JcsdkzInfo olditem = DeleteManualCrossControlCaches.Find(a => a.ID == item.ID);
                //增加重复判断
                if (olditem == null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前删除列表中的数据不存在！";
                    return Result;
                }
            }
            //向网关同步数据
            List<Jc_JcsdkzInfo> SendItemList = DeleteManualCrossControlInfos;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
            {


                foreach (Jc_JcsdkzInfo item in DeleteManualCrossControlInfos)
                {
                    //数据库操作
                    _Repository.DeleteManualCrossControl(item.ID);

                    //缓存操作
                    ManualCrossControlCacheDeleteRequest DeleteManualCrossControlCacheRequest = new ManualCrossControlCacheDeleteRequest();
                    DeleteManualCrossControlCacheRequest.ManualCrossControlInfo = item;
                    _ManualCrossControlCacheService.DeleteManualCrossControlCache(DeleteManualCrossControlCacheRequest);
                }
            });

            //调用驱动重新加载控制信息
            DriverManualCrossControlReLoadRequest reLoadRequest = new DriverManualCrossControlReLoadRequest();
            _DriverManualCrossControlService.ReLoad(reLoadRequest);

            return Result;
        }
        /// <summary>
        /// 批量添加\更新\删除接口
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        public BasicResponse BatchOperationManualCrossControls(ManualCrossControlsRequest manualCrossControlRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_JcsdkzInfo> ManualCrossControlInfos = manualCrossControlRequest.ManualCrossControlInfos;

            //向网关同步数据
            List<Jc_JcsdkzInfo> SendItemList = ManualCrossControlInfos;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (Jc_JcsdkzInfo item in ManualCrossControlInfos)
                {
                   
                    if (item.InfoState == InfoState.AddNew) {
                        //数据库操作
                        var _jc_Jcsdkz = ObjectConverter.Copy<Jc_JcsdkzInfo, Jc_JcsdkzModel>(item);
                        var resultjc_Jcsdkz = _Repository.AddManualCrossControl(_jc_Jcsdkz);
                        //缓存操作
                        ManualCrossControlCacheAddRequest AddManualCrossControlCacheRequest = new ManualCrossControlCacheAddRequest();
                        AddManualCrossControlCacheRequest.ManualCrossControlInfo = item;
                        _ManualCrossControlCacheService.AddManualCrossControlCache(AddManualCrossControlCacheRequest);
                    }
                    else if (item.InfoState == InfoState.Modified)
                    {
                        //数据库操作
                        var _jc_Jcsdkz = ObjectConverter.Copy<Jc_JcsdkzInfo, Jc_JcsdkzModel>(item);
                        _Repository.UpdateManualCrossControl(_jc_Jcsdkz);
                        //缓存操作
                        ManualCrossControlCacheUpdateRequest UpdateManualCrossControlCacheRequest = new ManualCrossControlCacheUpdateRequest();
                        UpdateManualCrossControlCacheRequest.ManualCrossControlInfo = item;
                        _ManualCrossControlCacheService.UpdateManualCrossControlCache(UpdateManualCrossControlCacheRequest);
                    }
                    else if (item.InfoState == InfoState.Delete)
                    {
                        //数据库操作
                        _Repository.DeleteManualCrossControl(item.ID);
                        //缓存操作
                        ManualCrossControlCacheDeleteRequest DeleteManualCrossControlCacheRequest = new ManualCrossControlCacheDeleteRequest();
                        DeleteManualCrossControlCacheRequest.ManualCrossControlInfo = item;
                        _ManualCrossControlCacheService.DeleteManualCrossControlCache(DeleteManualCrossControlCacheRequest);
                    }                    
                }
            });

            //调用驱动重新加载控制信息
            DriverManualCrossControlReLoadRequest reLoadRequest = new DriverManualCrossControlReLoadRequest();
            _DriverManualCrossControlService.ReLoad(reLoadRequest);

            return Result;
        }
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlList(ManualCrossControlGetListRequest ManualCrossControlRequest)
        {
            var jc_Jcsdkzresponse = new BasicResponse<List<Jc_JcsdkzInfo>>();
            ManualCrossControlRequest.PagerInfo.PageIndex = ManualCrossControlRequest.PagerInfo.PageIndex - 1;
            if (ManualCrossControlRequest.PagerInfo.PageIndex < 0)
            {
                ManualCrossControlRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_JcsdkzModelLists = _Repository.GetManualCrossControlList(ManualCrossControlRequest.PagerInfo.PageIndex, ManualCrossControlRequest.PagerInfo.PageSize, out rowcount);
            var jc_JcsdkzInfoLists = new List<Jc_JcsdkzInfo>();
            foreach (var item in jc_JcsdkzModelLists)
            {
                var Jc_JcsdkzInfo = ObjectConverter.Copy<Jc_JcsdkzModel, Jc_JcsdkzInfo>(item);
                jc_JcsdkzInfoLists.Add(Jc_JcsdkzInfo);
            }
            jc_Jcsdkzresponse.Data = jc_JcsdkzInfoLists;
            return jc_Jcsdkzresponse;
        }
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlList()
        {
            var jc_Jcsdkzresponse = new BasicResponse<List<Jc_JcsdkzInfo>>();
            var jc_JcsdkzModelLists = _Repository.GetManualCrossControlList();
            var jc_JcsdkzInfoLists = new List<Jc_JcsdkzInfo>();
            foreach (var item in jc_JcsdkzModelLists)
            {
                var Jc_JcsdkzInfo = ObjectConverter.Copy<Jc_JcsdkzModel, Jc_JcsdkzInfo>(item);
                jc_JcsdkzInfoLists.Add(Jc_JcsdkzInfo);
            }
            jc_Jcsdkzresponse.Data = jc_JcsdkzInfoLists;
            return jc_Jcsdkzresponse;
        }
        public BasicResponse<Jc_JcsdkzInfo> GetManualCrossControlById(ManualCrossControlGetRequest ManualCrossControlRequest)
        {
            var result = _Repository.GetManualCrossControlById(ManualCrossControlRequest.Id);
            var jc_JcsdkzInfo = ObjectConverter.Copy<Jc_JcsdkzModel, Jc_JcsdkzInfo>(result);
            var jc_Jcsdkzresponse = new BasicResponse<Jc_JcsdkzInfo>();
            jc_Jcsdkzresponse.Data = jc_JcsdkzInfo;
            return jc_Jcsdkzresponse;
        }
        /// <summary>
        /// 获取所有手动/交叉控制缓存
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControl()
        {
            BasicResponse<List<Jc_JcsdkzInfo>> Result = new BasicResponse<List<Jc_JcsdkzInfo>>();
            ManualCrossControlCacheGetAllRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetAllRequest();
            var result = _ManualCrossControlCacheService.GetAllManualCrossControlCache(manualCrossControlCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过被控分站号查询手动/交叉控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByStationID(ManualCrossControlGetByStationIDRequest ManualCrossControlRequest)
        {
            BasicResponse<List<Jc_JcsdkzInfo>> Result = new BasicResponse<List<Jc_JcsdkzInfo>>();
            ManualCrossControlCacheGetByConditionRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByConditionRequest();
            manualCrossControlCacheRequest.Predicate = a => a.Bkpoint.Contains(ManualCrossControlRequest.StationID.ToString().PadLeft(3, '0'));
            var result = _ManualCrossControlCacheService.GetManualCrossControlCache(manualCrossControlCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过被控分站号查询手动控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlHandCtrlByStationID(ManualCrossControlGetByStationIDRequest ManualCrossControlRequest)
        {
            BasicResponse<List<Jc_JcsdkzInfo>> Result = new BasicResponse<List<Jc_JcsdkzInfo>>();
            ManualCrossControlCacheGetByConditionRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByConditionRequest();
            manualCrossControlCacheRequest.Predicate = a => a.Type == 0 && a.Bkpoint.Contains(ManualCrossControlRequest.StationID.ToString().PadLeft(3, '0'));
            var result = _ManualCrossControlCacheService.GetManualCrossControlCache(manualCrossControlCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过类型、被控测点号查询手动/交叉控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeBkPoint(ManualCrossControlGetByTypeBkPointRequest ManualCrossControlRequest)
        {
            BasicResponse<List<Jc_JcsdkzInfo>> Result = new BasicResponse<List<Jc_JcsdkzInfo>>();
            ManualCrossControlCacheGetByConditionRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByConditionRequest();
            manualCrossControlCacheRequest.Predicate = a => a.Bkpoint == ManualCrossControlRequest.BkPoint && a.Type == ManualCrossControlRequest.Type;
            var result = _ManualCrossControlCacheService.GetManualCrossControlCache(manualCrossControlCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过类型、主控测点号、被控测点号查询手动/交叉控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeZkPointBkPoint(ManualCrossControlGetByTypeZkPointBkPointRequest ManualCrossControlRequest)
        {
            BasicResponse<List<Jc_JcsdkzInfo>> Result = new BasicResponse<List<Jc_JcsdkzInfo>>();
            ManualCrossControlCacheGetByConditionRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByConditionRequest();
            manualCrossControlCacheRequest.Predicate = a => a.Bkpoint == ManualCrossControlRequest.BkPoint && a.ZkPoint == ManualCrossControlRequest.ZkPoint && a.Type == ManualCrossControlRequest.Type;
            var result = _ManualCrossControlCacheService.GetManualCrossControlCache(manualCrossControlCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过被控测点号查询手动/交叉控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByBkPoint(ManualCrossControlGetByBkPointRequest ManualCrossControlRequest)
        {
            BasicResponse<List<Jc_JcsdkzInfo>> Result = new BasicResponse<List<Jc_JcsdkzInfo>>();
            ManualCrossControlCacheGetByConditionRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByConditionRequest();
            manualCrossControlCacheRequest.Predicate = a => a.Bkpoint == ManualCrossControlRequest.BkPoint;
            var result = _ManualCrossControlCacheService.GetManualCrossControlCache(manualCrossControlCacheRequest);
            Result.Data = result.Data;
            return Result;
        }

        /// <summary>
        /// 向网关同步数据
        /// </summary>
        /// <param name="SendItemList"></param>
        /// <returns></returns>
        private bool SynchronousDataToGateway(List<Jc_JcsdkzInfo> SendItemList)
        {

            UpdateCacheDataRequest UpdateCache = new UpdateCacheDataRequest();
            List<DeviceAcrossControlInfo> UpdateCacheDataList = new List<DeviceAcrossControlInfo>();

            UpdateCacheDataList = ObjectConverter.CopyList<Jc_JcsdkzInfo, DeviceAcrossControlInfo>(SendItemList).ToList();
            foreach (DeviceAcrossControlInfo deviceInfo in UpdateCacheDataList)
            {
                deviceInfo.UniqueKey = deviceInfo.ID;
            }
            UpdateCache.DeviceAcrossControlList = UpdateCacheDataList;
            //调用RPC发送
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.UpdateCacheDataRequest);
            masProtocol.Protocol = UpdateCache;
            var result = RpcService.Send<GatewayRpcResponse>(masProtocol, RequestType.BusinessRequest);
            if (result == null)
            {
                return false;
            }
            return result.IsSuccess;
            //return true;
        }
        /// <summary>
        /// 通过类型，主控测点号查询手动/交叉控制
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeZkPoint(ManualCrossControlGetByTypeZkPointRequest request)
        {
            BasicResponse<List<Jc_JcsdkzInfo>> Result = new BasicResponse<List<Jc_JcsdkzInfo>>();
            ManualCrossControlCacheGetByConditionRequest manualCrossControlCacheRequest = new ManualCrossControlCacheGetByConditionRequest();
            manualCrossControlCacheRequest.Predicate = a => a.Type == request.Type && a.ZkPoint == request.ZkPoint;
            var result = _ManualCrossControlCacheService.GetManualCrossControlCache(manualCrossControlCacheRequest);
            Result.Data = result.Data;
            return Result;
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControlDetail()
        {
            var dt = _Repository.QueryTable("global_ManualCrossControlService_GetDetailFromManualCrossControl", 
                new object[] { (int)ControlType.LargeDataAnalyticsAreaPowerOff, (int)ControlType.LargeDataAnalyticsSensorAlarmControl });
            var lis = ObjectConverter.Copy<Jc_JcsdkzInfo>(dt);
            var ret = new BasicResponse<List<Jc_JcsdkzInfo>>()
            {
                Data = lis
            };
            return ret;
        }


        public BasicResponse DeleteOtherManualCrossControlFromDB()
        {
            BasicResponse Result = new BasicResponse();
            _Repository.DelteManualCrossControlFromDB();
            return Result;
        }
    }
}



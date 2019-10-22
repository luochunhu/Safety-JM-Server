using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.DeviceDefine;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Data;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.Processing.Rpc;
using Sys.DataCollection.Common.Rpc;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 设备类型定义服务
    /// </summary>
    public partial class DeviceDefineService : IDeviceDefineService
    {
        private IDeviceDefineRepository _Repository;
        private IDeviceDefineCacheService _DeviceDefineCacheService;
        private IDeviceTypeCacheService _DeviceTypeCacheService;
        private IDevicePropertyCacheService _DevicePropertyCacheService;
        private IDeviceClassCacheService _DeviceClassCacheService;
        private IEnumcodeRepository _EnumcodeRepository;


        public DeviceDefineService(IDeviceDefineRepository _Repository, IDeviceDefineCacheService _DeviceDefineCacheService, IDeviceTypeCacheService _DeviceTypeCacheService
            , IDevicePropertyCacheService _DevicePropertyCacheService, IDeviceClassCacheService _DeviceClassCacheService, IEnumcodeRepository _EnumcodeRepository)
        {
            this._Repository = _Repository;
            this._DeviceDefineCacheService = _DeviceDefineCacheService;
            this._DeviceTypeCacheService = _DeviceTypeCacheService;
            this._DevicePropertyCacheService = _DevicePropertyCacheService;
            this._DeviceClassCacheService = _DeviceClassCacheService;
            this._EnumcodeRepository = _EnumcodeRepository;
        }
        /// <summary>
        /// 添加设备类型
        /// </summary>
        /// <param name="jc_Devrequest"></param>
        /// <returns></returns>
        public BasicResponse AddDeviceDefine(DeviceDefineAddRequest DeviceDefineRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_DevInfo item = DeviceDefineRequest.Jc_DevInfo;
            //重复判断
            DeviceDefineCacheGetByKeyRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetByKeyRequest();
            DeviceDefineCacheRequest.Devid = item.Devid;
            var result = _DeviceDefineCacheService.GetPointDefineCacheByKey(DeviceDefineCacheRequest);
            if (result.Data != null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前添加的设备类型已存在！";
                return Result;
            }
            //向网关同步数据
            List<Jc_DevInfo> SendItemList = new List<Jc_DevInfo>();
            SendItemList.Add(item);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            //保存数据库
            var _jc_Dev = ObjectConverter.Copy<Jc_DevInfo, Jc_DevModel>(DeviceDefineRequest.Jc_DevInfo);
            var resultjc_Dev = _Repository.AddDeviceDefine(_jc_Dev);
            //更新缓存
            DeviceDefineCacheAddRequest deviceDefineCacheRequest = new DeviceDefineCacheAddRequest();
            deviceDefineCacheRequest.DeviceDefineInfo = item;
            _DeviceDefineCacheService.AddPointDefineCache(deviceDefineCacheRequest);

            return Result;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse AddDeviceDefines(DeviceDefinesRequest DeviceDefineRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_DevInfo> items = DeviceDefineRequest.Jc_DevsInfo;
            List<Jc_DevInfo> Jc_DevCaches = new List<Jc_DevInfo>();
            Jc_DevInfo olditem = null;

            DeviceDefineCacheGetAllRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetAllRequest();
            var result = _DeviceDefineCacheService.GetAllPointDefineCache(DeviceDefineCacheRequest);
            Jc_DevCaches = result.Data;

            foreach (Jc_DevInfo item in items)
            {
                olditem = Jc_DevCaches.Find(a => a.Devid == item.Devid);
                //增加重复判断
                if (olditem != null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前添加列表中的数据在数据库中已经存在！";
                    return Result;
                }
            }
            //向网关同步数据
            List<Jc_DevInfo> SendItemList = items;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
            {
                

                foreach (Jc_DevInfo item in items)
                {
                    //保存数据库
                    var _Jc_Dev = ObjectConverter.Copy<Jc_DevInfo, Jc_DevModel>(item);
                    var resultJc_Dev = _Repository.AddDeviceDefine(_Jc_Dev);

                }

                //添加到缓存
                DeviceDefineCacheBatchAddRequest BacthAddPointDefineRequest = new DeviceDefineCacheBatchAddRequest();
                BacthAddPointDefineRequest.DeviceDefineInfos = items;
                _DeviceDefineCacheService.BacthAddPointDefineCache(BacthAddPointDefineRequest);

            });

            return Result;
        }
        /// <summary>
        /// 更新设备类型
        /// </summary>
        /// <param name="jc_Devrequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateDeviceDefine(DeviceDefineUpdateRequest DeviceDefineRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_DevInfo item = DeviceDefineRequest.Jc_DevInfo;
            //重复判断
            DeviceDefineCacheGetByKeyRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetByKeyRequest();
            DeviceDefineCacheRequest.Devid = item.Devid;
            var result = _DeviceDefineCacheService.GetPointDefineCacheByKey(DeviceDefineCacheRequest);
            if (result.Data == null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前更新的设备类型不存在！";
                return Result;
            }
            //向网关同步数据
            List<Jc_DevInfo> SendItemList = new List<Jc_DevInfo>();
            SendItemList.Add(item);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            //保存数据库
            var _jc_Dev = ObjectConverter.Copy<Jc_DevInfo, Jc_DevModel>(DeviceDefineRequest.Jc_DevInfo);
            _Repository.UpdateDeviceDefine(_jc_Dev);
            //更新缓存
            DeviceDefineCacheUpdateRequest deviceDefineCacheRequest = new DeviceDefineCacheUpdateRequest();
            deviceDefineCacheRequest.DeviceDefineInfo = item;
            _DeviceDefineCacheService.UpdatePointDefineCahce(deviceDefineCacheRequest);

            return Result;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateDeviceDefines(DeviceDefinesRequest DeviceDefineRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_DevInfo> items = DeviceDefineRequest.Jc_DevsInfo;
            List<Jc_DevInfo> Jc_DevCaches = new List<Jc_DevInfo>();
            Jc_DevInfo olditem = null;

            DeviceDefineCacheGetAllRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetAllRequest();
            var result = _DeviceDefineCacheService.GetAllPointDefineCache(DeviceDefineCacheRequest);
            Jc_DevCaches = result.Data;

            foreach (Jc_DevInfo item in items)
            {
                olditem = Jc_DevCaches.Find(a => a.Devid == item.Devid);
                //增加重复判断
                if (olditem == null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前更新列表中的数据在数据库中不存在！";
                    return Result;
                }
            }
            //向网关同步数据
            List<Jc_DevInfo> SendItemList = items;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
            {
                

                foreach (Jc_DevInfo item in items)
                {
                    //保存数据库
                    var _Jc_Dev = ObjectConverter.Copy<Jc_DevInfo, Jc_DevModel>(item);
                    var resultJc_Dev = _Repository.AddDeviceDefine(_Jc_Dev);

                }

                //更新到缓存
                DeviceDefineCacheBatchUpdateRequest BacthUpdatePointDefineRequest = new DeviceDefineCacheBatchUpdateRequest();
                BacthUpdatePointDefineRequest.DeviceDefineInfos = items;
                _DeviceDefineCacheService.BatchUpdatePointDefineCache(BacthUpdatePointDefineRequest);

            });

            return Result;
        }
        /// <summary>
        /// 删除设备类型
        /// </summary>
        /// <param name="jc_Devrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteDeviceDefine(DeviceDefineDeleteRequest DeviceDefineRequest)
        {
            BasicResponse Result = new BasicResponse();
            //重复判断
            DeviceDefineCacheGetByKeyRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetByKeyRequest();
            DeviceDefineCacheRequest.Devid = DeviceDefineRequest.Id;
            var result = _DeviceDefineCacheService.GetPointDefineCacheByKey(DeviceDefineCacheRequest);
            if (result.Data == null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前删除的设备类型不存在！";
                return Result;
            }

            //向网关同步数据
            Jc_DevInfo DeleteDevInfo = result.Data;
            DeleteDevInfo.InfoState = InfoState.Delete;
            List<Jc_DevInfo> SendItemList = new List<Jc_DevInfo>();
            SendItemList.Add(DeleteDevInfo);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            //保存数据库
            _Repository.DeleteDeviceDefine(DeleteDevInfo.ID);

            //更新缓存
            DeviceDefineCacheDeleteRequest deviceDefineCacheRequest = new DeviceDefineCacheDeleteRequest();
            deviceDefineCacheRequest.DeviceDefineInfo = DeleteDevInfo;
            _DeviceDefineCacheService.DeletePointDefineCache(deviceDefineCacheRequest);

            return Result;
        }
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineList(DeviceDefineGetListRequest DeviceDefineRequest)
        {
            var jc_Devresponse = new BasicResponse<List<Jc_DevInfo>>();
            DeviceDefineRequest.PagerInfo.PageIndex = DeviceDefineRequest.PagerInfo.PageIndex - 1;
            if (DeviceDefineRequest.PagerInfo.PageIndex < 0)
            {
                DeviceDefineRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_DevModelLists = _Repository.GetDeviceDefineList(DeviceDefineRequest.PagerInfo.PageIndex, DeviceDefineRequest.PagerInfo.PageSize, out rowcount);
            var jc_DevInfoLists = new List<Jc_DevInfo>();
            foreach (var item in jc_DevModelLists)
            {
                var Jc_DevInfo = ObjectConverter.Copy<Jc_DevModel, Jc_DevInfo>(item);
                jc_DevInfoLists.Add(Jc_DevInfo);
            }
            jc_Devresponse.Data = jc_DevInfoLists;
            return jc_Devresponse;
        }
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineList()
        {
            var jc_Devresponse = new BasicResponse<List<Jc_DevInfo>>();           
            var jc_DevModelLists = _Repository.GetDeviceDefineList();
            var jc_DevInfoLists = new List<Jc_DevInfo>();
            foreach (var item in jc_DevModelLists)
            {
                var Jc_DevInfo = ObjectConverter.Copy<Jc_DevModel, Jc_DevInfo>(item);
                jc_DevInfoLists.Add(Jc_DevInfo);
            }
            jc_Devresponse.Data = jc_DevInfoLists;
            return jc_Devresponse;
        }
        public BasicResponse<Jc_DevInfo> GetDeviceDefineById(DeviceDefineGetRequest DeviceDefineRequest)
        {
            var result = _Repository.GetDeviceDefineById(DeviceDefineRequest.Id);
            var jc_DevInfo = ObjectConverter.Copy<Jc_DevModel, Jc_DevInfo>(result);
            var jc_Devresponse = new BasicResponse<Jc_DevInfo>();
            jc_Devresponse.Data = jc_DevInfo;
            return jc_Devresponse;
        }
        /// <summary>
        /// 获取所有设备类型缓存信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DevInfo>> GetAllDeviceDefineCache()
        {
            BasicResponse<List<Jc_DevInfo>> Result = new BasicResponse<List<Jc_DevInfo>>();
            DeviceDefineCacheGetAllRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetAllRequest();
            var result = _DeviceDefineCacheService.GetAllPointDefineCache(DeviceDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }

        public BasicResponse<List<Jc_DevInfo>> GetNotMonitoringAllDeviceDefineCache()
        {
            return new BasicResponse<List<Jc_DevInfo>>();
        }

        /// <summary>
        /// 根据DevId获取设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<Jc_DevInfo> GetDeviceDefineCacheByDevId(DeviceDefineGetByDevIdRequest DeviceDefineRequest)
        {
            BasicResponse<Jc_DevInfo> Result = new BasicResponse<Jc_DevInfo>();
            DeviceDefineCacheGetByKeyRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetByKeyRequest();
            DeviceDefineCacheRequest.Devid = DeviceDefineRequest.DevId;
            var result = _DeviceDefineCacheService.GetPointDefineCacheByKey(DeviceDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }

        /// <summary>
        /// 通过设备性质查找设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevpropertID(DeviceDefineGetByDevpropertIDRequest DeviceDefineRequest)
        {
            BasicResponse<List<Jc_DevInfo>> Result = new BasicResponse<List<Jc_DevInfo>>();
            DeviceDefineCacheGetByConditonRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetByConditonRequest();
            DeviceDefineCacheRequest.Predicate = a => a.Type == DeviceDefineRequest.DevpropertID;
            var result = _DeviceDefineCacheService.GetPointDefineCache(DeviceDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过设备性质、设备型号查找设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevpropertIDDevModelID(DeviceDefineGetByDevpropertIDDevModelIDRequest DeviceDefineRequest)
        {
            BasicResponse<List<Jc_DevInfo>> Result = new BasicResponse<List<Jc_DevInfo>>();
            DeviceDefineCacheGetByConditonRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetByConditonRequest();
            DeviceDefineCacheRequest.Predicate = a => a.Type == DeviceDefineRequest.DevpropertID && a.Bz4 == DeviceDefineRequest.DevModelID;
            var result = _DeviceDefineCacheService.GetPointDefineCache(DeviceDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过设备种类查找设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevClassID(DeviceDefineGetByDevClassIDRequest DeviceDefineRequest)
        {
            BasicResponse<List<Jc_DevInfo>> Result = new BasicResponse<List<Jc_DevInfo>>();
            DeviceDefineCacheGetByConditonRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetByConditonRequest();
            DeviceDefineCacheRequest.Predicate = a => a.Bz3 == DeviceDefineRequest.DevClassID;
            var result = _DeviceDefineCacheService.GetPointDefineCache(DeviceDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        
        /// <summary>
        /// 获取所有设备型号
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceTypeCache()
        {
            BasicResponse<List<EnumcodeInfo>> Result = new BasicResponse<List<EnumcodeInfo>>();
            DeviceTypeCacheGetAllRequest deviceTypeCacheRequest = new DeviceTypeCacheGetAllRequest();
            var result = _DeviceTypeCacheService.GetAllDeviceTypeCache(deviceTypeCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache()
        {
            BasicResponse<List<EnumcodeInfo>> Result = new BasicResponse<List<EnumcodeInfo>>();
            DeviceClassCacheGetAllRequest DeviceClassCacheRequest = new DeviceClassCacheGetAllRequest();
            var result = _DeviceClassCacheService.GetAllDeviceClassCache(DeviceClassCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取所有设备性质
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache()
        {
            BasicResponse<List<EnumcodeInfo>> Result = new BasicResponse<List<EnumcodeInfo>>();
            DevicePropertyCacheGetAllRequest devicePropertyCacheRequest = new DevicePropertyCacheGetAllRequest();
            var result = _DevicePropertyCacheService.GetAllDevicePropertyCache(devicePropertyCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取所有解析驱动信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDriverInf()
        {
            BasicResponse<List<EnumcodeInfo>> Result = new BasicResponse<List<EnumcodeInfo>>();
            var result = ObjectConverter.CopyList<EnumcodeModel, EnumcodeInfo>(_EnumcodeRepository.GetEnumcodeList());
            Result.Data = result.ToList().FindAll(a => a.EnumTypeID == "7");
            return Result;
        }
        /// <summary>
        /// 获取当前最大的设备类型ID
        /// </summary>
        /// <returns></returns>
        public BasicResponse<long> GetMaxDeviceDefineId()
        {
            BasicResponse<long> Result = new BasicResponse<long>();
            long MaxDeviceId = 0;
            DeviceDefineCacheGetAllRequest DeviceDefineCacheRequest = new DeviceDefineCacheGetAllRequest();
            var result = _DeviceDefineCacheService.GetAllPointDefineCache(DeviceDefineCacheRequest);
            foreach (Jc_DevInfo item in result.Data)
            {
                if (item.InfoState == InfoState.Delete)
                {
                    continue;
                }
                if (long.Parse(item.Devid) > MaxDeviceId)
                {
                    MaxDeviceId = long.Parse(item.Devid);
                }
            }
            Result.Data = MaxDeviceId;
            return Result;
        }
        /// <summary>
        /// 向网关同步数据
        /// </summary>
        /// <param name="SendItemList"></param>
        /// <returns></returns>
        private bool SynchronousDataToGateway(List<Jc_DevInfo> SendItemList)
        {

            UpdateCacheDataRequest UpdateCache = new UpdateCacheDataRequest();
            List<DeviceTypeInfo> UpdateCacheDataList = new List<DeviceTypeInfo>();

            UpdateCacheDataList = ObjectConverter.CopyList<Jc_DevInfo, DeviceTypeInfo>(SendItemList).ToList();
            foreach (DeviceTypeInfo deviceInfo in UpdateCacheDataList)
            {
                deviceInfo.UniqueKey = deviceInfo.Devid;
            }
            UpdateCache.DeviceTypeList = UpdateCacheDataList;
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
    }
}



using Sys.Safety.ServiceContract.Cache;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:设备定义缓存业务
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class DeviceDefineCacheService : IDeviceDefineCacheService
    {
        public BasicResponse AddPointDefineCache(DeviceDefineCacheAddRequest deviceDefineCacheRequest)
        {
            DeviceDefineCache.DeviceDefineCahceInstance.AddItem(deviceDefineCacheRequest.DeviceDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse BacthAddPointDefineCache(DeviceDefineCacheBatchAddRequest deviceDefineCacheRequest)
        {
            DeviceDefineCache.DeviceDefineCahceInstance.AddItems(deviceDefineCacheRequest.DeviceDefineInfos);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdatePointDefineCache(DeviceDefineCacheBatchUpdateRequest deviceDefineCacheRequest)
        {
            DeviceDefineCache.DeviceDefineCahceInstance.UpdateItems(deviceDefineCacheRequest.DeviceDefineInfos);
            return new BasicResponse();
        }

        public BasicResponse DeletePointDefineCache(DeviceDefineCacheDeleteRequest deviceDefineCacheRequest)
        {
            DeviceDefineCache.DeviceDefineCahceInstance.DeleteItem(deviceDefineCacheRequest.DeviceDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse<List<Jc_DevInfo>> GetAllPointDefineCache(DeviceDefineCacheGetAllRequest deviceDefineCacheRequest)
        {
            var deviceDefineCache = DeviceDefineCache.DeviceDefineCahceInstance.Query();
            var deviceDefineCacheResponse = new BasicResponse<List<Jc_DevInfo>>();
            deviceDefineCacheResponse.Data = deviceDefineCache;
            return deviceDefineCacheResponse;
        }

        public BasicResponse<List<Jc_DevInfo>> GetPointDefineCache(DeviceDefineCacheGetByConditonRequest deviceDefineCacheRequest)
        {
            var deviceDefineCache = DeviceDefineCache.DeviceDefineCahceInstance.Query(deviceDefineCacheRequest.Predicate);
            var deviceDefineCacheResponse = new BasicResponse<List<Jc_DevInfo>>();
            deviceDefineCacheResponse.Data = deviceDefineCache;
            return deviceDefineCacheResponse;
        }

        public BasicResponse<Jc_DevInfo> GetPointDefineCacheByKey(DeviceDefineCacheGetByKeyRequest deviceDefineCacheRequest)
        {
            var deviceDefineCache = DeviceDefineCache.DeviceDefineCahceInstance.Query(devdefine => devdefine.Devid == deviceDefineCacheRequest.Devid).FirstOrDefault();
            var deviceDefineCacheResponse = new BasicResponse<Jc_DevInfo>();
            deviceDefineCacheResponse.Data = deviceDefineCache;
            return deviceDefineCacheResponse;
        }

        public BasicResponse LoadDeviceDefineCache(DeviceDefineCacheLoadRequest deviceDefineCacheRequest)
        {
            DeviceDefineCache.DeviceDefineCahceInstance.Load();
            //加载设备定义基本信息之后，加载设备定义拓展属性
            var deviceDefineList = DeviceDefineCache.DeviceDefineCahceInstance.Query();
            if (deviceDefineList.Any())
            {
                LoadDeviceDefineExtendProperty(deviceDefineList);
                //DeviceDefineCache.DeviceDefineCahceInstance.UpdateItems(deviceDefineList);//取消缓存深复制，存在引用关系  20170814
            }
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineCahce(DeviceDefineCacheUpdateRequest deviceDefineCacheRequest)
        {
            DeviceDefineCache.DeviceDefineCahceInstance.UpdateItem(deviceDefineCacheRequest.DeviceDefineInfo);
            return new BasicResponse();
        }

        /// <summary>
        /// 加载设备定义拓展属性
        /// </summary>
        /// <param name="deviceDefines"></param>
        private void LoadDeviceDefineExtendProperty(List<Jc_DevInfo> deviceDefines)
        {
            //设备种类信息
            var deviceClassList = DeviceClassCache.DeviceClassCahceInstance.Query();
            //设备性质信息
            var devicePropertyList = DevicePropertyCache.DeviceDefineCahceInstance.Query();
            //设备型号信息
            var deviceTypeList = DeviceTypeCache.DeviceTypeCahceInstance.Query();

            deviceDefines.ForEach(deviceDefine =>
            {
                if (deviceClassList.Any())
                {
                    var deviceClass = deviceClassList.FirstOrDefault(devc => devc.LngEnumValue == deviceDefine.Bz3);
                    deviceDefine.DevClass = deviceClass == null ? string.Empty : deviceClass.StrEnumDisplay;
                }
                if (devicePropertyList.Any())
                {
                    var deviceProperty = devicePropertyList.FirstOrDefault(devc => devc.LngEnumValue == deviceDefine.Type);
                    deviceDefine.DevProperty = deviceProperty == null ? string.Empty : deviceProperty.StrEnumDisplay;
                }
                if (deviceTypeList.Any())
                {
                    var deviceType = deviceTypeList.FirstOrDefault(devc => devc.LngEnumValue == deviceDefine.Bz4);
                    deviceDefine.DevModel = deviceType == null ? string.Empty : deviceType.StrEnumDisplay;
                }

            });
        }
    }
}

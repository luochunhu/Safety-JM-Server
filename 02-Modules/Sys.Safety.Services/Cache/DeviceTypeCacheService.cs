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
    /// 描述:设备型号缓存业务
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class DeviceTypeCacheService : IDeviceTypeCacheService
    {
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceTypeCache(DeviceTypeCacheGetAllRequest deviceTypeCacheRequest)
        {
            var deviceTypeCache = DeviceTypeCache.DeviceTypeCahceInstance.Query();
            var configCacheResponse = new BasicResponse<List<EnumcodeInfo>>();
            configCacheResponse.Data = deviceTypeCache;
            return configCacheResponse;
        }

        public BasicResponse<EnumcodeInfo> GetDeviceTypeByKey(DeviceTypeCacheGetByKeyRequest deviceTypeCacheRequest)
        {
            var deviceTypeCache = DeviceTypeCache.DeviceTypeCahceInstance.Query(deviceType => deviceType.LngEnumValue == deviceTypeCacheRequest.LngEnumValue).FirstOrDefault();
            var configCacheResponse = new BasicResponse<EnumcodeInfo>();
            configCacheResponse.Data = deviceTypeCache;
            return configCacheResponse;
        }

        public BasicResponse<List<EnumcodeInfo>> GetDeviceTypeCache(DeviceTypeCacheGetByConditionRequest deviceTypeCacheRequest)
        {
            var deviceTypeCache = DeviceTypeCache.DeviceTypeCahceInstance.Query(deviceTypeCacheRequest.Predicate);
            var configCacheResponse = new BasicResponse<List<EnumcodeInfo>>();
            configCacheResponse.Data = deviceTypeCache;
            return configCacheResponse;
        }

        public BasicResponse<bool> IsExistsDeviceType(DeviceTypeCacheIsExistsRequest deviceTypeCacheRequest)
        {
            var deviceTypeCache = DeviceTypeCache.DeviceTypeCahceInstance.Query(deviceType => deviceType.LngEnumValue == deviceTypeCacheRequest.LngEnumValue).FirstOrDefault();
            var configCacheResponse = new BasicResponse<bool>();
            configCacheResponse.Data = deviceTypeCache != null;
            return configCacheResponse;
        }

        public BasicResponse LoadDeviceTypeCache(DeviceTypeCacheLoadRequest deviceTypeCacheRequest)
        {
            DeviceTypeCache.DeviceTypeCahceInstance.Load();
            return new BasicResponse();
        }
    }
}

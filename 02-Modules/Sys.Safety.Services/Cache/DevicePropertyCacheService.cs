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
    /// 描述:设备性质缓存业务
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class DevicePropertyCacheService : IDevicePropertyCacheService
    {
        public BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache(DevicePropertyCacheGetAllRequest devicePropertyCacheRequest)
        {
            var devicePropertyCache = DevicePropertyCache.DeviceDefineCahceInstance.Query();
            var configCacheResponse = new BasicResponse<List<EnumcodeInfo>>();
            configCacheResponse.Data = devicePropertyCache;
            return configCacheResponse;
        }

        public BasicResponse<EnumcodeInfo> GetDevicePropertyByKey(DevicePropertyCacheGetByKeyRequest devicePropertyCacheRequest)
        {
            var devicePropertyCache = DevicePropertyCache.DeviceDefineCahceInstance.Query(deviceClass => deviceClass.LngEnumValue == devicePropertyCacheRequest.LngEnumValue).FirstOrDefault();
            var configCacheResponse = new BasicResponse<EnumcodeInfo>();
            configCacheResponse.Data = devicePropertyCache;
            return configCacheResponse;
        }

        public BasicResponse<List<EnumcodeInfo>> GetDevicePropertyCache(DevicePropertyCacheGetByConditionRequest devicePropertyCacheRequest)
        {
            var devicePropertyCache = DevicePropertyCache.DeviceDefineCahceInstance.Query(devicePropertyCacheRequest.Predicate);
            var configCacheResponse = new BasicResponse<List<EnumcodeInfo>>();
            configCacheResponse.Data = devicePropertyCache;
            return configCacheResponse;
        }

        public BasicResponse<bool> IsExistsDeviceProperty(DevicePropertyCacheIsExistsRequest devicePropertyCacheRequest)
        {
            var devicePropertyCache = DevicePropertyCache.DeviceDefineCahceInstance.Query(deviceClass => deviceClass.LngEnumValue == devicePropertyCacheRequest.LngEnumValue).FirstOrDefault();
            var configCacheResponse = new BasicResponse<bool>();
            configCacheResponse.Data = devicePropertyCache != null;
            return configCacheResponse;
        }

        public BasicResponse LoadDevicePropertyCache(DevicePropertyCacheLoadRequest devicePropertyCacheRequest)
        {
            DevicePropertyCache.DeviceDefineCahceInstance.Load();
            return new BasicResponse();
        }
    }
}

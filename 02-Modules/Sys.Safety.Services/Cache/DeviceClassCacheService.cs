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
    /// 描述:设备种类缓存业务
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class DeviceClassCacheService : IDeviceClassCacheService
    {
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache(DeviceClassCacheGetAllRequest deviceClassCacheRequest)
        {
            var deviceClassCache = DeviceClassCache.DeviceClassCahceInstance.Query();
            var configCacheResponse = new BasicResponse<List<EnumcodeInfo>>();
            configCacheResponse.Data = deviceClassCache;
            return configCacheResponse;
        }

        public BasicResponse<List<EnumcodeInfo>> GetDeviceClassByDeviciProperty(DeviceClassCacheGetByDevicePropertyRequest deviceClassCacheRequest)
        {
            var deviceClassCache = DeviceClassCache.DeviceClassCahceInstance.Query(deviceClass=>deviceClass.LngEnumValue3==deviceClassCacheRequest.DevicePropertyId.ToString());
            var configCacheResponse = new BasicResponse<List<EnumcodeInfo>>();
            configCacheResponse.Data = deviceClassCache;
            return configCacheResponse;
        }

        public BasicResponse<EnumcodeInfo> GetDeviceClassByKey(DeviceClassCacheGetByKeyRequest deviceClassCacheRequest)
        {
            var deviceClassCache = DeviceClassCache.DeviceClassCahceInstance.Query(deviceClass => deviceClass.LngEnumValue == deviceClassCacheRequest.LngEnumValue).FirstOrDefault();
            var configCacheResponse = new BasicResponse<EnumcodeInfo>();
            configCacheResponse.Data = deviceClassCache;
            return configCacheResponse;
        }

        public BasicResponse<List<EnumcodeInfo>> GetDeviceClassCache(DeviceClassCacheGetByConditionRequest deviceClassCacheRequest)
        {
            var deviceClassCache = DeviceClassCache.DeviceClassCahceInstance.Query(deviceClassCacheRequest.Predicate);
            var configCacheResponse = new BasicResponse<List<EnumcodeInfo>>();
            configCacheResponse.Data = deviceClassCache;
            return configCacheResponse;
        }

        public BasicResponse<bool> IsExistsDeviceClass(DeviceClassCacheIsExistsRequest deviceClassCacheRequest)
        {
            var deviceClassCache = DeviceClassCache.DeviceClassCahceInstance.Query(deviceClass => deviceClass.LngEnumValue == deviceClassCacheRequest.LngEnumValue).FirstOrDefault();
            var configCacheResponse = new BasicResponse<bool>();
            configCacheResponse.Data = deviceClassCache != null;
            return configCacheResponse;
        }

        public BasicResponse LoadDeviceClassCache(DeviceClassCacheLoadRequest deviceClassCacheRequest)
        {
            DeviceClassCache.DeviceClassCahceInstance.Load();
            return new BasicResponse();
        }
    }
}

using Sys.Safety.ServiceContract.Cache;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using System.Threading;
using System;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    ///  自动挂接设备缓存操作服务 luoch 20170614
    /// </summary>
    public class AutomaticArticulatedDeviceCacheService : IAutomaticArticulatedDeviceCacheService
    {

        public AutomaticArticulatedDeviceCacheService()
        {

        }

        public BasicResponse AddAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheAddRequest AutomaticArticulatedDeviceCacheRequest)
        {
            AutomaticArticulatedDeviceCache.AutomaticArticulatedDeviceCahceInstance.AddItem(AutomaticArticulatedDeviceCacheRequest.AutomaticArticulatedDeviceInfo);
            return new BasicResponse();
        }
        public BasicResponse UpdateAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheUpdateRequest AutomaticArticulatedDeviceCacheRequest)
        {
            AutomaticArticulatedDeviceCache.AutomaticArticulatedDeviceCahceInstance.UpdateItem(AutomaticArticulatedDeviceCacheRequest.AutomaticArticulatedDeviceInfo);
            return new BasicResponse();
        }
        public BasicResponse DeleteAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheDeleteRequest AutomaticArticulatedDeviceCacheRequest)
        {
            AutomaticArticulatedDeviceCache.AutomaticArticulatedDeviceCahceInstance.DeleteItem(AutomaticArticulatedDeviceCacheRequest.AutomaticArticulatedDeviceInfo);
            return new BasicResponse();
        }

        public BasicResponse<List<AutomaticArticulatedDeviceInfo>> GetAllAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheGetAllRequest AutomaticArticulatedDeviceCacheRequest)
        {
            var automaticArticulatedDeviceCache = AutomaticArticulatedDeviceCache.AutomaticArticulatedDeviceCahceInstance.Query();
            var automaticArticulatedDeviceCacheResponse = new BasicResponse<List<AutomaticArticulatedDeviceInfo>>();
            automaticArticulatedDeviceCacheResponse.Data = automaticArticulatedDeviceCache;
            return automaticArticulatedDeviceCacheResponse;
        }

        public BasicResponse<List<AutomaticArticulatedDeviceInfo>> GetAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheGetByConditionRequest AutomaticArticulatedDeviceCacheRequest)
        {
            var automaticArticulatedDeviceCache = AutomaticArticulatedDeviceCache.AutomaticArticulatedDeviceCahceInstance.Query(AutomaticArticulatedDeviceCacheRequest.Pridicate);
            var automaticArticulatedDeviceCacheResponse = new BasicResponse<List<AutomaticArticulatedDeviceInfo>>();
            automaticArticulatedDeviceCacheResponse.Data = automaticArticulatedDeviceCache;
            return automaticArticulatedDeviceCacheResponse;
        }

        public BasicResponse<AutomaticArticulatedDeviceInfo> GetAutomaticArticulatedDeviceCacheByKey(AutomaticArticulatedDeviceCacheGetByKeyRequest AutomaticArticulatedDeviceCacheRequest)
        {
            var automaticArticulatedDeviceCache = AutomaticArticulatedDeviceCache.AutomaticArticulatedDeviceCahceInstance.Query(runlog => runlog.ID == AutomaticArticulatedDeviceCacheRequest.AutomaticArticulatedDeviceId).FirstOrDefault();
            var automaticArticulatedDeviceCacheResponse = new BasicResponse<AutomaticArticulatedDeviceInfo>();
            automaticArticulatedDeviceCacheResponse.Data = automaticArticulatedDeviceCache;
            return automaticArticulatedDeviceCacheResponse;
        }

    }
}

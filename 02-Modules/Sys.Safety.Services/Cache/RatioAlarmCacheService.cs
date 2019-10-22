using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;
using System.Threading;
using Basic.Framework.Logging;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-7-26
    /// 描述:倍数报警缓存业务
    /// </summary>
    public class RatioAlarmCacheService : IRatioAlarmCacheService
    {

        public BasicResponse AddAlarmCache(RatioAlarmCacheAddRequest alarmCacheRequest)
        {
            RatioAlarmCache.AlarmCacheInstance.AddItem(alarmCacheRequest.AlarmInfo);
            return new BasicResponse();
        }

        public BasicResponse BacthAddAlarmCache(RatioAlarmCacheBatchAddRequest alarmCacheRequest)
        {
            RatioAlarmCache.AlarmCacheInstance.AddItems(alarmCacheRequest.AlarmInfos);
            return new BasicResponse();
        }

        public BasicResponse UpdateAlarmCahce(RatioAlarmCacheUpdateRequest alarmCacheRequest)
        {
            RatioAlarmCache.AlarmCacheInstance.UpdateItem(alarmCacheRequest.AlarmInfo);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdateAlarmCache(RatioAlarmCacheBatchUpdateRequest alarmCacheRequest)
        {
            RatioAlarmCache.AlarmCacheInstance.AddItems(alarmCacheRequest.AlarmInfos);
            return new BasicResponse();
        }

        public BasicResponse<List<JC_MbInfo>> GetAllAlarmCache(RatioAlarmCacheGetAllRequest alarmCacheRequest)
        {
            var alarmCache = RatioAlarmCache.AlarmCacheInstance.Query();
            var alarmCacheResponse = new BasicResponse<List<JC_MbInfo>>();
            alarmCacheResponse.Data = alarmCache;
            return alarmCacheResponse;
        }

        public BasicResponse<JC_MbInfo> GetAlarmCacheByKey(RatioAlarmCacheGetByKeyRequest alarmCacheRequest)
        {
            var alarmCache = RatioAlarmCache.AlarmCacheInstance.Query(alarm => alarm.Id == alarmCacheRequest.Id).FirstOrDefault();
            var alarmCacheResponse = new BasicResponse<JC_MbInfo>();
            alarmCacheResponse.Data = alarmCache;
            return alarmCacheResponse;
        }

        public BasicResponse<List<JC_MbInfo>> GetAlarmCache(RatioAlarmCacheGetByConditonRequest alarmCacheRequest)
        {
            var alarmCache = RatioAlarmCache.AlarmCacheInstance.Query(alarmCacheRequest.Predicate);
            var alarmCacheResponse = new BasicResponse<List<JC_MbInfo>>();
            alarmCacheResponse.Data = alarmCache;
            return alarmCacheResponse;
        }

        public BasicResponse<List<JC_MbInfo>> GetAlarmCacheByStime(RatioAlarmCacheGetByStimeRequest alarmCacheRequest)
        {
            var alarmCache = RatioAlarmCache.AlarmCacheInstance.Query(alarm => alarm.Stime > alarmCacheRequest.Stime);
            var alarmCacheResponse = new BasicResponse<List<JC_MbInfo>>();
            alarmCacheResponse.Data = alarmCache;
            return alarmCacheResponse;
        }

        public BasicResponse BatchDeleteAlarmCache(RatioAlarmCacheBatchDeleteRequest alarmCacheRequest)
        {
            RatioAlarmCache.AlarmCacheInstance.DeleteItems(alarmCacheRequest.AlarmInfos);
            return new BasicResponse();
        }

        public BasicResponse UpdateAlarmInfoProperties(RatioAlarmCacheUpdatePropertiesRequest alarmCacheRequest)
        {
            RatioAlarmCache.AlarmCacheInstance.UpdateAlarmInfoProperties(alarmCacheRequest.AlarmKey, alarmCacheRequest.UpdateItems);
            return new BasicResponse();
        }
    }
}

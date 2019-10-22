using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Alarm;
using Sys.Safety.DataContract;
using System.Data;
using System.Web.Http;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.WebApi.AlarmModule
{
    /// <summary>
    /// 2017.7.27 by
    /// </summary>
    public class RatioAlarmServiceController : Basic.Framework.Web.WebApi.BasicApiController, IRatioAlarmCacheService
    {
        private IRatioAlarmCacheService alarmService = ServiceFactory.Create<IRatioAlarmCacheService>();

        [HttpPost]
        [Route("v1/ratioalarm/AddAlarmCache")]
        public BasicResponse AddAlarmCache(RatioAlarmCacheAddRequest alarmCacheRequest)
        {
            return alarmService.AddAlarmCache(alarmCacheRequest);
        }

        [HttpPost]
        [Route("v1/ratioalarm/BacthAddAlarmCache")]
        public BasicResponse BacthAddAlarmCache(RatioAlarmCacheBatchAddRequest alarmCacheRequest)
        {
            return alarmService.BacthAddAlarmCache(alarmCacheRequest);
        }

        [HttpPost]
        [Route("v1/ratioalarm/UpdateAlarmCahce")]
        public BasicResponse UpdateAlarmCahce(RatioAlarmCacheUpdateRequest alarmCacheRequest)
        {
            return alarmService.UpdateAlarmCahce(alarmCacheRequest);
        }

        [HttpPost]
        [Route("v1/ratioalarm/BatchUpdateAlarmCache")]
        public BasicResponse BatchUpdateAlarmCache(RatioAlarmCacheBatchUpdateRequest alarmCacheRequest)
        {
            return alarmService.BatchUpdateAlarmCache(alarmCacheRequest);
        }

        [HttpPost]
        [Route("v1/ratioalarm/GetAllAlarmCache")]
        public BasicResponse<List<JC_MbInfo>> GetAllAlarmCache(RatioAlarmCacheGetAllRequest alarmCacheRequest)
        {
            return alarmService.GetAllAlarmCache(alarmCacheRequest);
        }

        [HttpPost]
        [Route("v1/ratioalarm/GetAlarmCacheByKey")]
        public BasicResponse<JC_MbInfo> GetAlarmCacheByKey(RatioAlarmCacheGetByKeyRequest alarmCacheRequest)
        {
            return alarmService.GetAlarmCacheByKey(alarmCacheRequest);
        }
        [HttpPost]
        [Route("v1/ratioalarm/GetAlarmCache")]
        public BasicResponse<List<JC_MbInfo>> GetAlarmCache(RatioAlarmCacheGetByConditonRequest alarmCacheRequest)
        {
            return alarmService.GetAlarmCache(alarmCacheRequest);
        }

        [HttpPost]
        [Route("v1/ratioalarm/BatchDeleteAlarmCache")]
        public BasicResponse BatchDeleteAlarmCache(RatioAlarmCacheBatchDeleteRequest alarmCacheRequest)
        {
            return alarmService.BatchDeleteAlarmCache(alarmCacheRequest);
        }

        [HttpPost]
        [Route("v1/ratioalarm/UpdateAlarmInfoProperties")]
        public BasicResponse UpdateAlarmInfoProperties(RatioAlarmCacheUpdatePropertiesRequest alarmCacheRequest)
        {
            return alarmService.UpdateAlarmInfoProperties(alarmCacheRequest);
        }


        [HttpPost]
        [Route("v1/ratioalarm/GetAlarmCacheByStime")]
        public BasicResponse<List<JC_MbInfo>> GetAlarmCacheByStime(RatioAlarmCacheGetByStimeRequest alarmCacheRequest)
        {
            return alarmService.GetAlarmCacheByStime(alarmCacheRequest);
        }
    }
}

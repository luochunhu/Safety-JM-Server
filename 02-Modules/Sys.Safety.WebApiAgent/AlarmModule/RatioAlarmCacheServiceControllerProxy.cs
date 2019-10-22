using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Alarm;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;
using Sys.Safety.DataContract;
using System.Data;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;


namespace Sys.Safety.WebApiAgent
{
    /// <summary>
    /// 2017.7.27 by
    /// </summary>
    public class RatioAlarmCacheServiceControllerProxy : BaseProxy, IRatioAlarmCacheService
    {

        /// <summary>
        /// 添加报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse AddAlarmCache(RatioAlarmCacheAddRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/AddAlarmCache?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        /// <summary>
        /// 批量添加报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse BacthAddAlarmCache(RatioAlarmCacheBatchAddRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/BacthAddAlarmCache?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        /// <summary>
        /// 更新报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateAlarmCahce(RatioAlarmCacheUpdateRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/UpdateAlarmCahce?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        /// <summary>
        /// 批量更新报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse BatchUpdateAlarmCache(RatioAlarmCacheBatchUpdateRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/BatchUpdateAlarmCache?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        /// <summary>
        /// 获取所有报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_MbInfo>> GetAllAlarmCache(RatioAlarmCacheGetAllRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/GetAllAlarmCache?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_MbInfo>>>(responsestr);
        }

        public BasicResponse<List<JC_MbInfo>> GetAlarmCache(RatioAlarmCacheGetByConditonRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/GetAlarmCache?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_MbInfo>>>(responsestr);
        }

        /// <summary>
        /// 根据Key(Name)获取缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<JC_MbInfo> GetAlarmCacheByKey(RatioAlarmCacheGetByKeyRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/GetAlarmCacheByKey?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_MbInfo>>(responsestr);
        }

        /// <summary>
        /// 批量删除报警信息
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse BatchDeleteAlarmCache(RatioAlarmCacheBatchDeleteRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/batchdeletealarmCache?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        /// <summary>
        /// 更新报警缓存部分属性
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateAlarmInfoProperties(RatioAlarmCacheUpdatePropertiesRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/updatealarmInfoproperties?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        /// <summary>
        /// 根据开始时间获取
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_MbInfo>> GetAlarmCacheByStime(RatioAlarmCacheGetByStimeRequest alarmCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/ratioalarm/GetAlarmCacheByStime?token=" + Token, JSONHelper.ToJSONString(alarmCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_MbInfo>>>(responsestr);
        }
    }
}

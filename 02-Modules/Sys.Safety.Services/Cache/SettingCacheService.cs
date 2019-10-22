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
    /// 时间:2017-05-22
    /// 描述:配置缓存业务
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public class SettingCacheService : ISettingCacheService
    {
        public BasicResponse AddOrUpdateSettingCache(ConfigCacheAddOrUpdateRequest settingCacheRequest)
        {
            var settingInfo = settingCacheRequest.SettingInfo;

            if (settingInfo != null && settingInfo.InfoState == InfoState.AddNew)
            {
                SettingCache.SettingCahceInstance.AddItem(settingInfo);
            }
            else if (settingInfo != null && settingInfo.InfoState == InfoState.Modified)
            {
                SettingCache.SettingCahceInstance.UpdateItem(settingInfo);
            }
            return new BasicResponse(); ;
        }

        public BasicResponse AddSettingCache(SettingCacheAddRequest settingCacheRequest)
        {
            SettingCache.SettingCahceInstance.AddItem(settingCacheRequest.SettingInfo);
            return new BasicResponse();
        }

        public BasicResponse LoadSettingCache(SettingCacheLoadRequest settingCacheRequest)
        {
            SettingCache.SettingCahceInstance.Load();
            return new BasicResponse();
        }

        public BasicResponse<List<SettingInfo>> GetAllSettingCache(SettingCacheGetAllRequest settingCacheRequest)
        {
            var settingCache = SettingCache.SettingCahceInstance.Query();
            var configCacheResponse = new BasicResponse<List<SettingInfo>>();
            configCacheResponse.Data = settingCache;
            return configCacheResponse;
        }

        public BasicResponse<List<SettingInfo>> GetSettingCace(SettingCacheGetByConditonRequest settingCacheRequest)
        {
            var settingCache = SettingCache.SettingCahceInstance.Query(settingCacheRequest.Predicate);
            var configCacheResponse = new BasicResponse<List<SettingInfo>>();
            configCacheResponse.Data = settingCache;
            return configCacheResponse;
        }

        public BasicResponse<SettingInfo> GetSettingCacheByKey(SettingCacheGetByKeyRequest settingCacheRequest)
        {
            var settingCache = SettingCache.SettingCahceInstance.Query(setting=>setting.StrKey==settingCacheRequest.StrKey).FirstOrDefault();
            var configCacheResponse = new BasicResponse<SettingInfo>();
            configCacheResponse.Data = settingCache;
            return configCacheResponse;
        }

        public BasicResponse UpdateSettingCache(SettingCacheUpdateRequest settingCacheRequest)
        {
            SettingCache.SettingCahceInstance.UpdateItem(settingCacheRequest.SettingInfo);
            return new BasicResponse();
        }
    }
}

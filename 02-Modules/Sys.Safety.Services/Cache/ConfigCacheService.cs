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
    public class ConfigCacheService : IConfigCacheService
    {
        public BasicResponse BatchUpdateConfigCache(ConfigCacheBatchUpdateRequest configCacheRequest)
        {
            ConfigCache.ConfigCacheInstance.UpdateItems(configCacheRequest.ConfigInfos);
            return new BasicResponse();
        }

        public BasicResponse<List<ConfigInfo>> GetAllConfigCache(ConfigCacheGetAllRequest configCacheRequest)
        {
            var configCache=  ConfigCache.ConfigCacheInstance.Query();
            var configCacheResponse = new BasicResponse<List<ConfigInfo>>();
            configCacheResponse.Data = configCache;
            return configCacheResponse;
        }

        public BasicResponse<List<ConfigInfo>> GetConfigCache(ConfigCacheGetByConditonRequest configCacheRequest)
        {
            var configCache = ConfigCache.ConfigCacheInstance.Query(configCacheRequest.Predicate);
            var configCacheResponse = new BasicResponse<List<ConfigInfo>>();
            configCacheResponse.Data = configCache;
            return configCacheResponse;
        }

        public BasicResponse<ConfigInfo> GetConfigCacheByKey(ConfigCacheGetByKeyRequest configCacheRequest)
        {
            var configCache = ConfigCache.ConfigCacheInstance.Query(config=>config.Name== configCacheRequest.Name).FirstOrDefault();
            var configCacheResponse = new BasicResponse<ConfigInfo>();
            configCacheResponse.Data = configCache;
            return configCacheResponse;
        }

        public BasicResponse LoadConfigCache(ConfigCacheLoadRequest configCacheRequest)
        {
            ConfigCache.ConfigCacheInstance.Load();
            return new BasicResponse();
        }

        public BasicResponse UpdateConfigCahce(ConfigCacheUpdateRequest configCacheRequest)
        {
            ConfigCache.ConfigCacheInstance.UpdateItem(configCacheRequest.ConfigInfo);
            return new BasicResponse();
        }
    }
}

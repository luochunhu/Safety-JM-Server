using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using System.Collections.Generic;

namespace Sys.Safety.ServiceContract.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-22
    /// 描述:配置缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface IConfigCacheService
    {
        /// <summary>
        /// 加载配置缓存
        /// </summary>
        /// <param name="configCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadConfigCache(ConfigCacheLoadRequest configCacheRequest);

        /// <summary>
        /// 更新配置缓存
        /// </summary>
        /// <param name="configCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateConfigCahce(ConfigCacheUpdateRequest configCacheRequest);

        /// <summary>
        /// 批量更新配置缓存
        /// </summary>
        /// <param name="configCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateConfigCache(ConfigCacheBatchUpdateRequest configCacheRequest);

        /// <summary>
        /// 获取所有配置缓存
        /// </summary>
        /// <param name="configCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<ConfigInfo>> GetAllConfigCache(ConfigCacheGetAllRequest configCacheRequest);

        /// <summary>
        /// 根据Key(Name)获取缓存
        /// </summary>
        /// <param name="configCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<ConfigInfo> GetConfigCacheByKey(ConfigCacheGetByKeyRequest configCacheRequest);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="configCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<ConfigInfo>> GetConfigCache(ConfigCacheGetByConditonRequest configCacheRequest);      
    }
}

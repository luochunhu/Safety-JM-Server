using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载配置缓存RPC
    /// </summary>
    public partial class ConfigCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 修改配置缓存RPC
    /// </summary>
    public partial class ConfigCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 配置缓存
        /// </summary>
        public ConfigInfo ConfigInfo { get; set; }
    }

    /// <summary>
    /// 批量修改配置缓存RPC
    /// </summary>
    public partial class ConfigCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 批量修改配置缓存集合
        /// </summary>
        public List<ConfigInfo> ConfigInfos { get; set; }
    }

    /// <summary>
    /// 查询所有配置缓存RPC
    /// </summary>
    public partial class ConfigCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 按Key查询配置缓存RPC
    /// </summary>
    public partial class ConfigCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// Key:配置名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 按条件查询配置缓存RPC
    /// </summary>
    public partial class ConfigCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<ConfigInfo, bool> Predicate { get; set; }
    }
}

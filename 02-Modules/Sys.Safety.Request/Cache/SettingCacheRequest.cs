using Sys.Safety.DataContract;
using System;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载设置缓存RPC
    /// </summary>
    public partial class SettingCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加设置缓存RPC
    /// </summary>
    public partial class SettingCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        public SettingInfo SettingInfo { get; set; }
    }

    /// <summary>
    /// 修改设置缓存RPC
    /// </summary>
    public partial class SettingCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        public SettingInfo SettingInfo { get; set; }
    }

    /// <summary>
    /// 添加或修改设置缓存RPC
    /// </summary>
    public partial class ConfigCacheAddOrUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        public SettingInfo SettingInfo { get; set; }
    }

    /// <summary>
    /// 查询所有设置缓存RPC
    /// </summary>
    public partial class SettingCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 按Key查询设置缓存RPC
    /// </summary>
    public partial class SettingCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// Key
        /// </summary>
        public string StrKey { get; set; }
    }

    /// <summary>
    /// 按条件查询设置缓存RPC
    /// </summary>
    public partial class SettingCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<SettingInfo, bool> Predicate { get; set; }
    }
}

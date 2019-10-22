using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.PersonCache
{
    /// <summary>
    /// 加载未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 未定义设备缓存
        /// </summary>
        public R_UndefinedDefInfo RUndefinedDefInfo { get; set; }
    }

    /// <summary>
    /// 批量添加未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 未定义设备缓存
        /// </summary>
        public List<R_UndefinedDefInfo> RUndefinedDefInfos { get; set; }
    }

    /// <summary>
    /// 更新未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 未定义设备缓存
        /// </summary>
        public R_UndefinedDefInfo RUndefinedDefInfo { get; set; }
    }

    /// <summary>
    /// 批量更新未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 未定义设备缓存
        /// </summary>
        public List<R_UndefinedDefInfo> RUndefinedDefInfos { get; set; }
    }

    /// <summary>
    /// 删除未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 未定义设备缓存
        /// </summary>
        public R_UndefinedDefInfo RUndefinedDefInfo { get; set; }
    }

    /// <summary>
    /// 批量删除未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 未定义设备缓存
        /// </summary>
        public List<R_UndefinedDefInfo> RUndefinedDefInfos { get; set; }
    }

    /// <summary>
    /// 获取所有未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 未定义设备Key
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 根据条件未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Func<R_UndefinedDefInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 是否存在未定义设备缓存RPC
    /// </summary>
    public partial class RUndefinedDefCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 未定义设备Key
        /// </summary>
        public string Id { get; set; }
    }
}

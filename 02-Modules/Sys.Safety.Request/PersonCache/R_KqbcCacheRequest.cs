using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.PersonCache
{
    /// <summary>
    /// 加载班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 班次缓存
        /// </summary>
        public R_KqbcInfo RKqbcInfo { get; set; }
    }

    /// <summary>
    /// 批量添加班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 班次缓存
        /// </summary>
        public List<R_KqbcInfo> RKqbcInfos { get; set; }
    }

    /// <summary>
    /// 更新班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 班次缓存
        /// </summary>
        public R_KqbcInfo RKqbcInfo { get; set; }
    }

    /// <summary>
    /// 批量更新班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 班次缓存
        /// </summary>
        public List<R_KqbcInfo> RKqbcInfos { get; set; }
    }

    /// <summary>
    /// 删除班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 班次缓存
        /// </summary>
        public R_KqbcInfo RKqbcInfo { get; set; }
    }

    /// <summary>
    /// 批量删除班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 班次缓存
        /// </summary>
        public List<R_KqbcInfo> RKqbcInfos { get; set; }
    }

    /// <summary>
    /// 获取所有班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }
    /// <summary>
    /// 获取所有默认班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheGetDefaultRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 班次Key
        /// </summary>
        public string Bcid { get; set; }
    }

    /// <summary>
    /// 根据条件班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Func<R_KqbcInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 是否存在班次缓存RPC
    /// </summary>
    public partial class RKqbcCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 班次Key
        /// </summary>
        public string Bcid { get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.PersonCache
{
    /// <summary>
    /// 加载人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 人员基本信息缓存
        /// </summary>
        public R_PersoninfInfo RPersoninfInfo { get; set; }
    }

    /// <summary>
    /// 批量添加人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 人员基本信息缓存
        /// </summary>
        public List<R_PersoninfInfo> RPersoninfInfos { get; set; }
    }

    /// <summary>
    /// 更新人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 人员基本信息缓存
        /// </summary>
        public R_PersoninfInfo RPersoninfInfo { get; set; }
    }

    /// <summary>
    /// 批量更新人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 人员基本信息缓存
        /// </summary>
        public List<R_PersoninfInfo> RPersoninfInfos { get; set; }
    }

    /// <summary>
    /// 删除人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 人员基本信息缓存
        /// </summary>
        public R_PersoninfInfo RPersoninfInfo { get; set; }
    }

    /// <summary>
    /// 批量删除人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 人员基本信息缓存
        /// </summary>
        public List<R_PersoninfInfo> RPersoninfInfos { get; set; }
    }

    /// <summary>
    /// 获取所有人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 人员基本信息Key
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 根据条件人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Func<R_PersoninfInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 是否存在人员基本信息缓存RPC
    /// </summary>
    public partial class RPersoninfCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 人员基本信息Key
        /// </summary>
        public string Id { get; set; }
    }
}

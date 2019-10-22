using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 手动交叉控制缓存
        /// </summary>
        public Jc_JcsdkzInfo ManualCrossControlInfo { get; set; }
    }

    /// <summary>
    /// 批量添加手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 手动交叉控制缓存
        /// </summary>
        public List<Jc_JcsdkzInfo> ManualCrossControlInfos { get; set; }
    }

    /// <summary>
    /// 更新手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 手动交叉控制缓存
        /// </summary>
        public Jc_JcsdkzInfo ManualCrossControlInfo { get; set; }
    }

    /// <summary>
    /// 批量更新手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 手动交叉控制缓存
        /// </summary>
        public List<Jc_JcsdkzInfo> ManualCrossControlInfos { get; set; }
    }

    /// <summary>
    /// 删除手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 手动交叉控制缓存
        /// </summary>
        public Jc_JcsdkzInfo ManualCrossControlInfo { get; set; }
    }

    /// <summary>
    /// 批量删除手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 手动交叉控制缓存
        /// </summary>
        public List<Jc_JcsdkzInfo> ManualCrossControlInfos { get; set; }
    }

    /// <summary>
    /// 获取所有手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 手动交叉控制Key
        /// </summary>
        public string ManualCrosControlId { get; set; }
    }

    /// <summary>
    /// 根据条件手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Func<Jc_JcsdkzInfo,bool> Predicate { get; set; }
    }

    /// <summary>
    /// 是否存在手动交叉控制缓存RPC
    /// </summary>
    public partial class ManualCrossControlCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 手动交叉控制Key
        /// </summary>
        public string ManualCrosControlId { get; set; }
    }
}

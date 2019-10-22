using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{

    /// <summary>
    /// 加载广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 广播控制缓存
        /// </summary>
        public B_CallInfo BCallInfo { get; set; }
    }

    /// <summary>
    /// 批量添加广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 广播控制缓存
        /// </summary>
        public List<B_CallInfo> BCallInfos { get; set; }
    }

    /// <summary>
    /// 更新广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 广播控制缓存
        /// </summary>
        public B_CallInfo BCallInfo { get; set; }
    }

    /// <summary>
    /// 批量更新广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 广播控制缓存
        /// </summary>
        public List<B_CallInfo> BCallInfos { get; set; }
    }

    /// <summary>
    /// 删除广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 广播控制缓存
        /// </summary>
        public B_CallInfo BCallInfo { get; set; }
    }

    /// <summary>
    /// 批量删除广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 广播控制缓存
        /// </summary>
        public List<B_CallInfo> BCallInfos { get; set; }
    }

    /// <summary>
    /// 获取所有广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 广播控制Key
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 根据条件广播控制缓存RPC
    /// </summary>
    public partial class BCallCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Func<B_CallInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 是否存在呼叫控制缓存RPC
    /// </summary>
    public partial class BCallCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 广播控制Key
        /// </summary>
        public string Id { get; set; }
    }

    public partial class BatchUpdatePointInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public Dictionary<string, Dictionary<string, object>> updateItems { get; set; }
    }
}

using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.PersonCache
{
    /// <summary>
    /// 加载呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 呼叫控制缓存
        /// </summary>
        public R_CallInfo RCallInfo { get; set; }
    }

    /// <summary>
    /// 批量添加呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 呼叫控制缓存
        /// </summary>
        public List<R_CallInfo> RCallInfos { get; set; }
    }

    /// <summary>
    /// 更新呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 呼叫控制缓存
        /// </summary>
        public R_CallInfo RCallInfo { get; set; }
    }

    /// <summary>
    /// 批量更新呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 呼叫控制缓存
        /// </summary>
        public List<R_CallInfo> RCallInfos { get; set; }
    }

    /// <summary>
    /// 删除呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 呼叫控制缓存
        /// </summary>
        public R_CallInfo RCallInfo { get; set; }
    }

    /// <summary>
    /// 批量删除呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 呼叫控制缓存
        /// </summary>
        public List<R_CallInfo> RCallInfos { get; set; }
    }

    /// <summary>
    /// 获取所有呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 呼叫控制Key
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 根据条件呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Func<R_CallInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 是否存在呼叫控制缓存RPC
    /// </summary>
    public partial class RCallCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 呼叫控制Key
        /// </summary>
        public string Id { get; set; }
    }

    public partial class RCallInfoGetByMasterIDRequest : BasicRequest 
    {
        /// <summary>主控id
        /// 
        /// </summary>
        public string MasterId { get; set; }

        /// <summary>呼叫类型
        /// 
        /// </summary>
        public int CallType { get; set; }

        /// <summary>类型
        /// 
        /// </summary>
        public int Type { get; set; }

        /// <summary>是否根据类型查询
        /// 
        /// </summary>
        public bool IsQueryByType { get; set; }
    }

    public partial class EndRcallByRcallInfoListEequest : BasicRequest
    {
        public List<R_CallInfo> RcallInfo { get; set; }
    }

    public partial class EndRcallDbByRcallInfoListEequest : BasicRequest
    {
        public List<R_CallInfo> RcallInfo { get; set; }
    }

}

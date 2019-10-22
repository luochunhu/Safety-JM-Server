using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 添加运行日志缓存RPC
    /// </summary>
    public partial class RunLogCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 运行日志缓存
        /// </summary>
        public Jc_RInfo RunLogInfo { get; set; }
    }

    /// <summary>
    /// 批量添加运行日志缓存RPC
    /// </summary>
    public partial class RunLogCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 运行日志缓存集合
        /// </summary>
        public List<Jc_RInfo> RunLogInfos { get; set; }
    }

    /// <summary>
    /// 更新运行日志缓存RPC
    /// </summary>
    public partial class RunLogCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 运行日志缓存
        /// </summary>
        public Jc_RInfo RunLogInfo { get; set; }
    }

    /// <summary>
    /// 批量更新运行日志缓存RPC
    /// </summary>
    public partial class RunLogCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 运行日志缓存
        /// </summary>
        public List<Jc_RInfo> RunLogInfos { get; set; }
    }

    /// <summary>
    /// 获取所有运行日志缓存RPC
    /// </summary>
    public partial class RunLogCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取运行日志缓存RPC
    /// </summary>
    public partial class RunLogCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 运行日志Key
        /// </summary>
        public string RunLogId { get; set; }
    }

    /// <summary>
    /// 根据条件获取运行日志缓存RPC
    /// </summary>
    public partial class RunLogCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 运行日志缓存
        /// </summary>
        public Func<Jc_RInfo,bool> Pridicate { get; set; }
    }

    /// <summary>
    /// 停止清除运行日志线程RPC
    /// </summary>
    public partial class RunLogCacheStopCleanRunLogRequest : Basic.Framework.Web.BasicRequest
    {
    }

    public partial class RunLogBatchDeleteRequest : BasicRequest
    {
        /// <summary>
        /// 运行日志缓存
        /// </summary>
        public List<Jc_RInfo> RunLogInfos { get; set; }
    }
}

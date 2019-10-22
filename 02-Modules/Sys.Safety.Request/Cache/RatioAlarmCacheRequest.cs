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
    /// 加载报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 添加报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存
        /// </summary>
        public JC_MbInfo AlarmInfo { get; set; }
    }

    /// <summary>
    /// 批量添加报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<JC_MbInfo> AlarmInfos { get; set; }
    }

    /// <summary>
    /// 修改报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存
        /// </summary>
        public JC_MbInfo AlarmInfo { get; set; }
    }

    /// <summary>
    /// 批量修改报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<JC_MbInfo> AlarmInfos { get; set; }
    }

    /// <summary>
    /// 删除报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheDeleteAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 查询所有报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 根据Key查询报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存Key
        /// </summary>
        public string Id { get; set; }
    }

    public partial class RatioAlarmCacheGetByStimeRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Stime { get; set; }
    }
    /// <summary>
    /// 根据条件查询报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<JC_MbInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 停止清除报警缓存RPC
    /// </summary>
    public partial class RatioAlarmCacheStopCleanRequest : Basic.Framework.Web.BasicRequest
    {

    }

    public partial class RatioAlarmCacheBatchDeleteRequest : BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<JC_MbInfo> AlarmInfos { get; set; }
    }

    public partial class RatioAlarmCacheUpdatePropertiesRequest : BasicRequest 
    {
        /// <summary>
        /// Key
        /// </summary>
        public string AlarmKey { get; set; }
        /// <summary>
        /// 更新信息
        /// </summary>
        public Dictionary<string, object> UpdateItems { get; set; }
    }
}

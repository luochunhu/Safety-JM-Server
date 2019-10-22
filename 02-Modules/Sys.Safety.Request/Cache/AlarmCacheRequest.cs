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
    public partial class AlarmCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 添加报警缓存RPC
    /// </summary>
    public partial class AlarmCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存
        /// </summary>
        public Jc_BInfo AlarmInfo { get; set; }
    }

    /// <summary>
    /// 批量添加报警缓存RPC
    /// </summary>
    public partial class AlarmCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<Jc_BInfo> AlarmInfos { get; set; }
    }

    /// <summary>
    /// 修改报警缓存RPC
    /// </summary>
    public partial class AlarmCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存
        /// </summary>
        public Jc_BInfo AlarmInfo { get; set; }
    }

    /// <summary>
    /// 批量修改报警缓存RPC
    /// </summary>
    public partial class AlarmCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<Jc_BInfo> AlarmInfos { get; set; }
    }

    /// <summary>
    /// 删除报警缓存RPC
    /// </summary>
    public partial class AlarmCacheDeleteAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 查询所有报警缓存RPC
    /// </summary>
    public partial class AlarmCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 根据Key查询报警缓存RPC
    /// </summary>
    public partial class AlarmCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存Key
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 根据条件查询报警缓存RPC
    /// </summary>
    public partial class AlarmCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<Jc_BInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 停止清除报警缓存RPC
    /// </summary>
    public partial class AlarmCacheStopCleanRequest: Basic.Framework.Web.BasicRequest
    {

    }

    public partial class AlarmCacheBatchDeleteRequest : BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<Jc_BInfo> AlarmInfos { get; set; }
    }

    public partial class AlarmCacheUpdatePropertiesRequest : BasicRequest 
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

    public partial class AlarmCacheGetByInfo : BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<Jc_BInfo, bool> Predicate { get; set; }
    }
}

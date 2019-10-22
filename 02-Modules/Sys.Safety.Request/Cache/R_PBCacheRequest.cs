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
    public partial class R_PBCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 添加报警缓存RPC
    /// </summary>
    public partial class R_PBCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存
        /// </summary>
        public R_PbInfo R_PBInfo { get; set; }
    }

    /// <summary>
    /// 批量添加报警缓存RPC
    /// </summary>
    public partial class R_PBCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<R_PbInfo> R_PBInfos { get; set; }
    }

    /// <summary>
    /// 修改报警缓存RPC
    /// </summary>
    public partial class R_PBCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存
        /// </summary>
        public R_PbInfo R_PBInfo { get; set; }
    }

    /// <summary>
    /// 批量修改报警缓存RPC
    /// </summary>
    public partial class R_PBCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<R_PbInfo> R_PBInfos { get; set; }
    }

    /// <summary>
    /// 删除报警缓存RPC
    /// </summary>
    public partial class R_PBCacheDeleteAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 查询所有报警缓存RPC
    /// </summary>
    public partial class R_PBCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 根据Key查询报警缓存RPC
    /// </summary>
    public partial class R_PBCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 报警缓存Key
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 根据条件查询报警缓存RPC
    /// </summary>
    public partial class R_PBCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<R_PbInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 停止清除报警缓存RPC
    /// </summary>
    public partial class R_PBCacheStopCleanRequest: Basic.Framework.Web.BasicRequest
    {

    }

    public partial class R_PBCacheBatchDeleteRequest : BasicRequest
    {
        /// <summary>
        /// 报警集合集合
        /// </summary>
        public List<R_PbInfo> R_PBInfos { get; set; }
    }

    public partial class R_PBCacheUpdatePropertiesRequest : BasicRequest 
    {
        /// <summary>
        /// Key
        /// </summary>
        public string R_PBKey { get; set; }
        /// <summary>
        /// 更新信息
        /// </summary>
        public Dictionary<string, object> UpdateItems { get; set; }
    }

    public partial class R_PBCacheGetByInfo : BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<R_PbInfo, bool> Predicate { get; set; }
    }
}

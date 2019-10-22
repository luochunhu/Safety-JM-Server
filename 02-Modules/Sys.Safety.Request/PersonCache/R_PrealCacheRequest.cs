using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.PersonCache
{
    /// <summary>
    /// 加载实时信息缓存RPC
    /// </summary>
    public partial class RPralCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加实时信息缓存RPC
    /// </summary>
    public partial class RPrealCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 实时信息
        /// </summary>
        public R_PrealInfo PrealInfo { get; set; }
    }

    /// <summary>
    /// 批量添加实时信息缓存RPC
    /// </summary>
    public partial class RPrealCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 实时信息集合
        /// </summary>
        public List<R_PrealInfo> PrealInfos { get; set; }
    }

    /// <summary>
    /// 更新实时信息缓存RPC
    /// </summary>
    public partial class RPrealCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 实时信息
        /// </summary>
        public R_PrealInfo PrealInfo { get; set; }
    }

    /// <summary>
    /// 批量更新实时信息缓存RPC
    /// </summary>
    public partial class RPrealCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 实时信息集合
        /// </summary>
        public List<R_PrealInfo> PrealInfos { get; set; }
    }

    /// <summary>
    /// 删除实时信息缓存RPC
    /// </summary>
    public partial class RPrealCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 实时信息
        /// </summary>
        public R_PrealInfo PrealInfo { get; set; }
    }

    /// <summary>
    /// 批量删除实时信息缓存RPC
    /// </summary>
    public partial class RPrealCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 实时信息
        /// </summary>
        public List<R_PrealInfo> PrealInfos { get; set; }
    }

    /// <summary>
    /// 测点缓存查询请求基类
    /// added by  20170719
    /// </summary>
    public partial class QueryRealRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 是否从写缓存读取数据(默认从只读缓存读取)
        /// </summary>
        public bool IsQueryFromWriteCache { get; set; }
    }
    /// <summary>
    /// 获取所有实时信息缓存RPC
    /// </summary>
    public partial class RPrealCacheGetAllRequest : QueryRealRequest // Basic.Framework.Web.BasicRequest
    {

    }

    public partial class RPrealCacheByPointIdRequeest : QueryRealRequest //Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 根据条件获取实时信息缓存RPC
    /// </summary>
    public partial class RPrealCacheGetByConditonRequest : QueryRealRequest // Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Expression<Func<R_PrealInfo, bool>> Predicate { get; set; }
    }

    /// <summary>
    /// 部分更新信息RPC
    /// </summary>
    public partial class RPrealCacheUpdatePropertiesRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 更新信息
        /// </summary>
        public Dictionary<string, object> UpdateItems { get; set; }
    }

    /// <summary>
    /// 批量 部分更新测点信息请求
    /// </summary>
    public class RPrealCacheBatchUpdatePropertiesRequest : Basic.Framework.Web.BasicRequest
    {
        public Dictionary<string, Dictionary<string, object>> UpdateItems { get; set; }
    }

    public partial class OldPlsPersonRealSyncRequest : Basic.Framework.Web.BasicRequest
    {
        public List<R_PrealInfo> Info { get; set; }
    }
}

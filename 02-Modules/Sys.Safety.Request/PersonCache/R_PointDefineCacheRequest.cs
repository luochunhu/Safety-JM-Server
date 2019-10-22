using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.PersonCache
{

    /// <summary>
    /// 加载测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 批量添加测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义集合
        /// </summary>
        public List<Jc_DefInfo> PointDefineInfos { get; set; }
    }

    /// <summary>
    /// 更新测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 批量更新测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义集合
        /// </summary>
        public List<Jc_DefInfo> PointDefineInfos { get; set; }
    }

    /// <summary>
    /// 删除测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 批量删除测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public List<Jc_DefInfo> PointDefineInfos { get; set; }
    }

    /// <summary>
    /// 测点缓存查询请求基类
    /// added by  20170719
    /// </summary>
    public partial class QueryPointRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 是否从写缓存读取数据(默认从只读缓存读取)
        /// </summary>
        public bool IsQueryFromWriteCache { get; set; }
    }
    /// <summary>
    /// 获取所有测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheGetAllRequest : QueryPointRequest // Basic.Framework.Web.BasicRequest
    {

    }

    public partial class RPointDefineCacheByPointIdRequeest : QueryPointRequest //Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointID { get; set; }
    }

    /// <summary>
    /// 根据条件获取测点定义缓存RPC
    /// </summary>
    public partial class RPointDefineCacheGetByConditonRequest : QueryPointRequest // Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<Jc_DefInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 部分更新测点信息RPC
    /// </summary>
    public partial class RDefineCacheUpdatePropertiesRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// Key
        /// </summary>
        public string PointID { get; set; }
        /// <summary>
        /// 更新信息
        /// </summary>
        public Dictionary<string, object> UpdateItems { get; set; }
    }

    /// <summary>
    /// 批量 部分更新测点信息请求
    /// </summary>
    public class RDefineCacheBatchUpdatePropertiesRequest : Basic.Framework.Web.BasicRequest
    {
        public Dictionary<string, Dictionary<string, object>> PointItems { get; set; }
    }
}

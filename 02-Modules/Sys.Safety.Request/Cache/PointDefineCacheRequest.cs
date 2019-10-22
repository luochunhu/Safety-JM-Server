using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 批量添加测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义集合
        /// </summary>
        public List<Jc_DefInfo> PointDefineInfos { get; set; }
    }

    /// <summary>
    /// 更新测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 批量更新测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义集合
        /// </summary>
        public List<Jc_DefInfo> PointDefineInfos { get; set; }
    }

    /// <summary>
    /// 删除测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 批量删除测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
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
    public partial class PointDefineCacheGetAllRequest : QueryPointRequest // Basic.Framework.Web.BasicRequest
    {
        
    }

    /// <summary>
    ///根据测点编号获取测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheGetByKeyRequest : QueryPointRequest //: Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点编号
        /// </summary>
        public string Point { get; set; }
    }

    public partial class PointDefineCacheByPointIdRequeest : QueryPointRequest //Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointID { get; set; }
    }

    /// <summary>
    /// 根据条件获取测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheGetByConditonRequest : QueryPointRequest // Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<Jc_DefInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 根据分站号获取测点定义缓存RPC
    /// </summary>
    public partial class PointDefineCacheGetByStationRequest : QueryPointRequest // Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点分站号
        /// </summary>
        public short Station { get; set; }
    }

    /// <summary>
    /// 更新测点定义缓存控制信息RPC
    /// </summary>
    public partial class PointDefineCacheUpdateControlReqest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }
    /// <summary>
    /// 更新测点定义缓存分站信息RPC
    /// </summary>
    public partial class PointDefineCacheUpdateStationFlatReqest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool IsInit { get; set; }
    }

    /// <summary>
    /// 更新测点定义缓存通讯次数RPC
    /// </summary>
    public partial class PointDefineCacheUpdateCommTimesReqest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }

        /// <summary>
        /// 更新通讯类类型 0:更新NComTest_TotalCount和NCommCount 1:更新NComTest_TotalCount 2:更新NCommCount
        /// </summary>
        public int UpdateType { get; set; }
    }

    /// <summary>
    /// 更新测点定义缓存唯一编码RPC
    /// </summary>
    public partial class PointDefineCacheUpdateUniqueCodeReqest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 更新测点定义缓存初始化信息RPC
    /// </summary>
    public partial class PointDefineCacheUpdateInitInfoReqest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 更新测点定义缓存错误次数RPC
    /// </summary>
    public partial class PointDefineCacheUpdateErrorCountReqest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 更新测点定义实时信息数RPC
    /// </summary>
    public partial class PointDefineCacheUpdateRealValueReqest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点定义
        /// </summary>
        public Jc_DefInfo PointDefineInfo { get; set; }
    }

    /// <summary>
    /// 部分更新测点信息RPC
    /// </summary>
    public partial class DefineCacheUpdatePropertiesRequest : Basic.Framework.Web.BasicRequest 
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
    public class DefineCacheBatchUpdatePropertiesRequest : Basic.Framework.Web.BasicRequest
    {
        public Dictionary<string, Dictionary<string, object>> PointItems { get; set; }
    }

}

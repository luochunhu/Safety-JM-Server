using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载安装位置缓存RPC
    /// </summary>
    public partial class PositonCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加安装位置缓存
    /// </summary>
    public partial class PositionCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 安装位置
        /// </summary>
        public Jc_WzInfo PositionInfo { get; set; }
    }

    /// <summary>
    /// 批量添加安装位置缓存
    /// </summary>
    public partial class PositionCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 安装位置集合
        /// </summary>
        public List<Jc_WzInfo> PositionInfos { get; set; }
    }

    /// <summary>
    /// 更新安装位置缓存
    /// </summary>
    public partial class PositionCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 安装位置
        /// </summary>
        public Jc_WzInfo PositionInfo { get; set; }
    }
    public partial class PositionCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 安装位置
        /// </summary>
        public Jc_WzInfo PositionInfo { get; set; }
    }

    /// <summary>
    /// 批量更新安装位置缓存
    /// </summary>
    public partial class PositionCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 安装位置集合
        /// </summary>
        public List<Jc_WzInfo> PositionInfos { get; set; }
    }

    /// <summary>
    /// 获取所有安装位置缓存
    /// </summary>
    public partial class PositionCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取安装位置缓存
    /// </summary>
    public partial class PositionCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 位置Id
        /// </summary>
        public string PositionId { get; set; }
    }

    /// <summary>
    /// 根据条件获取安装位置缓存
    /// </summary>
    public partial class PositionCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Func<Jc_WzInfo,bool> Predicate { get; set; }
    }

    /// <summary>
    /// 安装位置缓存是否存在
    /// </summary>
    public partial class PositionCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 位置id
        /// </summary>
        public string PositionId { get; set; }
    }

    /// <summary>
    /// 获取安装位置缓存最大Id
    /// </summary>
    public partial class PositionCacheGetMaxIdRequest : Basic.Framework.Web.BasicRequest
    {
    }
}

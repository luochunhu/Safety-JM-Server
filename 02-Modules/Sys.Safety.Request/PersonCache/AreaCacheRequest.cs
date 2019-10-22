using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.PersonCache
{
    /// <summary>
    /// 加载区域缓存RPC
    /// </summary>
    public partial class AreaCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加区域缓存RPC
    /// </summary>
    public partial class AreaCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 区域缓存
        /// </summary>
        public AreaInfo AreaInfo { get; set; }
    }

    /// <summary>
    /// 批量添加区域缓存RPC
    /// </summary>
    public partial class AreaCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 区域缓存
        /// </summary>
        public List<AreaInfo> AreaInfos { get; set; }
    }

    /// <summary>
    /// 更新区域缓存RPC
    /// </summary>
    public partial class AreaCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 区域缓存
        /// </summary>
        public AreaInfo AreaInfo { get; set; }
    }

    /// <summary>
    /// 批量更新区域缓存RPC
    /// </summary>
    public partial class AreaCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 区域缓存
        /// </summary>
        public List<AreaInfo> AreaInfos { get; set; }
    }

    /// <summary>
    /// 删除区域缓存RPC
    /// </summary>
    public partial class AreaCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 区域缓存
        /// </summary>
        public AreaInfo AreaInfo { get; set; }
    }

    /// <summary>
    /// 批量删除区域缓存RPC
    /// </summary>
    public partial class AreaCacheBatchDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 区域缓存
        /// </summary>
        public List<AreaInfo> AreaInfos { get; set; }
    }

    /// <summary>
    /// 获取所有区域缓存RPC
    /// </summary>
    public partial class AreaCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }
    /// <summary>
    /// 获取所有默认区域缓存RPC
    /// </summary>
    public partial class AreaCacheGetDefaultRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取区域缓存RPC
    /// </summary>
    public partial class AreaCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 区域Key
        /// </summary>
        public string Areaid { get; set; }
    }

    /// <summary>
    /// 根据条件区域缓存RPC
    /// </summary>
    public partial class AreaCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Func<AreaInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 是否存在区域缓存RPC
    /// </summary>
    public partial class AreaCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 区域Key
        /// </summary>
        public string Areaid { get; set; }
    }
}

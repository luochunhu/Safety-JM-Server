using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 添加大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 大数据分析配置缓存
        /// </summary>
        public JC_LargedataAnalysisConfigInfo LargeDataAnalysisConfigInfo { get; set; }
    }

    /// <summary>
    /// 批量添加大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 大数据分析配置集合集合
        /// </summary>
        public List<JC_LargedataAnalysisConfigInfo> LargeDataAnalysisConfigInfos { get; set; }
    }

    /// <summary>
    /// 修改大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 大数据分析配置缓存
        /// </summary>
        public JC_LargedataAnalysisConfigInfo LargeDataAnalysisConfigInfo { get; set; }
    }

    /// <summary>
    /// 批量修改大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 大数据分析配置集合集合
        /// </summary>
        public List<JC_LargedataAnalysisConfigInfo> LargeDataAnalysisConfigInfos { get; set; }
    }

    /// <summary>
    /// 删除大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 大数据分析配置缓存
        /// </summary>
        public JC_LargedataAnalysisConfigInfo LargeDataAnalysisConfigInfo { get; set; }
    }

    /// <summary>
    /// 查询所有大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 根据Key查询大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 大数据分析配置缓存Key
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 根据条件查询大数据分析配置缓存RPC
    /// </summary>
    public partial class LargeDataAnalysisConfigCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<JC_LargedataAnalysisConfigInfo, bool> Predicate { get; set; }
    }
}

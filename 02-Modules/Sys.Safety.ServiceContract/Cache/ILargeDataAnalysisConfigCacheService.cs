using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using System.Collections.Generic;

namespace Sys.Safety.ServiceContract.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-25
    /// 描述:大数据分析配置RPC接口
    /// 修改记录
    /// 2017-05-25
    /// </summary>
    public interface ILargeDataAnalysisConfigCacheService
    {
        /// <summary>
        /// 加载大数据分析配置缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse LoadLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheLoadRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 添加大数据分析配置缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse AddLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheAddRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 批量添加大数据分析配置缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse BacthAddLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheBatchAddRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 更新大数据分析配置缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse UpdateLargeDataAnalysisConfigCahce(LargeDataAnalysisConfigCacheUpdateRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 批量更新大数据分析配置缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheBatchUpdateRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 删除所有大数据分析配置缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse DeleteLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheDeleteRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 获取所有大数据分析配置缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheGetAllRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 根据Key(Id)获取缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse<JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigCacheByKey(LargeDataAnalysisConfigCacheGetByKeyRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 获取大数据分析配置缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache">条件</param>
        /// <returns></returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheGetByConditonRequest largeDataAnalysisConfigCache);
    }
}

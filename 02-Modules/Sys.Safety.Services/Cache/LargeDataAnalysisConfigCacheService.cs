using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-25
    /// 描述:大数据分析配置缓存业务
    /// 修改记录
    /// 2017-05-25
    /// </summary>
    public class LargeDataAnalysisConfigCacheService : ILargeDataAnalysisConfigCacheService
    {
        public BasicResponse AddLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheAddRequest largeDataAnalysisConfigCache)
        {
            LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.AddItem(largeDataAnalysisConfigCache.LargeDataAnalysisConfigInfo);
            return new BasicResponse();
        }

        public BasicResponse BacthAddLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheBatchAddRequest largeDataAnalysisConfigCache)
        {
            LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.AddItems(largeDataAnalysisConfigCache.LargeDataAnalysisConfigInfos);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdateLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheBatchUpdateRequest largeDataAnalysisConfigCache)
        {
            LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.UpdateItems(largeDataAnalysisConfigCache.LargeDataAnalysisConfigInfos);
            return new BasicResponse();
        }

        public BasicResponse DeleteLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheDeleteRequest largeDataAnalysisConfigCache)
        {
            LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.DeleteItem(largeDataAnalysisConfigCache.LargeDataAnalysisConfigInfo);
            return new BasicResponse();
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheGetAllRequest largeDataAnalysisConfigCache)
        {
            var bigDataAnalysisConfigCache = LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.Query();
            var bigDataAnalysisConfigCacheResponse = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            bigDataAnalysisConfigCacheResponse.Data = bigDataAnalysisConfigCache;
            return bigDataAnalysisConfigCacheResponse;
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheGetByConditonRequest largeDataAnalysisConfigCache)
        {
            var bigDataAnalysisConfigCache = LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.Query(largeDataAnalysisConfigCache.Predicate);
            var bigDataAnalysisConfigCacheResponse = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            bigDataAnalysisConfigCacheResponse.Data = bigDataAnalysisConfigCache;
            return bigDataAnalysisConfigCacheResponse;
        }

        public BasicResponse<JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigCacheByKey(LargeDataAnalysisConfigCacheGetByKeyRequest largeDataAnalysisConfigCache)
        {
            var bigDataAnalysisConfigCache = LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.Query(bigdata=>bigdata.Id==largeDataAnalysisConfigCache.Id).FirstOrDefault();
            var bigDataAnalysisConfigCacheResponse = new BasicResponse<JC_LargedataAnalysisConfigInfo>();
            bigDataAnalysisConfigCacheResponse.Data = bigDataAnalysisConfigCache;
            return bigDataAnalysisConfigCacheResponse;
        }

        public BasicResponse LoadLargeDataAnalysisConfigCache(LargeDataAnalysisConfigCacheLoadRequest largeDataAnalysisConfigCache)
        {
            LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.Load();
            return new BasicResponse();
        }

        public BasicResponse UpdateLargeDataAnalysisConfigCahce(LargeDataAnalysisConfigCacheUpdateRequest largeDataAnalysisConfigCache)
        {
            LargeDataAnalysisConfigCache.LargedataAnalysisConfigCahceInstance.UpdateItem(largeDataAnalysisConfigCache.LargeDataAnalysisConfigInfo);
            return new BasicResponse();
        }
    }
}

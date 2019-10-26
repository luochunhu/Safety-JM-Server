using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Cache;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.BigDataAnalysis
{
    public class AnalysisModelCache : KJ73NCacheManager<JC_LargedataAnalysisConfigInfo>
    {
        private readonly ILargeDataAnalysisCacheClientService largeDataAnalysisCacheClientService;
        private static volatile AnalysisModelCache analysisModelCacheInstance;
        public static AnalysisModelCache Instance
        {
            get
            {
                if (analysisModelCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (analysisModelCacheInstance == null)
                        {
                            analysisModelCacheInstance = new AnalysisModelCache();
                        }
                    }
                }
                return analysisModelCacheInstance;
            }
        }

        private AnalysisModelCache()
        {
            largeDataAnalysisCacheClientService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();
        }
        public override void Load()
        {
            var response = largeDataAnalysisCacheClientService.GetAllLargeDataAnalysisConfigCache(new LargeDataAnalysisCacheClientGetAllRequest());
            if (response.IsSuccess && response.Data != null)
            {
                _cache.Clear();
                _cache = response.Data;
            }
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        public int Count {
            get { return _cache.Count; }
        }

        protected override void AddEntityToCache(JC_LargedataAnalysisConfigInfo item)
        {
            if (_cache.Count(q => q.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(JC_LargedataAnalysisConfigInfo item)
        {
            var findItem = _cache.FirstOrDefault(q => q.Id == item.Id);
            if (findItem != null)
                _cache.Remove(findItem);
        }

        protected override void UpdateEntityToCache(JC_LargedataAnalysisConfigInfo item)
        {
            int itemIndex = _cache.FindIndex(q => q.Id == item.Id);
            if (itemIndex >= 0)
            {
                if (_cache[itemIndex].UpdatedTime > item.UpdatedTime)
                    return;
                _cache[itemIndex] = item;
            }
        }

        public void UpdateProperties(string key, Dictionary<string, object> updateItems)
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                var iteminfo = _cache.FirstOrDefault(q => q.Id == key);
                if (iteminfo != null)
                {
                    iteminfo.CopyProperties(updateItems);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新分析模型缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}

using Basic.Framework.Service;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.BigDataAnalysis
{
    public class ExpressionRealTimeResultCache : KJ73NCacheManager<ExpressionRealTimeResultInfo>
    {
        private static volatile ExpressionRealTimeResultCache expressionRealTimeResultCacheInstance;
        public static ExpressionRealTimeResultCache Instance
        {
            get
            {
                if (expressionRealTimeResultCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (expressionRealTimeResultCacheInstance == null)
                        {
                            expressionRealTimeResultCacheInstance = new ExpressionRealTimeResultCache();
                        }
                    }
                }
                return expressionRealTimeResultCacheInstance;
            }
        }
        private ExpressionRealTimeResultCache(){ }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        public int Count
        {
            get { return _cache.Count; }
        }

        protected override void AddEntityToCache(ExpressionRealTimeResultInfo item)
        {
            if (_cache.Count(q => q.AnalysisModelId == item.AnalysisModelId && q.ExpressionId == item.ExpressionId) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(ExpressionRealTimeResultInfo item)
        {
            var findItem = _cache.FirstOrDefault(q => q.AnalysisModelId == item.AnalysisModelId && q.ExpressionId == item.ExpressionId);
            if (findItem != null)
                _cache.Remove(findItem);
        }

        protected override void UpdateEntityToCache(ExpressionRealTimeResultInfo item)
        {
            int itemIndex = _cache.FindIndex(q => q.AnalysisModelId == item.AnalysisModelId && q.ExpressionId == item.ExpressionId);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

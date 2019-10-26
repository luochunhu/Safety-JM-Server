using Basic.Framework.Service;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.BigDataAnalysis
{
    public class HistoryDataCache : KJ73NCacheManager<DataAnalysisHistoryDataInfo>
    {
        private readonly IJc_HourService hourService;
        private static volatile HistoryDataCache historyDataCacheInstance;
        public static HistoryDataCache Instance
        {
            get
            {
                if (historyDataCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (historyDataCacheInstance == null)
                        {
                            historyDataCacheInstance = new HistoryDataCache();
                        }
                    }
                }
                return historyDataCacheInstance;
            }
        }

        private HistoryDataCache()
        {
            hourService = ServiceFactory.Create<IJc_HourService>();
        }

        public override void Load()
        {
            var response = hourService.GetDataAnalysisHistoryData(new Basic.Framework.Web.BasicRequest());
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

        public int Count
        {
            get { return _cache.Count; }
        }

        protected override void AddEntityToCache(DataAnalysisHistoryDataInfo item)
        {
            if (_cache.Count(q => q.PointId == item.PointId) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(DataAnalysisHistoryDataInfo item)
        {
            var findItem = _cache.FirstOrDefault(q => q.PointId == item.PointId);
            if (findItem != null)
                _cache.Remove(findItem);
        }

        protected override void UpdateEntityToCache(DataAnalysisHistoryDataInfo item)
        {
            int itemIndex = _cache.FindIndex(q => q.PointId == item.PointId);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

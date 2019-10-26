using Basic.Framework.Service;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request.RealMessage;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.BigDataAnalysis
{
    public class PointCache : KJ73NCacheManager<Jc_DefInfo>
    {

        private readonly IPointDefineService pointDefineService;
        private static volatile PointCache pointCacheInstance;
        public static PointCache Instance
        {
            get
            {
                if (pointCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (pointCacheInstance == null)
                        {
                            pointCacheInstance = new PointCache();
                        }
                    }
                }
                return pointCacheInstance;
            }
        }
        private PointCache()
        {
            pointDefineService = ServiceFactory.Create<IPointDefineService>();
        }

        public override void Load()
        {
            //GetRealDataRequest realDataRequest = new GetRealDataRequest();
            //realDataRequest.LastRefreshRealDataTime = DateTime.MinValue;
            var response = pointDefineService.GetAllPointDefineCache();
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

        protected override void AddEntityToCache(Jc_DefInfo item)
        {
            if (_cache.Count(q => q.PointID == item.PointID) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_DefInfo item)
        {
            var findItem = _cache.FirstOrDefault(q => q.PointID == item.PointID);
            if (findItem != null)
                _cache.Remove(findItem);
        }

        protected override void UpdateEntityToCache(Jc_DefInfo item)
        {
            int itemIndex = _cache.FindIndex(q => q.PointID == item.PointID);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

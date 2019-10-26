using Basic.Framework.Service;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.Person
{
    public class RKqbcCache : KJ73NCacheManager<R_KqbcInfo>
    {
        private readonly IR_KqbcService _R_KqbcService;

        private static volatile RKqbcCache _RKqbcCacheInstance;
        /// <summary>
        /// 人员班次缓存
        /// </summary>
        public static RKqbcCache RKqbcCacheInstance
        {
            get
            {
                if (_RKqbcCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (_RKqbcCacheInstance == null)
                        {
                            _RKqbcCacheInstance = new RKqbcCache();
                        }
                    }
                }
                return _RKqbcCacheInstance;
            }
        }

        private RKqbcCache()
        {
            _R_KqbcService = ServiceFactory.Create<IR_KqbcService>();
        }

        public override void Load()
        {
            var respose = _R_KqbcService.GetAllKqbcList();
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                _cache = respose.Data;
            }
        }

        protected override void AddEntityToCache(R_KqbcInfo item)
        {
            if (_cache.Count(a => a.Bcid == item.Bcid) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(R_KqbcInfo item)
        {
            var tempitem = _cache.FirstOrDefault(a => a.Bcid == item.Bcid);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(R_KqbcInfo item)
        {
            int itemIndex = _cache.FindIndex(a => a.Bcid == item.Bcid);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

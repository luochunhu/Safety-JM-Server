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
    public class RUndefinedDefCache : KJ73NCacheManager<R_UndefinedDefInfo>
    {
        
        private static volatile RUndefinedDefCache _RUndefinedDefCacheInstance;
        /// <summary>
        /// 人员未定义设备缓存
        /// </summary>
        public static RUndefinedDefCache RUndefinedDefCacheInstance
        {
            get
            {
                if (_RUndefinedDefCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (_RUndefinedDefCacheInstance == null)
                        {
                            _RUndefinedDefCacheInstance = new RUndefinedDefCache();
                        }
                    }
                }
                return _RUndefinedDefCacheInstance;
            }
        }

        private RUndefinedDefCache()
        {
            
        }

        public override void Load()
        {
           
        }

        protected override void AddEntityToCache(R_UndefinedDefInfo item)
        {
            if (_cache.Count(a => a.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(R_UndefinedDefInfo item)
        {
            var tempitem = _cache.FirstOrDefault(a => a.Id == item.Id);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(R_UndefinedDefInfo item)
        {
            int itemIndex = _cache.FindIndex(a => a.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }

        
    }
}

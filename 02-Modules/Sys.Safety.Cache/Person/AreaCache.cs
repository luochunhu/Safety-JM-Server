using Basic.Framework.Service;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Area;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.Person
{
    public class AreaCache : KJ73NCacheManager<AreaInfo>
    {
        private readonly IAreaService _AreaService;

        private static volatile AreaCache _AreaCacheInstance;
        /// <summary>
        /// 人员班次缓存
        /// </summary>
        public static AreaCache AreaCacheInstance
        {
            get
            {
                if (_AreaCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (_AreaCacheInstance == null)
                        {
                            _AreaCacheInstance = new AreaCache();
                        }
                    }
                }
                return _AreaCacheInstance;
            }
        }

        private AreaCache()
        {
            _AreaService = ServiceFactory.Create<IAreaService>();
        }

        public override void Load()
        {
            var respose = _AreaService.GetAllAreaList(new AreaGetListRequest());
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                _cache = respose.Data;
            }
        }

        protected override void AddEntityToCache(AreaInfo item)
        {
            if (_cache.Count(a => a.Areaid == item.Areaid) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(AreaInfo item)
        {
            var tempitem = _cache.FirstOrDefault(a => a.Areaid == item.Areaid);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(AreaInfo item)
        {
            int itemIndex = _cache.FindIndex(a => a.Areaid == item.Areaid);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

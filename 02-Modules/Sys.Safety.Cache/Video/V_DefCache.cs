using Basic.Framework.Service;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Def;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.Video
{
    public class V_DefCache : KJ73NCacheManager<V_DefInfo>
    {
        private readonly IV_DefService vDefService;

        private static volatile V_DefCache _Instance;
        /// <summary>
        /// 呼叫控制单例
        /// </summary>
        public static V_DefCache Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (obj)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new V_DefCache();
                        }
                    }
                }
                return _Instance;
            }
        }

        public V_DefCache()
        {
            vDefService = ServiceFactory.Create<IV_DefService>();
        }

        public override void Load()
        {
            var respose = vDefService.GetAllDef(new DefGetAllRequest());
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                _cache = respose.Data;
            }
        }

        protected override void AddEntityToCache(V_DefInfo item)
        {
            if (_cache.Count(o => o.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(V_DefInfo item)
        {
            var tempitem = _cache.FirstOrDefault(o => o.Id == item.Id);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(V_DefInfo item)
        {
            int itemIndex = _cache.FindIndex(o => o.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

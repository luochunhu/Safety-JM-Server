using Basic.Framework.Service;
using Basic.Framework.Web;
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
    /// <summary>
    /// 人员基本信息缓存
    /// </summary>
    public class RPersoninfCache : KJ73NCacheManager<R_PersoninfInfo>
    {
        private readonly IR_PersoninfService rPersoninfService;

        private static volatile RPersoninfCache rCallCahceInstance;
        /// <summary>
        /// 人员基本信息单例
        /// </summary>
        public static RPersoninfCache RPersoninfInstance
        {
            get
            {
                if (rCallCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (rCallCahceInstance == null)
                        {
                            rCallCahceInstance = new RPersoninfCache();
                        }
                    }
                }
                return rCallCahceInstance;
            }
        }

        public RPersoninfCache()
        {
            rPersoninfService = ServiceFactory.Create<IR_PersoninfService>();
        }

        public override void Load()
        {
            var respose = rPersoninfService.GetAllPersonInfo(new BasicRequest());
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                _cache = respose.Data;
            }
        }

        protected override void AddEntityToCache(R_PersoninfInfo item)
        {
            if (_cache.Count(person => person.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(R_PersoninfInfo item)
        {
            var tempitem = _cache.FirstOrDefault(person => person.Id == item.Id);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(R_PersoninfInfo item)
        {
            int itemIndex = _cache.FindIndex(person => person.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

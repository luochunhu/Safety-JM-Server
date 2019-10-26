using Basic.Framework.Service;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmNotificationPersonnelConfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.BigDataAnalysis
{
    /// <summary>
    /// 多系统融合井下应急联动缓存
    /// </summary>
    public class SysEmergencyLinkageCache : KJ73NCacheManager<SysEmergencyLinkageInfo>
    {
        private ISysEmergencyLinkageService sysEmergencyLinkageService;

        private static volatile SysEmergencyLinkageCache sysEmergencyLinkageInstance;
        public static SysEmergencyLinkageCache Instance
        {
            get
            {
                if (sysEmergencyLinkageInstance == null)
                {
                    lock (obj)
                    {
                        if (sysEmergencyLinkageInstance == null)
                        {
                            sysEmergencyLinkageInstance = new SysEmergencyLinkageCache();
                        }
                    }
                }
                return sysEmergencyLinkageInstance;
            }
        }

        public SysEmergencyLinkageCache()
        {
            sysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();
        }

        public override void Load()
        {
            var response = sysEmergencyLinkageService.GetAllSysEmergencyLinkageInfo();
            if (response.IsSuccess && response.Data != null)
            {
                _cache.Clear();
                _cache = response.Data.Where(o => o.Activity == 1).ToList();
            }
        }

        protected override void AddEntityToCache(SysEmergencyLinkageInfo item)
        {
            if (_cache.Count(o => o.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(SysEmergencyLinkageInfo item)
        {
            var findItem = _cache.FirstOrDefault(o => o.Id == item.Id);
            if (findItem != null)
                _cache.Remove(findItem);
        }

        protected override void UpdateEntityToCache(SysEmergencyLinkageInfo item)
        {
            int itemIndex = _cache.FindIndex(o => o.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

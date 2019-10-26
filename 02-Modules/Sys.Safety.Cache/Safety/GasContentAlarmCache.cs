using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.DataContract.Custom;

namespace Sys.Safety.Cache.Safety
{
    public class GasContentAlarmCache : KJ73NCacheManager<GasContentAlarmInfo>
    {
        private static volatile GasContentAlarmCache _gasContentAlarmCache;

        public static GasContentAlarmCache AlarmCacheInstance
        {
            get
            {
                if (_gasContentAlarmCache == null)
                {
                    lock (obj)
                    {
                        if (_gasContentAlarmCache == null)
                        {
                            _gasContentAlarmCache = new GasContentAlarmCache();
                        }
                    }
                }
                return _gasContentAlarmCache;
            }
        }

        public override void Load()
        {

        }

        protected override void AddEntityToCache(GasContentAlarmInfo item)
        {
            if (_cache.Count(alarm => alarm.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(GasContentAlarmInfo item)
        {
            var alarmitem = _cache.FirstOrDefault(alarm => alarm.Id == item.Id);
            if (alarmitem != null)
                _cache.Remove(alarmitem);
        }

        protected override void UpdateEntityToCache(GasContentAlarmInfo item)
        {
            int itemIndex = _cache.FindIndex(alarm => alarm.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

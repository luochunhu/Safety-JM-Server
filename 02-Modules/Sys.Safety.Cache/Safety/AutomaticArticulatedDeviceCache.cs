using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_R;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:运行日志缓存
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class AutomaticArticulatedDeviceCache : KJ73NCacheManager<AutomaticArticulatedDeviceInfo>
    {

        private static volatile AutomaticArticulatedDeviceCache automaticArticulatedDeviceCahceInstance;
        /// <summary>
        /// 运行日志单例
        /// </summary>
        public static AutomaticArticulatedDeviceCache AutomaticArticulatedDeviceCahceInstance
        {
            get
            {
                if (automaticArticulatedDeviceCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (automaticArticulatedDeviceCahceInstance == null)
                        {
                            automaticArticulatedDeviceCahceInstance = new AutomaticArticulatedDeviceCache();
                        }
                    }
                }
                return automaticArticulatedDeviceCahceInstance;
            }
        }

        private AutomaticArticulatedDeviceCache()
        {

        }

        public override void Load()
        {

        }

        protected override void AddEntityToCache(AutomaticArticulatedDeviceInfo item)
        {
            if (_cache.Count(AutomaticArticulatedDevice => AutomaticArticulatedDevice.ID == item.ID) < 1)
            {
                _cache.Add(item);
            }
        }

        protected override void DeleteEntityFromCache(AutomaticArticulatedDeviceInfo item)
        {
            var tempitem = _cache.FirstOrDefault(AutomaticArticulatedDevice => AutomaticArticulatedDevice.ID == item.ID);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(AutomaticArticulatedDeviceInfo item)
        {
            int itemIndex = _cache.FindIndex(AutomaticArticulatedDevice => AutomaticArticulatedDevice.ID == item.ID);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

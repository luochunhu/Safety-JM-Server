using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Cache;
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
    public class AlarmConfigCache : KJ73NCacheManager<JC_AlarmNotificationPersonnelConfigInfo>
    {
        private readonly IAlarmNotificationPersonnelConfigService alarmNotificationPersonnelConfigService;
        private static volatile AlarmConfigCache alarmConfigCacheInstance;
        public static AlarmConfigCache Instance
        {
            get
            {
                if (alarmConfigCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (alarmConfigCacheInstance == null)
                        {
                            alarmConfigCacheInstance = new AlarmConfigCache();
                        }
                    }
                }
                return alarmConfigCacheInstance;
            }
        }
        private AlarmConfigCache()
        {
            alarmNotificationPersonnelConfigService = ServiceFactory.Create<IAlarmNotificationPersonnelConfigService>();
        }

        public override void Load()
        {
            var response = alarmNotificationPersonnelConfigService.GetAlarmNotificationPersonnelConfigAllList(new GetAllAlarmNotificationRequest());
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

        protected override void AddEntityToCache(JC_AlarmNotificationPersonnelConfigInfo item)
        {
            if (_cache.Count(q => q.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(JC_AlarmNotificationPersonnelConfigInfo item)
        {
            var findItem = _cache.FirstOrDefault(q => q.Id == item.Id);
            if (findItem != null)
                _cache.Remove(findItem);
        }

        protected override void UpdateEntityToCache(JC_AlarmNotificationPersonnelConfigInfo item)
        {
            int itemIndex = _cache.FindIndex(q => q.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }

        public void UpdateProperties(string key, Dictionary<string, object> updateItems)
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                var iteminfo = _cache.FirstOrDefault(q => q.Id == key);
                if (iteminfo != null)
                {
                    iteminfo.CopyProperties(updateItems);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新报警配置缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}

using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Cache;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.BigDataAnalysis
{
    public class EmergencyLinkageConfigCache : KJ73NCacheManager<JC_EmergencyLinkageConfigInfo>
    {
        private readonly IEmergencyLinkageConfigService emergencyLinkageConfigService;
        private static volatile EmergencyLinkageConfigCache emergencyLinkageCacheInstance;
        public static EmergencyLinkageConfigCache Instance
        {
            get
            {
                if (emergencyLinkageCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (emergencyLinkageCacheInstance == null)
                        {
                            emergencyLinkageCacheInstance = new EmergencyLinkageConfigCache();
                        }
                    }
                }
                return emergencyLinkageCacheInstance;
            }
        }
        private EmergencyLinkageConfigCache()
        {
            emergencyLinkageConfigService = ServiceFactory.Create<IEmergencyLinkageConfigService>();
        }
        public override void Load()
        {
            var response = emergencyLinkageConfigService.GetEmergencyLinkageConfigAllList(new GetAllEmergencyLinkageConfigRequest());
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

        protected override void AddEntityToCache(JC_EmergencyLinkageConfigInfo item)
        {
            if (_cache.Count(q => q.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(JC_EmergencyLinkageConfigInfo item)
        {
            var findItem = _cache.FirstOrDefault(q => q.Id == item.Id);
            if (findItem != null)
                _cache.Remove(findItem);
        }

        protected override void UpdateEntityToCache(JC_EmergencyLinkageConfigInfo item)
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
                LogHelper.Error("部分更新应急联动配置缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}

using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Cache;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request.RegionOutageConfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.BigDataAnalysis
{
    public class RegionOutageConfigCache : KJ73NCacheManager<JC_RegionOutageConfigInfo>
    {
        private readonly IRegionOutageConfigService regionOutageConfigService;
        private static volatile RegionOutageConfigCache regionOutageConfigCacheInstance;
        public static RegionOutageConfigCache Instance
        {
            get
            {
                if (regionOutageConfigCacheInstance == null)
                {
                    lock (obj)
                    {
                        if (regionOutageConfigCacheInstance == null)
                        {
                            regionOutageConfigCacheInstance = new RegionOutageConfigCache();
                        }
                    }
                }
                return regionOutageConfigCacheInstance;
            }
        }
        private RegionOutageConfigCache()
        {
            regionOutageConfigService = ServiceFactory.Create<IRegionOutageConfigService>();
        }
        public override void Load()
        {
            var response = regionOutageConfigService.GetRegionOutageConfigAllList(new GetAllRegionOutageConfigRequest());
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

        protected override void AddEntityToCache(JC_RegionOutageConfigInfo item)
        {
            if (_cache.Count(q => q.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(JC_RegionOutageConfigInfo item)
        {
            var findItem = _cache.FirstOrDefault(q => q.Id == item.Id);
            if (findItem != null)
                _cache.Remove(findItem);
        }

        protected override void UpdateEntityToCache(JC_RegionOutageConfigInfo item)
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

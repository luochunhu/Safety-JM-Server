using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Gascontentanalyzeconfig;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Cache.Safety
{
    public class GasContentAnalyzeConfigCache : KJ73NCacheManager<GascontentanalyzeconfigInfo>
    {
        private static readonly IGascontentanalyzeconfigService GascontentanalyzeconfigService = ServiceFactory.Create<IGascontentanalyzeconfigService>();

        private static volatile GasContentAnalyzeConfigCache _gasContentAnalyzeConfigCache;

        public static GasContentAnalyzeConfigCache CacheInstance
        {
            get
            {
                if (_gasContentAnalyzeConfigCache == null)
                {
                    lock (obj)
                    {
                        if (_gasContentAnalyzeConfigCache == null)
                        {
                            _gasContentAnalyzeConfigCache = new GasContentAnalyzeConfigCache();
                        }
                    }
                }
                return _gasContentAnalyzeConfigCache;
            }
        }

        public override void Load()
        {
            _cache = GascontentanalyzeconfigService.GetAllGascontentanalyzeconfigList().Data;
        }

        protected override void AddEntityToCache(GascontentanalyzeconfigInfo item)
        {
            if (_cache.Count(alarm => alarm.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(GascontentanalyzeconfigInfo item)
        {
            var alarmitem = _cache.FirstOrDefault(alarm => alarm.Id == item.Id);
            if (alarmitem != null)
                _cache.Remove(alarmitem);
        }

        protected override void UpdateEntityToCache(GascontentanalyzeconfigInfo item)
        {
            int itemIndex = _cache.FindIndex(alarm => alarm.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }

        public void UpdateRealTimeValue(UpdateRealTimeValueRequest updateData)
        {
            if (updateData == null)
                return;

            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.Id == updateData.Id);
                if (itemIndex >= 0)
                {
                    _cache[itemIndex].RealTimeValue = updateData.RealTimeValue;
                    _cache[itemIndex].State = updateData.State;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义实时信息缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}

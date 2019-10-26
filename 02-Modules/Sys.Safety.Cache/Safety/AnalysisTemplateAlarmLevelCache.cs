using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.Safety
{
    public class AnalysisTemplateAlarmLevelCache : KJ73NCacheManager<Jc_AnalysistemplatealarmlevelInfo>
    {
        private IJc_AnalysistemplatealarmlevelService _AnalysistemplatealarmlevelService;

        private static volatile AnalysisTemplateAlarmLevelCache _instance;

        public static AnalysisTemplateAlarmLevelCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                            _instance = new AnalysisTemplateAlarmLevelCache();
                    }
                }
                return _instance;
            }
        }

        public AnalysisTemplateAlarmLevelCache()
        {
            _AnalysistemplatealarmlevelService = ServiceFactory.Create<IJc_AnalysistemplatealarmlevelService>();
        }

        public override void Load()
        {
            var respose = _AnalysistemplatealarmlevelService.GetAllAnalysistemplateAlarmLevelInfos();
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                _cache = respose.Data;
            }
        }

        protected override void AddEntityToCache(Jc_AnalysistemplatealarmlevelInfo item)
        {
            if (_cache.Count(i => i.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_AnalysistemplatealarmlevelInfo item)
        {
            var alarmitem = _cache.FirstOrDefault(i => i.Id == item.Id);
            if (alarmitem != null)
                _cache.Remove(alarmitem);
        }

        protected override void UpdateEntityToCache(Jc_AnalysistemplatealarmlevelInfo item)
        {
            int itemIndex = _cache.FindIndex(i => i.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

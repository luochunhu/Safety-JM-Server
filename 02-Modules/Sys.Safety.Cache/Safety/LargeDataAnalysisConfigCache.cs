using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Largedataanalysisconfig;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-25
    /// 描述:大数据分析配置缓存
    /// 修改记录
    /// 2017-05-25
    /// </summary>
    public class LargeDataAnalysisConfigCache : KJ73NCacheManager<JC_LargedataAnalysisConfigInfo>
    {
        private readonly ILargedataAnalysisConfigService largedataAnalysisConfigService;

        private LargeDataAnalysisConfigCache()
        {
            largedataAnalysisConfigService = ServiceFactory.Create<ILargedataAnalysisConfigService>();
        }

        private static volatile LargeDataAnalysisConfigCache largedataAnalysisConfigCahceInstance;
        /// <summary>
        /// 大数据分析配置单例
        /// </summary>
        public static LargeDataAnalysisConfigCache LargedataAnalysisConfigCahceInstance
        {
            get
            {
                if (largedataAnalysisConfigCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (largedataAnalysisConfigCahceInstance == null)
                        {
                            largedataAnalysisConfigCahceInstance = new LargeDataAnalysisConfigCache();
                        }
                    }
                }
                return largedataAnalysisConfigCahceInstance;
            }
        }

        public override void Load()
        {
            //try
            //{
                LargedataAnalysisConfigGetListRequest request = new LargedataAnalysisConfigGetListRequest();
                var respose = largedataAnalysisConfigService.GetAllEnabledLargeDataAnalysisConfigWithDetail(request);
                if (respose.Data != null && respose.IsSuccess)
                {
                    _cache.Clear();
                    _cache = respose.Data;
                }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
            //}
        }

        protected override void AddEntityToCache(JC_LargedataAnalysisConfigInfo item)
        {
            if (_cache.Count(bigdata => bigdata.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(JC_LargedataAnalysisConfigInfo item)
        {
            var tempitem = _cache.FirstOrDefault(bigdata => bigdata.Id == item.Id);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(JC_LargedataAnalysisConfigInfo item)
        {
            int itemIndex = _cache.FindIndex(bigdata => bigdata.Id == item.Id);
            if (itemIndex >= 0)
            {
                if (_cache[itemIndex].UpdatedTime > item.UpdatedTime)
                    return;
                _cache[itemIndex] = item;
            }
        }
    }
}

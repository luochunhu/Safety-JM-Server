using Basic.Framework.JobSchedule;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Request;
using Sys.Safety.Cache.BigDataAnalysis;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Processing.DataAnalysis.Notification
{
    public class HistoryDataCacheNotification : BasicTask
    {
        protected static readonly object obj = new object();
        private static volatile HistoryDataCacheNotification _instance;
        public static HistoryDataCacheNotification Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new HistoryDataCacheNotification(10000);
                        }
                    }
                }
                return _instance;
            }
        }
        private HistoryDataCache historyDataCache;
        private DateTime pointDefineLastChangedTime = DateTime.MinValue;
        private ILargeDataAnalysisLastChangedService largeDataAnalysisLastChangedService;
        private IJc_HourService hourService;
        private HistoryDataCacheNotification(int intreval)
            : base("获取模拟量历史数据", intreval)
        {
            historyDataCache = HistoryDataCache.Instance;
            hourService = ServiceFactory.Create<IJc_HourService>();
            largeDataAnalysisLastChangedService = ServiceFactory.Create<ILargeDataAnalysisLastChangedService>();
        }
        protected override void DoWork()
        {
            DateTime analysisModelServerSideTime = DateTime.MinValue;
            try
            {
                DateTime pointDefineServerSideTime = DateTime.MinValue;
                BasicResponse<string> pointDefineLastChangedTimeResponse = largeDataAnalysisLastChangedService.GetPointDefineLastChangedTime(new LargeDataAnalysisLastChangedRequest());
                if (pointDefineLastChangedTimeResponse.IsSuccess && !string.IsNullOrEmpty(pointDefineLastChangedTimeResponse.Data))
                {
                    pointDefineServerSideTime = DateTime.Parse(pointDefineLastChangedTimeResponse.Data);
                }

                var getAllHistoryDataResponse = hourService.GetDataAnalysisHistoryData(new BasicRequest());
                if (getAllHistoryDataResponse.IsSuccess && getAllHistoryDataResponse.Data != null)
                {
                    if (pointDefineServerSideTime > pointDefineLastChangedTime || historyDataCache.Count == 0)
                    {
                        historyDataCache.Clear();
                        foreach (var item in getAllHistoryDataResponse.Data)
                        {
                            historyDataCache.AddItem(item);
                        }
                        pointDefineLastChangedTime = pointDefineServerSideTime;
                    }
                    else
                    {
                        foreach (var item in getAllHistoryDataResponse.Data)
                        {
                            var oldItem = historyDataCache.Query(q => q.PointId == item.PointId).FirstOrDefault();
                            if (null == oldItem)
                            {
                                historyDataCache.AddItem(item);
                            }
                            else
                            {
                                historyDataCache.UpdateItem(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("更新内部维护的模拟量历史数据时出错:{0}", ex.StackTrace));
            }
            base.DoWork();
        }

        private void Reset()
        {
            pointDefineLastChangedTime = DateTime.MinValue;
            historyDataCache.Clear();
        }

        public override void Stop()
        {
            base.Stop();
            Reset();
        }

        public override void Start()
        {
            Reset();
            base.Start();
        }
    }
}

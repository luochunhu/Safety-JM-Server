using Basic.Framework.JobSchedule;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using Sys.Safety.Cache.BigDataAnalysis;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.DataAnalysis.Notification
{
    public class AnalysisModelChangeNotification : BasicTask
    {
        protected static readonly object obj = new object();
        private static volatile AnalysisModelChangeNotification _instance;
        public static AnalysisModelChangeNotification Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AnalysisModelChangeNotification(1000);
                        }
                    }
                }
                return _instance;
            }
        }
        private ILargeDataAnalysisLastChangedService largeDataAnalysisLastChangedService;
        private ILargeDataAnalysisCacheClientService largeDataAnalysisCacheClientService;
        private DateTime analysisModelLastChangedTime = DateTime.MinValue;
        private AnalysisModelCache analysisModelCache;
        public event DataAnalysisNotificationDelegate AnalysisModelChangedEvent;
        private AnalysisModelChangeNotification(int intreval)
            : base("检查服务端分析模型是否修改", intreval)
        {
            largeDataAnalysisLastChangedService = ServiceFactory.Create<ILargeDataAnalysisLastChangedService>();
            largeDataAnalysisCacheClientService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();
            analysisModelCache = AnalysisModelCache.Instance;
            DataAnalysisService.Instance.RegisterAnalysisModelChangedEvent(this);
        }

        protected override void DoWork()
        {
            DateTime analysisModelServerSideTime = DateTime.MinValue;
            try
            {
                BasicResponse<string> analysisModelLastChangedTimeResponse = largeDataAnalysisLastChangedService.GetAnalysisModelLastChangedTime(new LargeDataAnalysisLastChangedRequest());
                if (analysisModelLastChangedTimeResponse.IsSuccess && !string.IsNullOrEmpty(analysisModelLastChangedTimeResponse.Data))
                {
                    analysisModelServerSideTime = DateTime.Parse(analysisModelLastChangedTimeResponse.Data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("检查服务端分析模型最后修改时间时出错:{0}", ex.StackTrace));
            }

            if (analysisModelServerSideTime > analysisModelLastChangedTime || analysisModelCache.Count == 0)
            {
                BasicResponse<List<JC_LargedataAnalysisConfigInfo>> analysisConfigInfoListResponse;
                try
                {
                    analysisConfigInfoListResponse = largeDataAnalysisCacheClientService.GetAllLargeDataAnalysisConfigCache(new LargeDataAnalysisCacheClientGetAllRequest());
                    if (analysisConfigInfoListResponse.IsSuccess && analysisConfigInfoListResponse.Data != null)
                    {
                        var newestAnalysisConfigInfoList = analysisConfigInfoListResponse.Data;
                        foreach (var item in newestAnalysisConfigInfoList)
                        {
                            JC_LargedataAnalysisConfigInfo existsAnalysisModel = analysisModelCache.Query(q => q.Id == item.Id, false).FirstOrDefault();
                            if (null == existsAnalysisModel)
                            {
                                //有新的分析模型加入
                                if (AnalysisModelChangedEvent != null)
                                    AnalysisModelChangedEvent(new AnalysisChangedEventArgs() { Action = "Add", AnalysisConfig = item });
                            }
                            else
                            {
                                if (item.UpdatedTime > existsAnalysisModel.UpdatedTime)
                                {
                                    //分析模型有更新。
                                    if (AnalysisModelChangedEvent != null)
                                        AnalysisModelChangedEvent(new AnalysisChangedEventArgs() { Action = "Update", AnalysisConfig = item });
                                }
                            }
                        }
                        var cachedItems = analysisModelCache.Query();
                        for (int i = cachedItems.Count - 1; i >= 0; i--)
                        {
                            if (!newestAnalysisConfigInfoList.Any(q => q.Id == cachedItems[i].Id))
                            {
                                //内部维护的分析模型在服务端已被删除
                                if (AnalysisModelChangedEvent != null)
                                    AnalysisModelChangedEvent(new AnalysisChangedEventArgs() { Action = "Delete", AnalysisConfig = cachedItems[i] });
                            }
                        }
                    }
                    analysisModelLastChangedTime = analysisModelServerSideTime;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("更新内部维护的分析模型时出错:{0}", ex.StackTrace));
                }
            }
            base.DoWork();
        }

        private void Reset()
        {
            analysisModelLastChangedTime = DateTime.MinValue;
            analysisModelCache.Clear();
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

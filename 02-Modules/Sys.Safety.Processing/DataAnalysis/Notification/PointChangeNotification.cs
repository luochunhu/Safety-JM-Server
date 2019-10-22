using Basic.Framework.JobSchedule;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Request;
using Sys.Safety.Cache.BigDataAnalysis;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.RealMessage;
using System;
using System.Linq;
using Basic.Framework.Logging;

namespace Sys.Safety.Processing.DataAnalysis.Notification
{
    public class PointChangeNotification : BasicTask
    {
        protected static readonly object obj = new object();
        private static volatile PointChangeNotification _instance;
        public static PointChangeNotification Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new PointChangeNotification(3000);
                        }
                    }
                }
                return _instance;
            }
        }
        private IPointDefineService pointDefineService;
        private ILargeDataAnalysisLastChangedService largeDataAnalysisLastChangedService;
        private DateTime pointDefineLastChangedTime = DateTime.MinValue;
        private DateTime lastRefreshRealDataTime = DateTime.MinValue;
        private PointCache pointCache;
        private PointChangeNotification(int intreval)
            : base("检查服务端测点是否修改", intreval)
        {
            pointDefineService = ServiceFactory.Create<IPointDefineService>();
            largeDataAnalysisLastChangedService = ServiceFactory.Create<ILargeDataAnalysisLastChangedService>();
            pointCache = PointCache.Instance;
        }

        protected override void DoWork()
        {
            try
            {
                DateTime pointDefineServerSideTime = DateTime.MinValue;
                BasicResponse<string> pointDefineLastChangedTimeResponse = largeDataAnalysisLastChangedService.GetPointDefineLastChangedTime(new LargeDataAnalysisLastChangedRequest());
                if (pointDefineLastChangedTimeResponse.IsSuccess && !string.IsNullOrEmpty(pointDefineLastChangedTimeResponse.Data))
                {
                    pointDefineServerSideTime = DateTime.Parse(pointDefineLastChangedTimeResponse.Data);
                }
                if (pointDefineServerSideTime > pointDefineLastChangedTime || pointCache.Count == 0)
                {
                    //测点定义发生变化后重新初始化测点定义缓存.
                    //GetRealDataRequest realDataRequest = new GetRealDataRequest();
                    //realDataRequest.LastRefreshRealDataTime = DateTime.MinValue;
                    var getAllPointResponse = pointDefineService.GetAllPointDefineCache();
                    if (getAllPointResponse.IsSuccess && getAllPointResponse.Data != null)
                    {
                        pointCache.Clear();
                        foreach (var item in getAllPointResponse.Data)
                        {
                            if (item.DttStateTime > lastRefreshRealDataTime)
                            {
                                lastRefreshRealDataTime = item.DttStateTime;
                            }
                            pointCache.AddItem(item);
                        }
                    }
                    pointDefineLastChangedTime = pointDefineServerSideTime;
                }
                else
                {
                    //测点定义没有发生变化没有重新初始化测点定义缓存时，更新本地测点缓存
                    //GetRealDataRequest realDataRequest = new GetRealDataRequest();
                    //realDataRequest.LastRefreshRealDataTime = lastRefreshRealDataTime;
                    //var getAllPointResponse = realMessageService.GetRealData(realDataRequest);
                    var getAllPointResponse = pointDefineService.GetAllPointDefineCache();
                    if (getAllPointResponse.IsSuccess && getAllPointResponse.Data != null)
                    {
                        foreach (var item in getAllPointResponse.Data)
                        {
                            if (item.DttStateTime > lastRefreshRealDataTime)
                            {
                                lastRefreshRealDataTime = item.DttStateTime;
                            }
                            var oldPointDefine = pointCache.Query(q => q.PointID == item.PointID).FirstOrDefault();
                            if (null == oldPointDefine)
                            {
                                pointCache.AddItem(item);
                            }
                            else
                            {
                                pointCache.UpdateItem(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pointCache.Clear();//获取监控服务端获取缓存出错时，清除内部缓存。
                LogHelper.Error(string.Format("更新内部维护的缓存列表时出错:{0}", ex.StackTrace));
            }
            base.DoWork();
        }

        private void Reset()
        {
            pointDefineLastChangedTime = DateTime.MinValue;
            lastRefreshRealDataTime = DateTime.MinValue;
            pointCache.Clear();
        }

        public override void Start()
        {
            Reset();
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
            Reset();
        }
    }
}

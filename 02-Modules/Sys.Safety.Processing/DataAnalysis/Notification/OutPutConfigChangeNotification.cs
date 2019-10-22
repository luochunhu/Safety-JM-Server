using Basic.Framework.JobSchedule;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Request;
using Sys.Safety.Request.AlarmNotificationPersonnelConfig;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Sys.Safety.Request.RegionOutageConfig;
using Sys.Safety.Cache.BigDataAnalysis;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Processing.DataAnalysis.Notification
{
    public class OutPutConfigChangeNotification : BasicTask
    {
        protected static readonly object obj = new object();
        private static volatile OutPutConfigChangeNotification _instance;
        public static OutPutConfigChangeNotification Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new OutPutConfigChangeNotification(1000);
                        }
                    }
                }
                return _instance;
            }
        }

        private ILargeDataAnalysisLastChangedService largeDataAnalysisLastChangedService;
        private IAlarmNotificationPersonnelConfigService alarmNotificationPersonnelConfigService;
        private IEmergencyLinkageConfigService emergencyLinkageConfigService;
        private IRegionOutageConfigService regionOutageConfigService;
        private DateTime alarmNotificationLastChangedTime = DateTime.MinValue;
        private DateTime emergencyLinkageLastChangedTime = DateTime.MinValue;
        private DateTime regionOutageLastChangedTime = DateTime.MinValue;
        private RegionOutageConfigCache regionOutageConfigCache;
        private EmergencyLinkageConfigCache emergencyLinkageConfigCache;
        private AlarmConfigCache alarmConfigCache;
        private OutPutConfigChangeNotification(int intreval)
            : base("检查服务端输出配置是否修改", intreval)
        {
            largeDataAnalysisLastChangedService = ServiceFactory.Create<ILargeDataAnalysisLastChangedService>();
            emergencyLinkageConfigService = ServiceFactory.Create<IEmergencyLinkageConfigService>();
            regionOutageConfigService = ServiceFactory.Create<IRegionOutageConfigService>();
            alarmNotificationPersonnelConfigService = ServiceFactory.Create<IAlarmNotificationPersonnelConfigService>();
            regionOutageConfigCache = RegionOutageConfigCache.Instance;
            alarmConfigCache = AlarmConfigCache.Instance;
            emergencyLinkageConfigCache = EmergencyLinkageConfigCache.Instance;
        }

        protected override void DoWork()
        {
            /*检查报警通知-start*/
            try
            {
                DateTime alarmNotificationServerSideTime = DateTime.MinValue;
                BasicResponse<string> alarmNotificationLastChangedTimeResponse = largeDataAnalysisLastChangedService.GetAlarmNotificationLastChangedTime(new LargeDataAnalysisLastChangedRequest());
                if (alarmNotificationLastChangedTimeResponse.IsSuccess && !string.IsNullOrEmpty(alarmNotificationLastChangedTimeResponse.Data))
                {
                    alarmNotificationServerSideTime = DateTime.Parse(alarmNotificationLastChangedTimeResponse.Data);
                }
                if (alarmNotificationServerSideTime > alarmNotificationLastChangedTime || alarmConfigCache.Count == 0)
                {
                    var alarmNotificationPersonnelResponse = alarmNotificationPersonnelConfigService.GetAlarmNotificationPersonnelConfigAllList(new GetAllAlarmNotificationRequest());
                    if (alarmNotificationPersonnelResponse.IsSuccess && alarmNotificationPersonnelResponse.Data != null)
                    {
                        var cachedAlarmNotificationItems = alarmConfigCache.Query();
                        foreach (var item in alarmNotificationPersonnelResponse.Data)
                        {
                            var oldAlarmConfig = cachedAlarmNotificationItems.FirstOrDefault(q => q.Id == item.Id);
                            if (null == oldAlarmConfig)
                                alarmConfigCache.AddItem(item);
                            else
                                alarmConfigCache.UpdateItem(item);
                        }
                        int alarmConfigCount = cachedAlarmNotificationItems.Count;
                        for (int i = alarmConfigCount - 1; i >= 0; i--)
                        {
                            if (!alarmNotificationPersonnelResponse.Data.Any(q => q.Id == cachedAlarmNotificationItems[i].Id))
                            {
                                alarmConfigCache.DeleteItem(cachedAlarmNotificationItems[i]);
                            }
                        }
                    }
                    alarmNotificationLastChangedTime = alarmNotificationServerSideTime;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("检查报警通知时出错:{0}", ex.StackTrace));
            }

            /*检查报警通知-end*/
            /*检查应急联动-start*/
            try
            {
                DateTime emergencyLinkageServerSideTime = DateTime.MinValue;
                BasicResponse<string> emergencyLinkageLastChangedTimeResponse = largeDataAnalysisLastChangedService.GetEmergencyLinkageLastChangedTime(new LargeDataAnalysisLastChangedRequest());
                if (emergencyLinkageLastChangedTimeResponse.IsSuccess && !string.IsNullOrEmpty(emergencyLinkageLastChangedTimeResponse.Data))
                {
                    emergencyLinkageServerSideTime = DateTime.Parse(emergencyLinkageLastChangedTimeResponse.Data);
                }
                if (emergencyLinkageServerSideTime > emergencyLinkageLastChangedTime || emergencyLinkageConfigCache.Count == 0)
                {
                    var emergencyLinkageResponse = emergencyLinkageConfigService.GetEmergencyLinkageConfigAllList(new GetAllEmergencyLinkageConfigRequest());
                    if (emergencyLinkageResponse.IsSuccess && emergencyLinkageResponse.Data != null)
                    {
                        var cachedEmergencyLinkageItems = emergencyLinkageConfigCache.Query();
                        foreach (var item in emergencyLinkageResponse.Data)
                        {
                            var oldEmergencyLinkageConfig = cachedEmergencyLinkageItems.FirstOrDefault(q => q.Id == item.Id);
                            if (null == oldEmergencyLinkageConfig)
                                emergencyLinkageConfigCache.AddItem(item);
                            else
                                emergencyLinkageConfigCache.UpdateItem(item);
                        }
                        int emergencyLinkageCount = cachedEmergencyLinkageItems.Count;
                        for (int i = emergencyLinkageCount - 1; i >= 0; i--)
                        {
                            if (!emergencyLinkageResponse.Data.Any(q => q.Id == cachedEmergencyLinkageItems[i].Id))
                            {
                                emergencyLinkageConfigCache.DeleteItem(cachedEmergencyLinkageItems[i]);
                            }
                        }
                    }
                    emergencyLinkageLastChangedTime = emergencyLinkageServerSideTime;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("检查应急联动时出错:{0}", ex.StackTrace));
            }
            /*检查应急联动-end*/
            /*检查区域断电-start RegionOutageConfigCacheList*/
            try
            {
                DateTime regionOutageServerSideTime = DateTime.MinValue;
                BasicResponse<string> regionOutageLastChangedTimeResponse = largeDataAnalysisLastChangedService.GetRegionOutageLastChangedTime(new LargeDataAnalysisLastChangedRequest());
                if (regionOutageLastChangedTimeResponse.IsSuccess && !string.IsNullOrEmpty(regionOutageLastChangedTimeResponse.Data))
                {
                    regionOutageServerSideTime = DateTime.Parse(regionOutageLastChangedTimeResponse.Data);
                }
                if (regionOutageServerSideTime > regionOutageLastChangedTime || regionOutageConfigCache.Count == 0)
                {
                    var regionOutageResponse = regionOutageConfigService.GetRegionOutageConfigAllList(new GetAllRegionOutageConfigRequest());
                    if (regionOutageResponse.IsSuccess && regionOutageResponse.Data != null)
                    {
                        var cachedRegionOutageItems = regionOutageConfigCache.Query();
                        foreach (var item in regionOutageResponse.Data)
                        {
                            var oldRegionOutageConfig = cachedRegionOutageItems.FirstOrDefault(q => q.Id == item.Id);
                            if (null == oldRegionOutageConfig)
                                regionOutageConfigCache.AddItem(item);
                            else
                                regionOutageConfigCache.UpdateItem(item);
                        }
                        int regionOutageCount = cachedRegionOutageItems.Count;
                        for (int i = regionOutageCount - 1; i >= 0; i--)
                        {
                            if (!regionOutageResponse.Data.Any(q => q.Id == cachedRegionOutageItems[i].Id))
                            {
                                regionOutageConfigCache.DeleteItem(cachedRegionOutageItems[i]);
                            }
                        }
                    }
                    regionOutageLastChangedTime = regionOutageServerSideTime;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("检查区域断电时出错:{0}", ex.StackTrace));
            }
            /*检查区域断电-end*/
            base.DoWork();
        }

        private void Reset()
        {
            alarmNotificationLastChangedTime = DateTime.MinValue;
            emergencyLinkageLastChangedTime = DateTime.MinValue;
            regionOutageLastChangedTime = DateTime.MinValue;
            regionOutageConfigCache.Clear();
            emergencyLinkageConfigCache.Clear();
            alarmConfigCache.Clear();
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

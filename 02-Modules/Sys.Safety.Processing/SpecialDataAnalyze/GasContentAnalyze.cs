using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Request.Gascontentanalyzeconfig;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract.Custom;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Processing.SpecialDataAnalyze
{
    public static class GasContentAnalyze
    {
        private static readonly IGasContentAlarmCacheService GasContentAlarmCacheService = ServiceFactory.Create<IGasContentAlarmCacheService>();
        private static readonly IGascontentanalyzeconfigService GascontentanalyzeconfigService = ServiceFactory.Create<IGascontentanalyzeconfigService>();
        private static readonly IJc_HourService JcHourService = ServiceFactory.Create<IJc_HourService>();
        private static readonly IPointDefineCacheService PointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
        private static readonly IGasContentAnalyzeConfigCacheService GasContentAnalyzeConfigCacheService = ServiceFactory.Create<IGasContentAnalyzeConfigCacheService>();

        /// <summary>运行标记
        /// 
        /// </summary>
        private static bool _isRun;

        /// <summary>处理线程
        /// 
        /// </summary>
        private static Thread _handleThread;

        /// <summary>最后一次运行时间
        /// 
        /// </summary>
        private static DateTime _lastRunTime;

        public static void Start()
        {
            LogHelper.Info("【GasContentAnalyze】瓦斯含量分析线程开启。");

            _isRun = true;
            if (_handleThread == null || (_handleThread != null && !_handleThread.IsAlive))
            {
                _handleThread = new Thread(HandleThreadFun);
                _handleThread.Start();
            }
        }

        /// <summary>结束分析
        /// 
        /// </summary>
        public static void Stop()
        {
            LogHelper.Info("【GasContentAnalyze】瓦斯含量分析线程结束。");
            _isRun = false;
            while (true)
            {
                if (_isRun) break;
                Thread.Sleep(1000);
            }
        }

        /// <summary>线程函数
        /// 
        /// </summary>
        private static void HandleThreadFun()
        {
            while (_isRun)
            {
                try
                {
                    var dtNow = DateTime.Now;
                    if ((dtNow - _lastRunTime).TotalSeconds >= 5)
                    {
                        Analyze();
                        _lastRunTime = DateTime.Now;
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }

                Thread.Sleep(1000);
            }
            _isRun = true;
            LogHelper.Info("【GasContentAnalyze】瓦斯含量分析线程结束成功。");
        }

        /// <summary>分析函数
        /// 
        /// </summary>
        private static void Analyze()
        {
            var req = new PointDefineCacheGetAllRequest();
            var allPoint = PointDefineCacheService.GetAllPointDefineCache(req).Data;

            var req2 = new BasicRequest();
            var res2 = JcHourService.GetDataAnalysisHistoryData(req2);
            var dataAnalysisHistory = res2.Data;

            var res = GascontentanalyzeconfigService.GetAllGascontentanalyzeconfigListCache();
            var allGascontentanalyzeconfig = res.Data;

            foreach (var item in allGascontentanalyzeconfig)
            {
                double height = Convert.ToDouble(item.Height);//巷道高 
                double width = Convert.ToDouble(item.Width);//巷道宽
                double thickness = Convert.ToDouble(item.Thickness);//煤层厚度
                double speed = Convert.ToDouble(item.Speed);//掘进速度m/Month
                double length = Convert.ToDouble(item.Length);//巷道已暴露长度m(40)
                double acreage = Convert.ToDouble(item.Acreage);//断面
                double percent = Convert.ToDouble(item.Percent);//煤的挥发分%====整数
                double wind = Convert.ToDouble(item.Wind);//风量

                var info = dataAnalysisHistory.FirstOrDefault(m => m.PointId == item.Pointid);
                if (info == null)
                {
                    continue;
                }
                var monthAvg = Convert.ToDouble(info.MonthAverageValue);//月平均浓度

                double d = 0;//为煤的周长
                if (thickness <= 3)
                    d = 2 * thickness;
                else
                {
                    d = 2 * (height + width);
                }

                speed = speed / 43200.0;//为每分钟的掘进速度
                var a = d * speed * (2 * Math.Pow(length / speed, 0.5) - 1);
                acreage = acreage * speed * 1.45;//为每分钟的平均落煤量[SVY初台为断面计算之后为落煤量]
                var b = acreage;
                var c = 0.026 * (0.0004 * Math.Pow(percent, 2) + 0.16);
                var d1 = 20.252 * Math.Pow(percent, -0.7064);
                var q = wind * monthAvg;

                //test
                var w0 = (q + b * d1) / (a * c + b);
                //var w0 = 0;

                var comparevalue = Convert.ToDouble(item.Comparevalue);

                //更新实时值
                var req6 = new UpdateRealTimeValueRequest
                {
                    Id = item.Id,
                    RealTimeValue = w0.ToString("0.##"),
                    State = w0 >= comparevalue ? "报警" : "正常"
                };
                GasContentAnalyzeConfigCacheService.UpdateRealTimeValue(req6);

                var req3 = new GetCacheByConditionRequest { Condition = m => m.GasContentAnalyzeConfigId == item.Id };
                var res3 = GasContentAlarmCacheService.GetCacheByCondition(req3);
                var alarmCache = res3.Data;

                if (w0 >= comparevalue)     //报警
                {
                    if (alarmCache.Count == 0)
                    {
                        var pointInfo = allPoint.FirstOrDefault(m => m.PointID == item.Pointid);
                        if (pointInfo == null)
                        {
                            continue;
                        }

                        var alarmInfo = new GasContentAlarmInfo
                        {
                            Id = IdHelper.CreateLongId().ToString(),
                            GasContentAnalyzeConfigId = item.Id,
                            GasContent = w0.ToString("0.##"),
                            Point = pointInfo.Point,
                            Location = pointInfo.Wz
                        };

                        var req4 = new AddCacheRequest
                        {
                            Info = alarmInfo
                        };
                        GasContentAlarmCacheService.AddCache(req4);
                    }
                }
                else        //解除报警
                {
                    if (alarmCache.Count != 0)
                    {
                        var req5 = new DeleteCachesRequest
                        {
                            Infos = alarmCache
                        };
                        GasContentAlarmCacheService.DeleteCaches(req5);
                    }
                }
            }
        }
    }
}

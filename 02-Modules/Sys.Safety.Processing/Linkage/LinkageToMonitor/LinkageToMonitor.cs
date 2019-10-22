using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using System.Data;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Enums.Constant;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.EmergencyLinkHistory;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Processing.Linkage
{
    public class LinkageToMonitor
    {

        private static readonly IPointDefineCacheService PointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();

        /// <summary>最后一次运行时间
        /// 
        /// </summary>
        private static DateTime _lastRunTime = new DateTime();

        /// <summary>运行标记
        /// 
        /// </summary>
        private static bool _isRun;

        /// <summary>处理线程
        /// 
        /// </summary>
        private static Thread _handleThread;

        /// <summary>开始分析
        /// 
        /// </summary>
        public static void Start()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["EmergencyLinkageEnable"].ToString() == "1")
            {
                LogHelper.Info("【LinkageToMonitor】应急联动线程开启。");

                _isRun = true;
                if (_handleThread == null || (_handleThread != null && !_handleThread.IsAlive))
                {
                    _handleThread = new Thread(HandleThreadFun);
                    _handleThread.Start();
                }
            }
            else
            {
                LogHelper.Info("【LinkageToMonitor】应急联动配置未启用。");
            }
        }

        /// <summary>结束分析
        /// 
        /// </summary>
        public static void Stop()
        {
            LogHelper.Info("【LinkageToMonitor】应急联动线程结束。");
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
                    if ((dtNow - _lastRunTime).TotalSeconds >= 20)
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
            LogHelper.Info("【LinkageAnalyze】应急联动分析线程结束成功。");
        }

        /// <summary>
        /// 应急联动数据同步到平台端线程
        /// </summary>
        private static void Analyze()
        {
            try
            {
                PointDefineCacheGetAllRequest pointDefineCacheRequest = new PointDefineCacheGetAllRequest();
                List<Jc_DefInfo> defList = PointDefineCacheService.GetAllPointDefineCache(pointDefineCacheRequest).Data;
                List<RealDevice> realDeviceList = new List<RealDevice>();
                foreach (Jc_DefInfo def in defList)
                {
                    RealDevice realDevice = new RealDevice();
                    realDevice.Code = def.Point;
                    realDevice.Name = def.Wz;
                    realDevice.TypeName = def.DevName;
                    realDevice.Unit = def.Unit;
                    realDevice.RealValue = def.Ssz;
                    int status = 0;
                    if (def.DevPropertyID == 0)
                    {
                        switch (def.State)
                        {
                            case 3:
                            case 4:
                                status = 1;
                                break;
                            case 0:
                            case 1:
                            case 2:
                                status = 5;
                                break;
                            default:
                                status = 1;
                                break;
                        }
                    }
                    else
                    {
                        switch (def.State)
                        {
                            case 21:
                                status = 1;
                                break;
                            case 20:
                                status = 0;
                                break;
                            case 8:
                            case 14:
                                status = 2;
                                break;
                            case 10:
                            case 16:
                                status = 3;
                                break;
                            case 12:
                            case 18:
                                status = 4;
                                break;
                            default:
                                status = 1;
                                break;
                        }
                    }
                    realDevice.Status = status;
                    realDevice.CTime = def.DttStateTime;
                    realDeviceList.Add(realDevice);
                }
                //上传到服务器端
                try
                {
                    string uri = System.Configuration.ConfigurationManager.AppSettings["EmergencyLinkageServerUri"].ToString();

                    object revalue = WcfChannelFactory.ExecuteMethod<IRLService>(uri, "UpdateRealDevices", 101, realDeviceList.ToArray());

                    bool reType = (bool)revalue;                    
                }
                catch(Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

    }
}

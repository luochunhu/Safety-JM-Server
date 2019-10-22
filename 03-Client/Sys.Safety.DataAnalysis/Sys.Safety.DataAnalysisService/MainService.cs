using Basic.Framework.Logging;
using Sys.Safety.ServiceContract;
using Sys.Safety.WebApiAgent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Sys.Safety.DataAnalysis.ServiceHost
{
    public partial class MainService : ServiceBase
    {
        bool _isRun = false;
        static string _serviceName = "DataAnalysisService";

        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Sys.Safety.CoreService.KJ73NService.Start();          

            if (_isRun)
            {
                return;
            }
            _isRun = true;

            RegistService();

            Sys.Safety.Processing.DataAnalysis.DataAnalysisService.Instance.Start();

            //启动双机热备状态检测
            StartHAService();
        }

        protected override void OnStop()
        {

            if (!_isRun)
            {
                return;
            }

            _isRun = false;
            //停止服务
            Sys.Safety.Processing.DataAnalysis.DataAnalysisService.Instance.Stop();
        }

        /// <summary>
        /// 开始启动HA检测
        /// </summary>
        private static void StartHAService()
        {
            Thread haThread = new Thread(new ThreadStart(MonitorHAService));
            haThread.IsBackground = true;
            haThread.Start();

            LogHelper.SystemInfo("热备热切状态检测服务启动");
        }
        /// <summary>
        /// 监测双机热备退出
        /// </summary>
        private static void MonitorHAService()
        {
            LogHelper.Debug("热备热切状态检测服务DoWork()开始执行");
            string haConfigPath = "";
            try
            {
                haConfigPath = System.Configuration.ConfigurationManager.AppSettings["HAPath"].ToString();
                if (!haConfigPath.StartsWith(@"\"))
                {
                    haConfigPath = @"\" + haConfigPath;
                }

                var dr = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                if (dr != null && dr.Parent != null)
                {
                    //获取双机热备的完整目录
                    haConfigPath = dr.Parent.FullName + haConfigPath;
                }
                // LogHelper.SystemInfo("haConfigPath：" + haConfigPath);

                if (!File.Exists(haConfigPath))
                {
                    LogHelper.Debug("HA热备配置文件路径不存，haConfigPath：" + haConfigPath);
                    return;
                }

                LogHelper.Debug("热备热切状态检测服务开始执行循环检测状态");
            }
            catch (Exception ex)
            {
                LogHelper.SystemInfo("Exception:" + ex.ToString());
                return;
            }

            while (true)
            {
                try
                {
                    #region 监测双机热备是否已置退出标记，如果已置，则退出程序
                    string flagIndex = "";

                    string[] tempArrStr = Basic.Framework.Common.IniConfigHelper.INIGetStringValue(haConfigPath, "Backupdb", "PFilePath", "").Split('|');

                    for (int i = 0; i < tempArrStr.Length; i++)
                    {
                        if (tempArrStr[i].ToString().ToLower().Contains(_serviceName.ToLower()))
                        {
                            flagIndex = (i + 1).ToString();
                            break;
                        }
                    }

                    if (Basic.Framework.Common.IniConfigHelper.INIGetStringValue(haConfigPath, "Backupdb", "ProgCloseFlag" + flagIndex, "") == "1")
                    {
                        LogHelper.Debug("检测到HA ProgCloseFlag=1，开始停止" + _serviceName + " ");

                        //停止服务
                        Sys.Safety.Processing.DataAnalysis.DataAnalysisService.Instance.Stop();


                        //重新回写标识
                        Basic.Framework.Common.IniConfigHelper.INIWriteValue(haConfigPath, "Backupdb", "ProgCloseFlag" + flagIndex, "2");

                        LogHelper.Debug("重写 HA ProgCloseFlag=2");

                        System.Environment.Exit(0);
                    }
                    #endregion
                }
                catch (Exception err)
                {
                    LogHelper.Error("监测双机热备退出异常:", err);
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }

            LogHelper.SystemInfo("热备热切状态检测服务停止");
        }
        static void RegistService()
        {
            //大数据分析模块
            Basic.Framework.Ioc.IocManager.RegistObject<IRealMessageService, RealMessageControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisLogService, LargedataAnalysisLogControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmHandleService, AlarmHandleControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisLastChangedService, LargeDataAnalysisLastChangedControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRegionOutageConfigService, RegionOutageConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageConfigService, EmergencyLinkageConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelConfigService, AlarmNotificationPersonnelConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisCacheClientService, LargeDataAnalysisCacheClientControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IManualCrossControlService, ManualCrossControlControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkHistoryService, EmergencyLinkHistoryControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IBroadCastPointDefineService, BroadCastPointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallService, B_CallControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISysEmergencyLinkageService, SysEmergencyLinkageControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_CallService, R_CallControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPointDefineService, PointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PersoninfService, R_PersoninfControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPersonPointDefineService, PersonPointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_HourService, HourServiceControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_AnalysistemplatealarmlevelService, AnalysisTemplateAlarmLevelControllerProxy>();
            Basic.Framework.Ioc.IocManager.Build();
        }
    }
}

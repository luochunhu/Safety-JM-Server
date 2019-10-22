using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Data;
using Sys.Safety.DataContract;

using Sys.Safety.DataAccess;
using Sys.Safety.Services;
using Sys.Safety.Services.Cache;
using Sys.Safety.Services.Chart;
using Sys.Safety.Services.Control;
using Sys.Safety.Services.DataToDb;
using Sys.Safety.Services.Driver;
using Sys.Safety.Services.ListReport;
using Sys.Safety.Services.UserRoleAuthorize;
using Sys.Safety.Request.Cache;
using Sys.Safety.Processing.Statistics;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.Chart;
using Sys.Safety.ServiceContract.Control;
using Sys.Safety.ServiceContract.DataToDb;
using Sys.Safety.ServiceContract.Driver;
using Sys.Safety.ServiceContract.ListReport;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using Sys.Safety.Processing.DataProcessing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Processing.DataAnalysis;
using Basic.Framework.Common;
using Sys.Safety.ServiceContract.App;
using Sys.Safety.Services.App;
using Sys.Safety.Processing.Cache;
using System.Threading;
using System.IO;
using Sys.Safety.Model;
using Sys.Safety.DataAccess;
using Sys.Safety.Services;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Driver;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Services.KJ237Cache;
using Sys.Safety.Request;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Processing.Linkage;
using Sys.Safety.Processing.SpecialDataAnalyze;


namespace Sys.Safety.CoreService
{
    /// <summary>
    /// 安全监控系统核心服务
    /// 20170615 added by 
    /// </summary>
    public class KJ73NService
    {
        //启动标识
        static bool _isRun = false;
        /// <summary>
        /// 统计日志输出线程
        /// </summary>
        private static Thread logOutputThread;
        /// <summary>
        /// 本地日志自动清除线程
        /// </summary>
        private static Thread log4netClearThread;

        /// <summary>
        /// 启动核心服务
        /// </summary>
        /// <returns></returns>
        public static bool Start()
        {
            if (_isRun)
            {
                return true;
            }

            try
            {
                _isRun = true;

                LogHelper.SystemInfo("正在启动安全监控系统服务程序,请等待。。。");
                //1. 加载统一配置
                LoadConfig();

                //2. 注册服务
                RegistService();

                //检测数据库状态  20170712
                CheckDBStatus();

                //结束所有应急联动和call
                LinkageAnalyze.EndAllLinkageAndCall();

                //删除数据库中交叉控制表中除手动控制外的所有其他控制
                DeleteOtherManualCrossControlFromDB();

                //3. 加载缓存模块  20170601
                LoadCache();


                //4. 数据处理线程
                DataProcHandle.Instance.Start();
                //LogHelper.SystemInfo("启动处理模块完成");

                //PersonDataProcHandle.Instance.Start();
                //LogHelper.Info("启动人员定位数据处理线程完成");

                //RatioAlarmProcHandle.Instance.Start();
                //LogHelper.SystemInfo("启动倍数报警模块完成");

                //5. 开始数据分析
                //DataAnalysisService.Instance.Start();

                //6. 数据统计
                DataStatistics.Start();
                Calibration.Start();
                PowerboxchargeManage.Instance.Start();//分站充放电记录统计
                //ElectricityStatistics.Start();//增加电源箱5分钟电量消耗统计  20180125
                //LogHelper.SystemInfo("启动统计模块完成");

                //7.应急联动分析
                LinkageAnalyze.Start();


                LinkageToMonitor.Start();

                //LogHelper.SystemInfo("启动联动模块完成");

                GasContentAnalyze.Start();
                //LogHelper.SystemInfo("启动瓦斯分析模块完成");

                //增加队列积压日志输出
                logOutputThread = new Thread(logOutputClass);
                logOutputThread.IsBackground = true;
                logOutputThread.Start();

                //增加本地日志自动清除功能
                log4netClearThread = new Thread(ClearLog4netLog);
                log4netClearThread.IsBackground = true;
                log4netClearThread.Start();

                //注册退出事件，用于客户端远程调用   20180123
                ConfigService.severCloseEvent = new ConfigService.SeverCloseEvent(SeverCloseEvent);

                LogHelper.SystemInfo("安全监控服务启动成功");
            }
            catch (Exception ex)
            {
                _isRun = false;

                LogHelper.SystemInfo("安全监控服务启动失败，具体查看错误日志。");
                LogHelper.Error("安全监控服务启动失败，错误原因：" + ex.ToString());
            }

            return _isRun;
        }
        private static void SeverCloseEvent()
        {
            Stop();

            //退出进程
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 停止核心有服务
        /// </summary>
        /// <returns></returns>
        public static bool Stop()
        {
            if (!_isRun)
            {
                return true;
            }
            try
            {
                _isRun = false;

                //todo 实现停止代码
                //停止数据处理模块
                DataProcHandle.Instance.Stop();

                //PersonDataProcHandle.Instance.Stop();

                //DataAnalysisService.Instance.Stop();               

                //RatioAlarmProcHandle.Instance.Stop();

                //数据统计线程
                DataStatistics.Stop();
                Calibration.Stop();

                //应急联动分析模块
                LinkageAnalyze.Stop();
                LinkageToMonitor.Stop();

                GasContentAnalyze.Stop();

                //缓存线程结束
                StopCacheInstance();

                LogHelper.SystemInfo("******************安全监控核心服务停止成功******************");
            }
            catch (Exception ex)
            {
                _isRun = true;

                LogHelper.SystemInfo("安全监控服务停止失败，具体查看错误日志。");
                LogHelper.Error("安全监控服务停止失败，错误原因：" + ex.ToString());
            }

            return _isRun;
        }


        /// <summary>
        /// 加载统一配置
        /// </summary>
        private static void LoadConfig()
        {
            string dbIp = System.Configuration.ConfigurationManager.AppSettings["dbIp"];
            string dbPort = System.Configuration.ConfigurationManager.AppSettings["dbPort"];
            string dbUserName = System.Configuration.ConfigurationManager.AppSettings["dbUserName"];
            string dbPassword = System.Configuration.ConfigurationManager.AppSettings["dbPassword"];
            string dbName = System.Configuration.ConfigurationManager.AppSettings["dbName"];
            //读取配置拼接数据库字符串
            string connString = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};CharSet=utf8;", dbIp, dbPort, dbUserName, dbPassword, dbName);

            //string connString = string.Format("Data Source ={0}; Initial Catalog = {1}; uid = {2}; pwd = {3};", dbIp, dbName, dbUserName, dbPassword);

            //更新当前进程的connectionStrings.mysql节点的配置，并刷新           
            //TODO：2017.0620  待确认这里如何修改，当宿主程序为IIS程序时，这里不能修改数据库连接串（即使修改后，也会导致程序重启）           
            AppConfigHelper.SaveConnectionStringsConfig("mysql", connString, "MySql.Data.MySqlClient");


            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "DatabaseType", "3");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "MasterDatabase", connString);
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "SlaveDatabase", connString);

            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "IsEnabledReadWriteModel", "true");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "IsDebug", "true");

            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "LogWriterQueues", "log_login,log_info");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "EnableLogLevel_None", "true");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "EnableLogLevel_SessionLog", "true");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "EnableLogLevel_OperationLog", "true");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "EnableLogLevel_Info", "true");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "EnableLogLevel_Debug", "true");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "EnableLogLevel_Exception", "true");
            Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "EnableTxtLog", "true");

            //Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "SessionAddress", "192.168.1.83:6379");
            //Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "Domain", "mas300275.com");
            //Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "SessionLifeTime", "20");
            //Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "SessionName", "MAS_Web_SessionId");
            //Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "EnableRedisSessionServer", "true");


            //Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "MessageQueueServer", "192.168.1.83:6379");
            //Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "WebApiTokenSessionServer", "192.168.1.83:6379");
            //Basic.Framework.Configuration.SecondLevelCache.AddOrUpdateAppConfiguration("Global", "MessageQueueQueryTime", "10");


            Basic.Framework.Data.PlatRuntime.Items.Add("_MASDataContext", new KJ73NDataContext());


            //LogHelper.SystemInfo("加载系统配置完成");
        }


        /// <summary>
        /// 注册服务
        /// </summary>
        private static void RegistService()
        {

            if (Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetBool("EnableRPC", false))
            {
                //远程调用
                RegistRemoteService();
            }
            else
            {
                //本地调用
                RegistLocalService();
            }

            //初始化数据访问层
            RegisterDataContext();

            //LogHelper.SystemInfo("注册系统服务完成");
        }

        /// <summary>
        /// 初始化数据访问层
        /// </summary>
        private static void RegisterDataContext()
        {
            Basic.Framework.Data.DbContextManager.RegistCreateDataContextHandler((bool isMaster) =>
            {
                if (isMaster)
                    return new Sys.Safety.Data.KJ73NDataContextForWrite();
                else
                    return new Sys.Safety.Data.KJ73NDataContextForRead();
            });
        }

        /// <summary>
        /// 注册本地服务
        /// </summary>
        private static void RegistLocalService()
        {
            //注册数据库的DataContext
            Basic.Framework.Ioc.IocManager.RuntimeType = Basic.Framework.Data.RuntimeType.Windows;
            Basic.Framework.Ioc.IocManager.RegistObject<DbContext, KJ73NDataContextForRead>("slaveDatabase", Basic.Framework.Ioc.InstanceType.PerDependency);
            Basic.Framework.Ioc.IocManager.RegistObject<DbContext, KJ73NDataContextForWrite>("masterDatabase", Basic.Framework.Ioc.InstanceType.PerDependency);

            Basic.Framework.Ioc.IocManager.RegistObject<IPositionRepository, PositionRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPositionService, PositionService>();

            //注册客户端框架基础服务  
            Basic.Framework.Ioc.IocManager.RegistObject<IConfigRepository, ConfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IConfigService, ConfigService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEnumcodeRepository, EnumcodeRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEnumcodeService, EnumcodeService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEnumtypeRepository, EnumtypeRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEnumtypeService, EnumtypeService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IOperatelogRepository, OperatelogRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IOperatelogService, OperatelogService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRemoteStateService, RemoteStateService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRunlogRepository, RunlogRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRunlogService, RunlogService>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISettingRepository, SettingRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISettingService, SettingService>();
            //注册客户端框架登录、用户、角色、权限、菜单、请求库管理服务  
            Basic.Framework.Ioc.IocManager.RegistObject<ILoginService, LoginService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMenuRepository, MenuRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMenuService, MenuService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRequestRepository, RequestRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRequestService, RequestService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRightRepository, RightRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRightService, RightService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRoleRepository, RoleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRoleService, RoleService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRolerightRepository, RolerightRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRolerightService, RolerightService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IUserRepository, UserRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IUserService, UserService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IUserroleRepository, UserroleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IUserroleService, UserroleService>();

            //报表模块
            Basic.Framework.Ioc.IocManager.RegistObject<IClassRepository, ClassRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IClassService, ClassService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListcommandexRepository, ListcommandexRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListcommandexService, ListcommandexService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdataexRepository, ListdataexRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdataexService, ListdataexService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdatalayountRepository, ListdatalayountRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdatalayountService, ListdatalayountService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdisplayexRepository, ListdisplayexRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdisplayexService, ListdisplayexService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListexRepository, ListexRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListexService, ListexService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListmetadataRepository, ListmetadataRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListmetadataService, ListmetadataService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListtempleRepository, ListtempleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListtempleService, ListtempleService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMetadatafieldsRepository, MetadatafieldsRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMetadatafieldsService, MetadatafieldsService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMetadataRepository, MetadataRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMetadataService, MetadataService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdataremarkRepository, ListdataremarkRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdataremarkService, ListdataremarkService>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISqlService, SqlService>();

            //曲线模块
            Basic.Framework.Ioc.IocManager.RegistObject<IChartService, ChartService>();

            //控制模块
            Basic.Framework.Ioc.IocManager.RegistObject<IControlService, ControlService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionHistoryDataService, StaionHistoryDataService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionHistoryDataRepository, StaionHistoryDataRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionControlHistoryDataService, StaionControlHistoryDataService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionControlHistoryDataRepository, StaionControlHistoryDataRepository>();

            //标校模块
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationDefRepository, CalibrationDefRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationDefService, CalibrationDefService>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationStatisticsService, CalibrationStatisticsService>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationStatisticsRepository, CalibrationStatisticsRepository>();

            //缓存模块
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmRecordRepository, AlarmRecordRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmRecordService, AlarmRecordService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmCacheService, AlarmCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IConfigCacheService, ConfigCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceClassCacheService, DeviceClassCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceDefineCacheService, DeviceDefineCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDevicePropertyCacheService, DevicePropertyCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceTypeCacheService, DeviceTypeCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisConfigCacheService, LargeDataAnalysisConfigCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IManualCrossControlCacheService, ManualCrossControlCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<INetworkModuleCacheService, NetworkModuleCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPointDefineCacheService, PointDefineCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPositionCacheService, PositionCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRunLogCacheService, RunLogCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISettingCacheService, SettingCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAutomaticArticulatedDeviceCacheService, AutomaticArticulatedDeviceCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRCallCacheService, RCallCacheService>();//新增加人员缓存注册  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IRPersoninfCacheService, RPersoninfCacheService>();//新增加人员缓存注册  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IRPointDefineCacheService, RPointDefineCacheService>();//新增加人员缓存注册  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IRPRealCacheService, RPrealCacheService>();//新增加人员缓存注册  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IRsyncLocalCacheService, RsyncLocalCacheService>();//新增加人员缓存注册  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IRUndefinedDefCacheService, RUndefinedDefCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRKqbcCacheService, RKqbcCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAreaCacheService, AreaCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGasContentAlarmCacheService, GasContentAlarmCacheService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IAccumulationDayService, AccumulationDayService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAccumulationHourService, AccumulationHourService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAccumulationMonthService, AccumulationMonthService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAccumulationYearService, AccumulationYearService>();
            //入库模块
            Basic.Framework.Ioc.IocManager.RegistObject<IAccumulationDayRepository, AccumulationDayRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_Ll_DInfo>, AccumulationDayDataToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAccumulationHourRepository, AccumulationHourRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_Ll_HInfo>, AccumulationHourDataToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAccumulationMonthRepository, AccumulationMonthRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_Ll_MInfo>, AccumulationMonthDataToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAccumulationYearRepository, AccumulationYearRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_Ll_YInfo>, AccumulationYearDataToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmRecordRepository, AlarmRecordRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_BInfo>, AlarmDataInsertToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_KdRepository, Jc_KdRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_KdInfo>, FeedDataInsertToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_MRepository, Jc_MRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_MInfo>, FiveDataInsertToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_McRepository, Jc_McRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_McInfo>, InitialDataInsertToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_RRepository, Jc_RRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<Jc_RInfo>, RunLogDataInsertToDbService>();

            //注册人员定位的入库接口  20171208
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<R_RInfo>, R_RunLogDataInsertToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<R_BInfo>, R_AlarmDataInsertToDbService>();


            Basic.Framework.Ioc.IocManager.RegistObject<IStaionHistoryDataRepository, StaionHistoryDataRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionHistoryDataService, StaionHistoryDataService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IStaionControlHistoryDataRepository, StaionControlHistoryDataRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionControlHistoryDataService, StaionControlHistoryDataService>();


            //实时模块
            Basic.Framework.Ioc.IocManager.RegistObject<IRealMessageService, RealMessageService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRealMessageRepository, RealMessageRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationService, CalibrationService>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationRepository, CalibrationRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceKoriyasuService, DeviceKoriyasuService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceKoriyasuRepository, DeviceKoriyasuRepository>();

            //报警模块
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmService, AlarmService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmRepository, AlarmRepository>();

            //大数据分析模块
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateConfigRepository, AnalysisTemplateConfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateConfigService, AnalysisTemplateConfigService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateRepository, AnalysisTemplateRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateService, AnalysisTemplateService>();      //模板
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalyticalExpressionRepository, AnalyticalExpressionRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalyticalExpressionService, AnalyticalExpressionService>();//表达式
            Basic.Framework.Ioc.IocManager.RegistObject<IExpressionConfigRepository, ExpressionConfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IExpressionConfigService, ExpressionConfigService>(); //表达式配置
            Basic.Framework.Ioc.IocManager.RegistObject<IParameterRepository, ParameterRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IParameterService, ParameterService>();    //参数表
            Basic.Framework.Ioc.IocManager.RegistObject<IFactorRepository, FactorRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IFactorService, FactorService>();//因子表
            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisConfigRepository, LargedataAnalysisConfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisConfigService, LargedataAnalysisConfigService>();//大数据分析配置表
            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisLogRepository, LargedatAanalysisLogRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisLogService, LargedataAnalysisLogService>();//大数据分析记录表
            Basic.Framework.Ioc.IocManager.RegistObject<ISetAnalysisModelPointRecordRepository, SetAnalysisModelPointRecordRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISetAnalysisModelPointRecordService, SetAnalysisModelPointRecordService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelConfigRepository, AlarmNotificationPersonnelConfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelConfigService, AlarmNotificationPersonnelConfigService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageConfigRepository, EmergencyLinkageConfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageConfigService, EmergencyLinkageConfigService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRegionOutageConfigRepository, RegionOutageConfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRegionOutageConfigService, RegionOutageConfigService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelRepository, AlarmNotificationPersonnelRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelService, AlarmNotificationPersonnelService>();
            //Basic.Framework.Ioc.IocManager.RegistObject<IFactorCalculateService, FactorCalculateService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmHandleRepository, AlarmHandleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmHandleService, AlarmHandleService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelRepository, AlarmNotificationPersonnelRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelService, AlarmNotificationPersonnelService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelConfigRepository, AlarmNotificationPersonnelConfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelConfigService, AlarmNotificationPersonnelConfigService>();


            Basic.Framework.Ioc.IocManager.RegistObject<IJc_HourRepository, Jc_HourRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_HourService, Jc_HourService>();//大数据分析获取因子值时用到，如果已经注册此处不再注册.
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisCacheClientService, LargeDataAnalysisCacheClientService>();//客户端获取分析换成的服务包了一层
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisLastChangedService, LargeDataAnalysisLastChangedService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_AnalysistemplatealarmlevelRepository, Jc_AnalysistemplatealarmlevelRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_AnalysistemplatealarmlevelService, Jc_AnalysistemplatealarmlevelService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateAlarmLevelCacheService, AnalysisTemplateAlarmLevelCacheService>();

            //定义模块
            Basic.Framework.Ioc.IocManager.RegistObject<IAreaRepository, AreaRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAreaService, AreaService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IPointDefineRepository, PointDefineRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_DefRepository, R_DefRepository>();//新增加人员入库  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IR_DefService, R_DefService>();//新增加人员入库  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IR_RestrictedpersonRepository, R_RestrictedpersonRepository>();//新增加人员定位识别器关联限制进入、禁止进入人员接口  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IR_RestrictedpersonService, RestrictedpersonService>();//新增加人员定位识别器关联限制进入、禁止进入人员接口  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IPersonPointDefineService, PersonPointDefineService>();//新增加人员定位定义接口  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IAllSystemPointDefineService, AllSystemPointDefineService>();//新增多系统查询接口  20171122
            Basic.Framework.Ioc.IocManager.RegistObject<IPointDefineService, PointDefineService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceDefineRepository, DeviceDefineRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceDefineService, DeviceDefineService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IManualCrossControlRepository, ManualCrossControlRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IManualCrossControlService, ManualCrossControlService>();
            Basic.Framework.Ioc.IocManager.RegistObject<INetworkModuleRepository, NetworkModuleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<INetworkModuleService, NetworkModuleService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPositionRepository, PositionRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPositionService, PositionService>();

            //图形模块
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicspointsinfRepository, GraphicspointsinfRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicspointsinfService, GraphicspointsinfService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsbaseinfRepository, GraphicsbaseinfRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsbaseinfService, GraphicsbaseinfService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsrouteinfRepository, GraphicsrouteinfRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsrouteinfService, GraphicsrouteinfService>();


            Basic.Framework.Ioc.IocManager.RegistObject<IJc_RService, Jc_RService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_RRepository, Jc_RRepository>();

            //驱动接口服务
            Basic.Framework.Ioc.IocManager.RegistObject<IDriverManualCrossControlService, DriverManualCrossControlService>();

            //App接口服务
            Basic.Framework.Ioc.IocManager.RegistObject<IKJ73NAppService, KJ73NAppService>();

            //平台调用接口
            Basic.Framework.Ioc.IocManager.RegistObject<IPtQueryService, PtQueryService>();

            //web接口服务
            Basic.Framework.Ioc.IocManager.RegistObject<IWebmenuRepository, WebmenuRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IWebmenuService, WebmenuService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IReportService, ReportService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRolewebmenuRepository, RolewebmenuRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRolewebmenuService, RolewebmenuService>();

            //倍数报警            
            Basic.Framework.Ioc.IocManager.RegistObject<IJC_MultiplesettingRepository, JC_MultiplesettingRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJC_MultiplesettingService, JC_MultiplesettingService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJC_MbRepository, JC_MbRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJC_MbService, JC_MbService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IRatioAlarmCacheService, RatioAlarmCacheService>();

            //短信
            Basic.Framework.Ioc.IocManager.RegistObject<IMsgRuleRepository, MsgRuleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMsgRuleService, MsgRuleService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMsgUserRuleRepository, MsgUserRuleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMsgUserRuleService, MsgUserRuleService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMsgLogRepository, MsgLogRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMsgLogService, MsgLogService>();

            //区域
            //Basic.Framework.Ioc.IocManager.RegistObject<IAreaService, AreaService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAreaRuleRepository, AreaRuleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAreaRuleService, AreaRuleService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_ArearestrictedpersonRepository, R_ArearestrictedpersonRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_ArearestrictedpersonService, R_ArearestrictedpersonService>();

            //快捷菜单
            Basic.Framework.Ioc.IocManager.RegistObject<IShortCutMenuRepository, ShortCutMenuRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IShortCutMenuService, ShortCutMenuService>();

            //分站远程升级
            Basic.Framework.Ioc.IocManager.RegistObject<IStationUpdateService, StationUpdateService>();

            //人员定位
            Basic.Framework.Ioc.IocManager.RegistObject<IR_DeptRepository, R_DeptRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_DeptService, R_DeptService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PersoninfRepository, R_PersoninfRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PersoninfService, R_PersoninfService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PrealRepository, R_PrealRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PrealService, R_PrealService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_UndefinedDefRepository, R_UndefinedDefRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_UndefinedDefService, R_UndefinedDefService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_KqbcRepository, R_KqbcRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_KqbcService, R_KqbcService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_CallRepository, R_CallRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_CallService, R_CallService>();


            Basic.Framework.Ioc.IocManager.RegistObject<IR_PbRepository, R_PbRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PbService, R_PbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<R_PbInfo>, R_PbDataToDbService>();


            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<R_PhistoryInfo>, R_PhistoryDataToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhistoryRepository, R_PhistoryRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhistoryService, R_PhistoryService>();


            Basic.Framework.Ioc.IocManager.RegistObject<IInsertToDbService<R_PhjInfo>, R_PhjDataToDbService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhjRepository, R_PhjRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhjService, R_PhjService>();


            Basic.Framework.Ioc.IocManager.RegistObject<IR_PBCacheService, R_PBCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAreaCacheService, AreaCacheService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IRsyncLocalCacheService, RsyncLocalCacheService>();

            //视频
            Basic.Framework.Ioc.IocManager.RegistObject<IV_DefRepository, V_DefRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IV_DefService, V_DefService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IV_DefCacheService, V_DefCacheService>();

            //应急联动
            Basic.Framework.Ioc.IocManager.RegistObject<ISysEmergencyLinkageRepository, SysEmergencyLinkageRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISysEmergencyLinkageService, SysEmergencyLinkageService>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISysEmergencyLinkageCacheService, SysEmergencyLinkageCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkHistoryRepository, EmergencyLinkHistoryRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkHistoryService, EmergencyLinkHistoryService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageHistoryMasterPointAssRepository, EmergencyLinkageHistoryMasterPointAssRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageHistoryMasterPointAssService, EmergencyLinkageHistoryMasterPointAssService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageMasterAreaAssRepository, EmergencyLinkageMasterAreaAssRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageMasterAreaAssService, EmergencyLinkageMasterAreaAssService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageMasterDevTypeAssRepository, EmergencyLinkageMasterDevTypeAssRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageMasterDevTypeAssService, EmergencyLinkageMasterDevTypeAssService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageMasterPointAssRepository, EmergencyLinkageMasterPointAssRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageMasterPointAssService, EmergencyLinkageMasterPointAssService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageMasterTriDataStateAssRepository, EmergencyLinkageMasterTriDataStateAssRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageMasterTriDataStateAssService, EmergencyLinkageMasterTriDataStateAssService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkagePassiveAreaAssRepository, EmergencyLinkagePassiveAreaAssRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkagePassiveAreaAssService, EmergencyLinkagePassiveAreaAssService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkagePassivePersonAssRepository, EmergencyLinkagePassivePersonAssRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkagePassivePersonAssService, EmergencyLinkagePassivePersonAssService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkagePassivePointAssRepository, EmergencyLinkagePassivePointAssRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkagePassivePointAssService, EmergencyLinkagePassivePointAssService>();

            //广播
            Basic.Framework.Ioc.IocManager.RegistObject<IB_DefRepository, B_DefRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_DefService, B_DefService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_DefCacheService, B_DefCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IBroadCastPointDefineService, BroadCastPointDefineService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallRepository, B_CallRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallService, B_CallService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallCacheService, B_CallCacheService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_MusicfilesRepository, B_MusicfilesRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_MusicfilesService, B_MusicfilesService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IRemoteStateService, RemoteStateService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallpointlistRepository, B_CallpointlistRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallpointlistService, B_CallpointlistService>();

            //电源箱放电历史记录    20180124          
            Basic.Framework.Ioc.IocManager.RegistObject<IPowerboxchargehistoryRepository, PowerboxchargehistoryRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPowerboxchargehistoryService, PowerboxchargehistoryService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IGascontentanalyzeconfigRepository, GascontentanalyzeconfigRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGascontentanalyzeconfigService, GascontentanalyzeconfigService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGasContentAnalyzeConfigCacheService, GasContentAnalyzeConfigCacheService>();

            Basic.Framework.Ioc.IocManager.RegistObject<IKJ_AddresstypeRepository, KJ_AddresstypeRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IKJ_AddresstypeService, KJ_AddresstypeService>();
            Basic.Framework.Ioc.IocManager.RegistObject<IKJ_AddresstyperuleRepository, KJ_AddresstyperuleRepository>();
            Basic.Framework.Ioc.IocManager.RegistObject<IKJ_AddresstyperuleService, KJ_AddresstyperuleService>();

            Basic.Framework.Ioc.IocManager.Build();

        }
        /// <summary>
        /// 注册远程服务
        /// </summary>
        private static void RegistRemoteService()
        {
            //Basic.Framework.Ioc.IocManager.RegistObject<IConfigService, ConfigControllerProxy>();           

            Basic.Framework.Ioc.IocManager.Build();
        }

        /// <summary>
        /// 检测数据库状态
        /// </summary>
        private static void CheckDBStatus()
        {
            //LogHelper.SystemInfo("检测数据库状态");

            IConfigService configService = ServiceFactory.Create<IConfigService>();

            while (true)
            {
                //LogHelper.Debug("正在检测数据数据库状态");
                var response = configService.GetDbState();
                if (response.IsSuccess && response.Data)
                {
                    break;
                }

                Thread.Sleep(1000 * 1);
            }

            //LogHelper.SystemInfo("检测数据库状态完成");
        }

        /// <summary>
        /// 加载缓存模块
        /// </summary>
        private static void LoadCache()
        {
            try
            {
                //LogHelper.SystemInfo("正在启动缓存模块");
                //加载设备性质
                IDevicePropertyCacheService _DevicePropertyCacheService = ServiceFactory.Create<IDevicePropertyCacheService>();
                _DevicePropertyCacheService.LoadDevicePropertyCache(new DevicePropertyCacheLoadRequest());
                //加载设备种类
                IDeviceClassCacheService _DeviceClassCacheService = ServiceFactory.Create<IDeviceClassCacheService>();
                _DeviceClassCacheService.LoadDeviceClassCache(new DeviceClassCacheLoadRequest());
                //加载设备型号
                IDeviceTypeCacheService _DeviceTypeCacheService = ServiceFactory.Create<IDeviceTypeCacheService>();
                _DeviceTypeCacheService.LoadDeviceTypeCache(new DeviceTypeCacheLoadRequest());
                //加载Config配置
                IConfigCacheService _ConfigCacheService = ServiceFactory.Create<IConfigCacheService>();
                _ConfigCacheService.LoadConfigCache(new ConfigCacheLoadRequest());
                //加载设备类型
                IDeviceDefineCacheService _DeviceDefineCacheService = ServiceFactory.Create<IDeviceDefineCacheService>();
                _DeviceDefineCacheService.LoadDeviceDefineCache(new DeviceDefineCacheLoadRequest());
                //加载安装位置
                IPositionCacheService _PositionCacheService = ServiceFactory.Create<IPositionCacheService>();
                _PositionCacheService.LoadPositionCache(new PositonCacheLoadRequest());
                //加载测点定义(安全监控)
                IPointDefineCacheService _PointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
                _PointDefineCacheService.LoadPointDefineCache(new PointDefineCacheLoadRequest());
                //加载测点定义(人员定位)  20171122
                IRPointDefineCacheService _RPointDefineCacheService = ServiceFactory.Create<IRPointDefineCacheService>();
                _RPointDefineCacheService.LoadRPointDefineCache(new RPointDefineCacheLoadRequest());
                //加载测点定义(广播系统)  20180131
                IB_DefCacheService _B_DefCacheService = ServiceFactory.Create<IB_DefCacheService>();
                _B_DefCacheService.LoadCache(new B_DefCacheLoadRequest());

                //加载网络模块
                INetworkModuleCacheService _NetworkModuleCacheService = ServiceFactory.Create<INetworkModuleCacheService>();
                _NetworkModuleCacheService.LoadNetworkModuleCache(new NetworkModuleCacheLoadRequest());
                //加载交叉控制
                IManualCrossControlCacheService _ManualCrossControlCacheService = ServiceFactory.Create<IManualCrossControlCacheService>();
                _ManualCrossControlCacheService.LoadManualCrossControlCache(new ManualCrossControlCacheLoadRequest());

                //加载交叉控制信息到DEF缓存  2017.10.25 by
                IDriverManualCrossControlService _DriverManualCrossControlService = ServiceFactory.Create<IDriverManualCrossControlService>();
                DriverManualCrossControlReLoadRequest reLoadRequest = new DriverManualCrossControlReLoadRequest();
                _DriverManualCrossControlService.ReLoad(reLoadRequest);

                //报警记录缓存改到补录结束时间之后加载  20170616
                //加载setting配置
                ISettingCacheService _SettingCacheService = ServiceFactory.Create<ISettingCacheService>();
                _SettingCacheService.LoadSettingCache(new SettingCacheLoadRequest());

                //加载大数据分析模型配置
                ILargeDataAnalysisConfigCacheService _LargeDataAnalysisConfigCacheService = ServiceFactory.Create<ILargeDataAnalysisConfigCacheService>();
                _LargeDataAnalysisConfigCacheService.LoadLargeDataAnalysisConfigCache(new LargeDataAnalysisConfigCacheLoadRequest() { });

                //加载人员定位人员信息缓存
                IRPersoninfCacheService _RPersoninfCacheService = ServiceFactory.Create<IRPersoninfCacheService>();
                _RPersoninfCacheService.LoadRPersoninfCache(new RPersoninfCacheLoadRequest());

                //加载人员定位班次缓存
                IRKqbcCacheService _RKqbcCacheService = ServiceFactory.Create<IRKqbcCacheService>();
                _RKqbcCacheService.LoadRKqbcCache(new RKqbcCacheLoadRequest());

                //加载区域缓存
                IAreaCacheService _AreaCacheService = ServiceFactory.Create<IAreaCacheService>();
                _AreaCacheService.LoadAreaCache(new AreaCacheLoadRequest());

                //加载人员呼叫缓存 
                IRCallCacheService _RCallCacheService = ServiceFactory.Create<IRCallCacheService>();
                _RCallCacheService.LoadRCallCache(new RCallCacheLoadRequest());

                IB_CallCacheService b_CallCacheService = ServiceFactory.Create<IB_CallCacheService>();
                b_CallCacheService.LoadBCallCache(new BCallCacheLoadRequest());

                //加载视频定义缓存
                IV_DefCacheService _VDefCacheService = ServiceFactory.Create<IV_DefCacheService>();
                _VDefCacheService.Load();

                //加载应急联动缓存
                var sysEmergencyLinkageCacheService = ServiceFactory.Create<ISysEmergencyLinkageCacheService>();
                sysEmergencyLinkageCacheService.LoadSysEmergencyLinkageCache(
                    new EmergencyLinkageConfigCacheLoadRequest());

                IGasContentAnalyzeConfigCacheService gasContentAnalyzeConfigCacheService = ServiceFactory.Create<IGasContentAnalyzeConfigCacheService>();
                gasContentAnalyzeConfigCacheService.LoadCache();

                //加载传感器分级报警配置缓存
                var analysisTemplateAlarmLevelCacheService = ServiceFactory.Create<IAnalysisTemplateAlarmLevelCacheService>();
                analysisTemplateAlarmLevelCacheService.LoadAnalysisTemplateAlarmLevelCache(new AnalysisTemplateAlarmLevelCacheLoadRequest());

                //缓存处理线程，先不用
                //AlarmCacheCleanTask.Instance.Start();
                //RunLogCacheCleanTask.Instance.Start();

                //写上下文缓存
                if (!Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_DefUpdateTime"))
                {
                    Basic.Framework.Data.PlatRuntime.Items.Add("_DefUpdateTime", DateTime.Now);//定义改变时间
                }
                if (!Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_AlarmCfgUpdateTime"))
                {
                    Basic.Framework.Data.PlatRuntime.Items.Add("_AlarmCfgUpdateTime", DateTime.Now);//报警配置改变时间
                }
                if (!Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_RealListCfgUpdateTime"))
                {
                    Basic.Framework.Data.PlatRuntime.Items.Add("_RealListCfgUpdateTime", DateTime.Now);//列表配置改变时间
                }

                //LogHelper.SystemInfo("启动缓存模块完成");
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                //LogHelper.Error("加载缓存模块异常，即将退出程序！！");
                //Thread.Sleep(3000);            
                //System.Environment.Exit(0);//加载缓存异常，退出程序  20170706
            }
        }

        private static void StopCacheInstance()
        {
            IAlarmCacheService alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
            alarmCacheService.StopCleanAlarmCacheThread(new AlarmCacheStopCleanRequest());

            IRunLogCacheService runlogCahceService = ServiceFactory.Create<IRunLogCacheService>();
            runlogCahceService.StopCleanRunlogCacheThread(new RunLogCacheStopCleanRunLogRequest());

            IPointDefineCacheService pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
            pointDefineCacheService.Stop();

            //退出时，结束R_PB缓存  20171206
            IR_PBCacheService _R_PBCacheService = ServiceFactory.Create<IR_PBCacheService>();
            R_PBCacheStopCleanRequest R_PBCacheRequest = new R_PBCacheStopCleanRequest();
            _R_PBCacheService.StopCleanR_PBCacheThread(R_PBCacheRequest);

        }

        private static void logOutputClass()
        {
            while (_isRun)
            {
                try
                {
                    //输出统计日志
                    DataProcHandle.Instance.OutputStaticLog();
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(10000);
            }
        }
        /// <summary>
        /// Log4net日志自动清除功能
        /// </summary>
        private static void ClearLog4netLog()
        {
            string sondirsonDate = string.Empty;
            DateTime sondirsonDateTime = new DateTime();
            TimeSpan ts = new TimeSpan();
            string AutoClearLog4netLog = System.Configuration.ConfigurationManager.AppSettings["AutoClearLog4netLog"].ToString().ToLower();
            string Log4netFilePath = System.Configuration.ConfigurationManager.AppSettings["Log4netFilePath"].ToString();
            string ClearTimeLongAgo = System.Configuration.ConfigurationManager.AppSettings["ClearTimeLongAgo"].ToString();

            while (_isRun)
            {
                try
                {
                    //清除同步debug日志 
                    if (AutoClearLog4netLog == "true")
                    {
                        if (Directory.Exists(Log4netFilePath))
                        {
                            DirectoryInfo dir = new DirectoryInfo(Log4netFilePath);
                            DirectoryInfo[] dirs = dir.GetDirectories();
                            foreach (DirectoryInfo sondir in dirs)
                            {
                                DirectoryInfo[] sondirsons = sondir.GetDirectories();
                                foreach (DirectoryInfo sondirson in sondirsons)
                                {
                                    sondirsonDate = sondirson.Name.Substring(0, 4) + "-" + sondirson.Name.Substring(4, 2) + "-" + sondirson.Name.Substring(6, 2);
                                    sondirsonDateTime = DateTime.Parse(sondirsonDate);
                                    ts = DateTime.Now - sondirsonDateTime;
                                    if (ts.TotalDays > int.Parse(ClearTimeLongAgo))
                                    {
                                        Directory.Delete(sondirson.FullName, true);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.Error("清除mysql同步日志失败,详细信息" + ex.Message + ex.StackTrace);
                }
                Thread.Sleep(3600000);//每小时执行一次
            }
        }

        private static void DeleteOtherManualCrossControlFromDB()
        {
            try
            {
                IManualCrossControlService servie = ServiceFactory.Create<IManualCrossControlService>();
                servie.DeleteOtherManualCrossControlFromDB();
            }
            catch (Exception ex)
            {
                LogHelper.Error("DeleteOtherManualCrossControlFromDB Error:" + ex.Message);
            }
        }
    }
}

using Sys.Safety.ServiceContract;
using Sys.Safety.WebApiAgent.UserRoleAuthorize;

using Sys.Safety.WebApiAgent.CBFCommon;
using Sys.Safety.ServiceContract.UserRoleAuthorize;

using Sys.Safety.ServiceContract.Chart;
using Sys.Safety.ServiceContract.Control;
using Sys.Safety.ServiceContract.ListReport;
using Sys.Safety.WebApiAgent;
using Sys.Safety.WebApiAgent.Chart;
using Sys.Safety.WebApiAgent.Control;
using Sys.Safety.WebApiAgent.ListReport;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.WebApiAgent.RealModule;
using Sys.Safety.WebApiProxy;

namespace Sys.Safety.Client.WindowHost
{
    /// <summary>
    /// 服务注册管理类
    /// </summary>
    public class ServiceManager
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        public static void RegisterService()
        {
            Basic.Framework.Ioc.IocManager.RegistObject<IPositionService, PositionControllerProxy>();
            //注册客户端框架基础服务             
            Basic.Framework.Ioc.IocManager.RegistObject<IConfigService, ConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEnumcodeService, EnumcodeControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEnumtypeService, EnumtypeControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IOperatelogService, OperatelogControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRemoteStateService, RemoteStateControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRunlogService, RunlogControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISettingService, SettingControllerProxy>();
            //注册客户端框架登录、用户、角色、权限、菜单、请求库管理服务  
            Basic.Framework.Ioc.IocManager.RegistObject<ILoginService, LoginControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMenuService, MenuControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRequestService, RequestControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRightService, RightControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRoleService, RoleControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRolerightService, RolerightControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IUserService, UserControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IUserroleService, UserroleControllerProxy>();

            //报表模块
            Basic.Framework.Ioc.IocManager.RegistObject<IClassService, ClassControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListcommandexService, ListcommandexControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdataexService, ListdataexControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdatalayountService, ListdatalayountControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdisplayexService, ListdisplayexControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListexService, ListexControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListmetadataService, ListmetadataControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListtempleService, ListtempleControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMetadatafieldsService, MetadatafieldsControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IMetadataService, MetadataControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISqlService, SqlControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IListdataremarkService, ListdataremarkControllerProxy>();

            //Basic.Framework.Ioc.IocManager.RegistObject<INetworkModuleService, NetworkModuleControllerProxy>();

            //曲线模块
            Basic.Framework.Ioc.IocManager.RegistObject<IChartService, ChartControllerProxy>();

            //控制模块
            Basic.Framework.Ioc.IocManager.RegistObject<IControlService, ControlControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionHistoryDataService, StaionHistoryDataControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionControlHistoryDataService, StaionControlHistoryDataControllerProxy>();

            //标校模块
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationDefService, CalibrationDefControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationStatisticsService, CalibrationStatisticsControllerProxy>();

            //实时模块
            Basic.Framework.Ioc.IocManager.RegistObject<IRealMessageService, RealMessageControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationService, CalibrationControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceKoriyasuService, DeviceKoriyasuControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_RService, Jc_RControllerProxy>();

            //报警模块
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmService, AlarmServiceControllerProxy>();

            //测点定义模块
            Basic.Framework.Ioc.IocManager.RegistObject<IPointDefineService, PointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPersonPointDefineService, PersonPointDefineControllerProxy>();//人员定位
            Basic.Framework.Ioc.IocManager.RegistObject<IAllSystemPointDefineService, AllSystemPointDefineControllerProxy>();//所有系统接口
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceDefineService, DeviceDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IManualCrossControlService, ManualCrossControlControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<INetworkModuleService, NetworkModuleControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPositionService, PositionControllerProxy>();

            Basic.Framework.Ioc.IocManager.RegistObject<IAreaService, AreaControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAreaRuleService, AreaRuleControllerProxy>();

            //图形模块
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsbaseinfService, GraphicsbaseinfControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicspointsinfService, GraphicspointsinfControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsrouteinfService, GraphicsrouteinfControllerProxy>();


            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmRecordService, AlarmRecordControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmHandleService, AlarmHandleControllerProxy>();


            #region 大数据分析
            //模板配置
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateConfigService, AnalysisTemplateConfigControllerProxy>();
            //模板
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateService, AnalysisTemplateControllerProxy>();
            //表达式
            Basic.Framework.Ioc.IocManager.RegistObject<IAnalyticalExpressionService, AnalyticalExpressionControllerProxy>();
            //表达式配置
            Basic.Framework.Ioc.IocManager.RegistObject<IExpressionConfigService, ExpressionConfigControllerProxy>();
            //参数表
            Basic.Framework.Ioc.IocManager.RegistObject<IParameterService, ParameterControllerProxy>();
            //因子表
            Basic.Framework.Ioc.IocManager.RegistObject<IFactorService, FactorControllerProxy>();
            //大数据分析配置表
            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisConfigService, LargedataAnalysisConfigControllerProxy>();
            //大数据分析记录表
            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisLogService, LargedataAnalysisLogControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRegionOutageConfigService, RegionOutageConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageConfigService, EmergencyLinkageConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISetAnalysisModelPointRecordService, SetAnalysisModelPointRecordControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelService, AlarmNotificationPersonnelControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelConfigService, AlarmNotificationPersonnelConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisCacheClientService, LargeDataAnalysisCacheClientControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisConfigCacheService, LargeDataAnalysisConfigCacheControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IJc_AnalysistemplatealarmlevelService, AnalysisTemplateAlarmLevelControllerProxy>();

            //倍数报警
            Basic.Framework.Ioc.IocManager.RegistObject<IJC_MultiplesettingService, MultiplesettingControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IRatioAlarmCacheService, RatioAlarmCacheServiceControllerProxy>();
            #endregion

           //快捷菜单
            Basic.Framework.Ioc.IocManager.RegistObject<IShortCutMenuService, ShortCutMenuControllerProxy>();
            //远程升级
            Basic.Framework.Ioc.IocManager.RegistObject<IStationUpdateService, StationUpdateControllerProxy>();

            //人员定位
            Basic.Framework.Ioc.IocManager.RegistObject<IR_DeptService, R_DeptControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PersoninfService, R_PersoninfControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PrealService, R_PrealControllerProxy>();//人员实时接口  20171127
            Basic.Framework.Ioc.IocManager.RegistObject<IR_UndefinedDefService, R_UndefinedDefControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_KqbcService, R_KqbcControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_CallService, R_CallControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhjService, R_PhjControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhistoryService, R_PhistoryControllerProxy>();

            //视频
            Basic.Framework.Ioc.IocManager.RegistObject<IV_DefService, V_DefControllerProxy>();

            //应急联动
            Basic.Framework.Ioc.IocManager.RegistObject<ISysEmergencyLinkageService, SysEmergencyLinkageControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkHistoryService, EmergencyLinkHistoryControllerProxy>();
            
            //广播
            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallService, B_CallControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IBroadCastPointDefineService, BroadCastPointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_MusicfilesService, B_MusicfilesControllerProxy>();

            //电源箱放电历史记录
            Basic.Framework.Ioc.IocManager.RegistObject<IPowerboxchargehistoryService, PowerboxchargehistoryControllerProxy>();

            Basic.Framework.Ioc.IocManager.RegistObject<IGascontentanalyzeconfigService, GascontentanalyzeconfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGasContentService, GasContentControllerProxy>();

            Basic.Framework.Ioc.IocManager.RegistObject<IKJ_AddresstypeService, KJ_AddresstypeControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IKJ_AddresstyperuleService, KJ_AddresstyperuleControllerProxy>();

            Basic.Framework.Ioc.IocManager.Build();
        }
    }
}

using Sys.Safety.ServiceContract.UserRoleAuthorize;
using Sys.Safety.ServiceContract.Chart;
using Sys.Safety.ServiceContract.Control;
using Sys.Safety.ServiceContract.ListReport;
using Sys.Safety.ServiceContract;
using Sys.Safety.WebApiAgent;
using Sys.Safety.WebApiAgent.Chart;
using Sys.Safety.WebApiAgent.Control;
using Sys.Safety.WebApiAgent.RealModule;
using Sys.Safety.WebApiAgent.ListReport;
using Sys.Safety.WebApiAgent.CBFCommon;
using Sys.Safety.WebApiAgent.UserRoleAuthorize;

namespace Sys.Safety.Client.Graphic
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

            //报警模块
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmService, AlarmServiceControllerProxy>();

            //测点定义模块
            Basic.Framework.Ioc.IocManager.RegistObject<IPointDefineService, PointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceDefineService, DeviceDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IManualCrossControlService, ManualCrossControlControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<INetworkModuleService, NetworkModuleControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPositionService, PositionControllerProxy>();

            //图形模块
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsbaseinfService, GraphicsbaseinfControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicspointsinfService, GraphicspointsinfControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsrouteinfService, GraphicsrouteinfControllerProxy>();


            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmRecordService, AlarmRecordControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IAlarmHandleService, AlarmHandleControllerProxy>();

            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisConfigService, LargedataAnalysisConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisCacheClientService, LargeDataAnalysisCacheClientControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PrealService, R_PrealControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IV_DefService, V_DefControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_CallService, R_CallControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhjService, R_PhjControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISysEmergencyLinkageService, SysEmergencyLinkageControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPersonPointDefineService, PersonPointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhistoryService, R_PhistoryControllerProxy>();

            Basic.Framework.Ioc.IocManager.RegistObject<IAllSystemPointDefineService, AllSystemPointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PersoninfService, R_PersoninfControllerProxy>();

            //广播
            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallService, B_CallControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IBroadCastPointDefineService, BroadCastPointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_MusicfilesService, B_MusicfilesControllerProxy>();
            
            Basic.Framework.Ioc.IocManager.Build();
        }
    }
}

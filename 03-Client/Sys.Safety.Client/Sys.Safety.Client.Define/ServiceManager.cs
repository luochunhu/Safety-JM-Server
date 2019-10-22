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

namespace Sys.Safety.Client.Define
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

            //Basic.Framework.Ioc.IocManager.RegistObject<INetworkModuleService, NetworkModuleControllerProxy>();

            //曲线模块
            Basic.Framework.Ioc.IocManager.RegistObject<IChartService, ChartControllerProxy>();

            //控制模块
            Basic.Framework.Ioc.IocManager.RegistObject<IControlService, ControlControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionHistoryDataService, StaionHistoryDataControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IStaionControlHistoryDataService, StaionControlHistoryDataControllerProxy>();
            

            //实时模块
            Basic.Framework.Ioc.IocManager.RegistObject<IRealMessageService, RealMessageControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationService, CalibrationControllerProxy>();

           
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



            //人员定位
            Basic.Framework.Ioc.IocManager.RegistObject<IR_DeptService, R_DeptControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PersoninfService, R_PersoninfControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PrealService, R_PrealControllerProxy>();//人员实时接口  20171127
            Basic.Framework.Ioc.IocManager.RegistObject<IR_UndefinedDefService, R_UndefinedDefControllerProxy>();

            Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisConfigService, LargedataAnalysisConfigControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisCacheClientService, LargeDataAnalysisCacheClientControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ISysEmergencyLinkageService, SysEmergencyLinkageControllerProxy>();

            Basic.Framework.Ioc.IocManager.RegistObject<IV_DefService, V_DefControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_CallService, R_CallControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IR_PhjService, R_PhjControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IBroadCastPointDefineService, BroadCastPointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IB_CallService, B_CallControllerProxy>();

         

            Basic.Framework.Ioc.IocManager.Build();
        }
    }
}

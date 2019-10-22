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

namespace Sys.Safety.Client.Chart
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
                        

            //实时模块
            Basic.Framework.Ioc.IocManager.RegistObject<IRealMessageService, RealMessageControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<ICalibrationService, CalibrationControllerProxy>();

           
            //测点定义模块
            Basic.Framework.Ioc.IocManager.RegistObject<IPointDefineService, PointDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IDeviceDefineService, DeviceDefineControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IManualCrossControlService, ManualCrossControlControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<INetworkModuleService, NetworkModuleControllerProxy>();
            Basic.Framework.Ioc.IocManager.RegistObject<IPositionService, PositionControllerProxy>();

           
            
            Basic.Framework.Ioc.IocManager.Build();
        }
    }
}

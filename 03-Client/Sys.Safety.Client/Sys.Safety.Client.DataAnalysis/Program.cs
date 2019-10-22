using Sys.Safety.ServiceContract;
using Sys.Safety.WebApiAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.DataAnalysis
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            ////创建业务服务对象（走webapi远程调用）
            ////模板配置
            //Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateConfigService, AnalysisTemplateConfigControllerProxy>();

            ////模板
            //Basic.Framework.Ioc.IocManager.RegistObject<IAnalysisTemplateService, AnalysisTemplateControllerProxy>();

            ////表达式
            //Basic.Framework.Ioc.IocManager.RegistObject<IAnalyticalExpressionService, AnalyticalExpressionControllerProxy>();

            ////表达式配置
            //Basic.Framework.Ioc.IocManager.RegistObject<IExpressionConfigService, ExpressionConfigControllerProxy>();
            ////参数表
            //Basic.Framework.Ioc.IocManager.RegistObject<IParameterService, ParameterControllerProxy>();
            ////因子表
            //Basic.Framework.Ioc.IocManager.RegistObject<IFactorService, FactorControllerProxy>();

            //Basic.Framework.Ioc.IocManager.RegistObject<IAlarmHandleService, AlarmHandleControllerProxy>();

            ////大数据分析配置表
            //Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisConfigService, LargedataAnalysisConfigControllerProxy>();
            ////大数据分析记录表
            //Basic.Framework.Ioc.IocManager.RegistObject<ILargedataAnalysisLogService, LargedataAnalysisLogControllerProxy>();
            ////
            //Basic.Framework.Ioc.IocManager.RegistObject<IRegionOutageConfigService, RegionOutageConfigControllerProxy>();

            //Basic.Framework.Ioc.IocManager.RegistObject<IEmergencyLinkageConfigService, EmergencyLinkageConfigControllerProxy>();

            //Basic.Framework.Ioc.IocManager.RegistObject<ISetAnalysisModelPointRecordService, SetAnalysisModelPointRecordControllerProxy>();

            //Basic.Framework.Ioc.IocManager.RegistObject<IPointDefineService, PointDefineControllerProxy>();

            //Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelService, AlarmNotificationPersonnelControllerProxy>();

            //Basic.Framework.Ioc.IocManager.RegistObject<IAlarmNotificationPersonnelConfigService, AlarmNotificationPersonnelConfigControllerProxy>();

            //Basic.Framework.Ioc.IocManager.RegistObject<ILargeDataAnalysisCacheClientService, LargeDataAnalysisCacheClientControllerProxy>();

            ////图形模块
            //Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsbaseinfService, GraphicsbaseinfControllerProxy>();
            //Basic.Framework.Ioc.IocManager.RegistObject<IGraphicspointsinfService, GraphicspointsinfControllerProxy>();
            //Basic.Framework.Ioc.IocManager.RegistObject<IGraphicsrouteinfService, GraphicsrouteinfControllerProxy>();
            //Basic.Framework.Ioc.IocManager.Build();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BigDataAnalysisForm());
        }
    }
}

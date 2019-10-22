
using Basic.Framework.Data;
using Sys.Safety.ServiceContract.App;
using Sys.Safety.WebApiAgent.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Mas.KJ73N.AppApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            try 
            {
                RegisterService();
                InitConfig();
            }
            catch (Exception ex)
            {

            }
        }

        private void RegisterService()
        {
            Basic.Framework.Ioc.IocManager.RegistObject<IKJ73NAppService, KJ73NAppControllerProxy>();
            Basic.Framework.Ioc.IocManager.Build();
        }

        private void InitConfig()
        {
            //获取设置，设置到客户端全局缓存     
            string ip = System.Configuration.ConfigurationManager.AppSettings["ServerIp"];
            string port = System.Configuration.ConfigurationManager.AppSettings["ServerPort"];
            string url = string.Format("http://{0}:{1}", ip, port);

            if (PlatRuntime.Items.ContainsKey("ServerUrl"))
            {
                PlatRuntime.Items["ServerUrl"] = url;
            }
            else
            {
                PlatRuntime.Items.Add("ServerUrl", url);
            }
        }
    }
}

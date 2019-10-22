using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    /// <summary>
    /// 代理基类
    /// </summary>
    public class BaseProxy
    {
        protected string Token = "token";

        /// <summary>
        /// 获取API URL
        /// </summary>
        protected string Webapi
        {
            get
            {
                return GetApiUrl();
            }
        }
        
        private string GetApiUrl()
        {
            string url = "";
            ////根据配置走配置动态构建WebApi服务端的地址及端口号(URL)
            //if (Framework.Data.PlatRuntime.Items.ContainsKey("ServerUrl"))
            //{
            //    url = Framework.Data.PlatRuntime.Items["ServerUrl"].ToString();
            //}
            string ip = System.Configuration.ConfigurationManager.AppSettings["ServerIp"];
            string port = System.Configuration.ConfigurationManager.AppSettings["ServerPort"];
            url = string.Format("http://{0}:{1}", ip, port); 

            

            return url;

            //webapi = Basic.Framework.Configuration.ConfigurationManager.GetValue<string>("WebApiServer");
            //token = Basic.Framework.Web.SessionManager.Session.GetSession<string>(string.Format("token_{0}", Basic.Framework.Web.SessionManager.UserId));
        }
    }
}

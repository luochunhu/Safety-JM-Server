using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract.Control;

namespace Sys.Safety.WebApiAgent.Control
{
    public class ControlControllerProxy : BaseProxy, IControlService
    {
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetDyxFz()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Control/GetDyxFz?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetDyxMac()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Control/GetDyxMac?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }
    }
}

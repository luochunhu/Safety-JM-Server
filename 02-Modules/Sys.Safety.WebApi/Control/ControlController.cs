using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract.Control;

namespace Sys.Safety.WebApi.Control
{
    public class ControlController : Basic.Framework.Web.WebApi.BasicApiController, IControlService
    {
        IControlService _controlService = ServiceFactory.Create<IControlService>();

        [HttpPost]
        [Route("v1/Control/GetDyxFz")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetDyxFz()
        {
            return _controlService.GetDyxFz();
        }

        [HttpPost]
        [Route("v1/Control/GetDyxMac")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetDyxMac()
        {
            return _controlService.GetDyxMac();
        }
    }
}

using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request;
using System.Data;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.RemoteState;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 网关、远程机状态管理WebApi接口
    /// </summary>
    public class RemoteStateController : Basic.Framework.Web.WebApi.BasicApiController, IRemoteStateService
    {
        static RemoteStateController()
        {

        }
        IRemoteStateService _remotestateService = ServiceFactory.Create<IRemoteStateService>();
        [HttpPost]
        [Route("v1/RemoteState/GetGatewayState")]
        public BasicResponse<bool> GetGatewayState()
        {
            return _remotestateService.GetGatewayState();
        }
        [HttpPost]
        [Route("v1/RemoteState/SetGatewayState")]
        public BasicResponse SetGatewayState(RemoteStateRequest staterequest)
        {
            return _remotestateService.SetGatewayState(staterequest);
        }
        [HttpPost]
        [Route("v1/RemoteState/GetRemoteState")]
        public BasicResponse<bool> GetRemoteState()
        {
            return _remotestateService.GetRemoteState();
        }
        [HttpPost]
        [Route("v1/RemoteState/SetRemoteState")]
        public BasicResponse SetRemoteState(RemoteStateRequest staterequest)
        {
            return _remotestateService.SetRemoteState(staterequest);
        }

        [HttpPost]
        [Route("v1/RemoteState/GetLastReciveTime")]
        public BasicResponse<DateTime> GetLastReciveTime()
        {
            return _remotestateService.GetLastReciveTime();
        }
        [HttpPost]
        [Route("v1/RemoteState/SetLastReciveTime")]
        public BasicResponse SetLastReciveTime(RemoteStateRequest staterequest)
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        [Route("v1/RemoteState/UpdateInspectionTime")]
        public BasicResponse UpdateInspectionTime(UpdateInspectionTimeRequest request)
        {
            return _remotestateService.UpdateInspectionTime(request);
        }
        [HttpPost]
        [Route("v1/RemoteState/GetInspectionTime")]
        public BasicResponse<long> GetInspectionTime(GetInspectionTimeRequest request)
        {
            return _remotestateService.GetInspectionTime(request);
        }
    }
}

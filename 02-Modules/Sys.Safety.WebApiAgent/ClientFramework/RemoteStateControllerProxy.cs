using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.RemoteState;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.CBFCommon
{
    public class RemoteStateControllerProxy : BaseProxy, IRemoteStateService
    {
        public BasicResponse<bool> GetGatewayState()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RemoteState/GetGatewayState?token=" + Token, string.Empty, 5);
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }
        public BasicResponse SetGatewayState(RemoteStateRequest staterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RemoteState/SetGatewayState?token=" + Token, JSONHelper.ToJSONString(staterequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<bool> GetRemoteState()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RemoteState/GetRemoteState?token=" + Token, string.Empty, 5);
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }
        public BasicResponse SetRemoteState(RemoteStateRequest staterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RemoteState/SetRemoteState?token=" + Token, JSONHelper.ToJSONString(staterequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<DateTime> GetLastReciveTime()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RemoteState/GetLastReciveTime?token=" + Token, string.Empty, 5);
            return JSONHelper.ParseJSONString<BasicResponse<DateTime>>(responseStr);
        }

        public BasicResponse SetLastReciveTime(RemoteStateRequest staterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RemoteState/SetLastReciveTime?token=" + Token, JSONHelper.ToJSONString(staterequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse UpdateInspectionTime(UpdateInspectionTimeRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RemoteState/UpdateInspectionTime?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<long> GetInspectionTime(GetInspectionTimeRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RemoteState/GetInspectionTime?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<long>>(responseStr);
        }
    }
}

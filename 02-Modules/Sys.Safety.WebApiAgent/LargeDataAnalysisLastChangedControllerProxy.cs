using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.Request;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class LargeDataAnalysisLastChangedControllerProxy : BaseProxy, ILargeDataAnalysisLastChangedService
    {
        public BasicResponse<string> GetAlarmNotificationLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisLastChanged/GetAlarmNotificationLastChangedTime?token=" + Token, JSONHelper.ToJSONString(lastChangedRequest));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
        }

        public BasicResponse<string> GetAnalysisModelLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisLastChanged/GetAnalysisModelLastChangedTime?token=" + Token, JSONHelper.ToJSONString(lastChangedRequest));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
        }

        public BasicResponse<string> GetEmergencyLinkageLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisLastChanged/GetEmergencyLinkageLastChangedTime?token=" + Token, JSONHelper.ToJSONString(lastChangedRequest));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
        }

        public BasicResponse<string> GetPointDefineLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisLastChanged/GetPointDefineLastChangedTime?token=" + Token, JSONHelper.ToJSONString(lastChangedRequest));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
        }

        public BasicResponse<string> GetRegionOutageLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisLastChanged/GetRegionOutageLastChangedTime?token=" + Token, JSONHelper.ToJSONString(lastChangedRequest));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
        }
    }
}

using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.Request;
using Basic.Framework.Service;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class LargeDataAnalysisLastChangedController : Basic.Framework.Web.WebApi.BasicApiController, ILargeDataAnalysisLastChangedService
    {

        ILargeDataAnalysisLastChangedService largeDataAnalysisLastChangedService = ServiceFactory.Create<ILargeDataAnalysisLastChangedService>();

        [HttpPost]
        [Route("v1/LargeDataAnalysisLastChanged/GetAlarmNotificationLastChangedTime")]
        public BasicResponse<string> GetAlarmNotificationLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            return largeDataAnalysisLastChangedService.GetAlarmNotificationLastChangedTime(lastChangedRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisLastChanged/GetAnalysisModelLastChangedTime")]
        public BasicResponse<string> GetAnalysisModelLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            return largeDataAnalysisLastChangedService.GetAnalysisModelLastChangedTime(lastChangedRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisLastChanged/GetEmergencyLinkageLastChangedTime")]
        public BasicResponse<string> GetEmergencyLinkageLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            return largeDataAnalysisLastChangedService.GetEmergencyLinkageLastChangedTime(lastChangedRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisLastChanged/GetPointDefineLastChangedTime")]
        public BasicResponse<string> GetPointDefineLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            return largeDataAnalysisLastChangedService.GetPointDefineLastChangedTime(lastChangedRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisLastChanged/GetRegionOutageLastChangedTime")]
        public BasicResponse<string> GetRegionOutageLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            return largeDataAnalysisLastChangedService.GetRegionOutageLastChangedTime(lastChangedRequest);
        }
    }
}

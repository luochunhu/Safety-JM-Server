using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_Hour;
using Basic.Framework.Service;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class HourServiceController : Basic.Framework.Web.WebApi.BasicApiController, IJc_HourService
    {

        private IJc_HourService hourService = ServiceFactory.Create<IJc_HourService>();

        [HttpPost]
        [Route("v1/HourService/GetDataAnalysisHistoryData")]
        public BasicResponse<List<DataAnalysisHistoryDataInfo>> GetDataAnalysisHistoryData(BasicRequest request)
        {
            return hourService.GetDataAnalysisHistoryData(request);
        }

        [HttpPost]
        [Route("v1/HourService/GetWeekAverageValueByPointId")]
        public BasicResponse<Jc_HourInfo> GetWeekAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            return hourService.GetWeekAverageValueByPointId(jc_Hourrequest);
        }

        [HttpPost]
        [Route("v1/HourService/AddJc_Hour")]
        BasicResponse<Jc_HourInfo> IJc_HourService.AddJc_Hour(Jc_HourAddRequest jc_Hourrequest)
        {
            return hourService.AddJc_Hour(jc_Hourrequest);
        }

        [HttpPost]
        [Route("v1/HourService/DeleteJc_Hour")]
        BasicResponse IJc_HourService.DeleteJc_Hour(Jc_HourDeleteRequest jc_Hourrequest)
        {
            return hourService.DeleteJc_Hour(jc_Hourrequest);
        }

        [HttpPost]
        [Route("v1/HourService/GetDayAverageValueByPointId")]
        BasicResponse<Jc_HourInfo> IJc_HourService.GetDayAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            return hourService.GetDayAverageValueByPointId(jc_Hourrequest);
        }

        [HttpPost]
        [Route("v1/HourService/GetDayMaxValueByPointId")]
        BasicResponse<Jc_HourInfo> IJc_HourService.GetDayMaxValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            return hourService.GetDayMaxValueByPointId(jc_Hourrequest);
        }

        [HttpPost]
        [Route("v1/HourService/GetJc_HourById")]
        BasicResponse<Jc_HourInfo> IJc_HourService.GetJc_HourById(Jc_HourGetRequest jc_Hourrequest)
        {
            return hourService.GetJc_HourById(jc_Hourrequest);
        }

        [HttpPost]
        [Route("v1/HourService/GetJc_HourList")]
        BasicResponse<List<Jc_HourInfo>> IJc_HourService.GetJc_HourList(Jc_HourGetListRequest jc_Hourrequest)
        {
            return hourService.GetJc_HourList(jc_Hourrequest);
        }

        [HttpPost]
        [Route("v1/HourService/GetMonthAverageValueByPointId")]
        BasicResponse<Jc_HourInfo> IJc_HourService.GetMonthAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            return hourService.GetMonthAverageValueByPointId(jc_Hourrequest);
        }

        [HttpPost]
        [Route("v1/HourService/UpdateJc_Hour")]
        BasicResponse<Jc_HourInfo> IJc_HourService.UpdateJc_Hour(Jc_HourUpdateRequest jc_Hourrequest)
        {
            return hourService.UpdateJc_Hour(jc_Hourrequest);
        }
    }
}

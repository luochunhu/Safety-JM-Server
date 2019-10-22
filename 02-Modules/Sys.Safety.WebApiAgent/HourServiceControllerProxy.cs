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
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class HourServiceControllerProxy : BaseProxy, IJc_HourService
    {
        public BasicResponse<Jc_HourInfo> AddJc_Hour(Jc_HourAddRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/AddJc_Hour?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_HourInfo>>(responsestr);
        }

        public BasicResponse DeleteJc_Hour(Jc_HourDeleteRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/DeleteJc_Hour?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public BasicResponse<List<DataAnalysisHistoryDataInfo>> GetDataAnalysisHistoryData(BasicRequest request)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/GetDataAnalysisHistoryData?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataAnalysisHistoryDataInfo>>>(responsestr);
        }

        public BasicResponse<Jc_HourInfo> GetDayAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/GetDayAverageValueByPointId?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_HourInfo>>(responsestr);
        }

        public BasicResponse<Jc_HourInfo> GetDayMaxValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/GetDayMaxValueByPointId?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_HourInfo>>(responsestr);
        }

        public BasicResponse<Jc_HourInfo> GetJc_HourById(Jc_HourGetRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/GetJc_HourById?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_HourInfo>>(responsestr);
        }

        public BasicResponse<List<Jc_HourInfo>> GetJc_HourList(Jc_HourGetListRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/GetJc_HourList?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_HourInfo>>>(responsestr);
        }

        public BasicResponse<Jc_HourInfo> GetMonthAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/GetMonthAverageValueByPointId?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_HourInfo>>(responsestr);
        }

        public BasicResponse<Jc_HourInfo> GetWeekAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/GetWeekAverageValueByPointId?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_HourInfo>>(responsestr);
        }

        public BasicResponse<Jc_HourInfo> UpdateJc_Hour(Jc_HourUpdateRequest jc_Hourrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/HourService/UpdateJc_Hour?token=" + Token, JSONHelper.ToJSONString(jc_Hourrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_HourInfo>>(responsestr);
        }
    }
}

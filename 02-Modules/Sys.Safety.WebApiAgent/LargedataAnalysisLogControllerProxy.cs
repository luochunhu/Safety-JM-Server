using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Request.JC_Largedataanalysislog;

namespace Sys.Safety.WebApiAgent
{
    public class LargedataAnalysisLogControllerProxy : BaseProxy, ILargedataAnalysisLogService
    {

        public Basic.Framework.Web.BasicResponse<DataContract.JC_LargedataAnalysisLogInfo> AddJC_Largedataanalysislog(Sys.Safety.Request.JC_Largedataanalysislog.LargedataAnalysisLogAddRequest jC_Largedataanalysislogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisLog/AddJC_Largedataanalysislog?token=" + Token, JSONHelper.ToJSONString(jC_Largedataanalysislogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_LargedataAnalysisLogInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_LargedataAnalysisLogInfo> UpdateJC_Largedataanalysislog(Sys.Safety.Request.JC_Largedataanalysislog.LargedataAnalysisLogUpdateRequest jC_Largedataanalysislogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisLog/UpdateJC_Largedataanalysislog?token=" + Token, JSONHelper.ToJSONString(jC_Largedataanalysislogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_LargedataAnalysisLogInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteJC_Largedataanalysislog(Sys.Safety.Request.JC_Largedataanalysislog.LargedataanalysislogDeleteRequest jC_Largedataanalysislogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisLog/DeleteJC_Largedataanalysislog?token=" + Token, JSONHelper.ToJSONString(jC_Largedataanalysislogrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_LargedataAnalysisLogInfo>> GetJC_LargedataanalysislogList(Sys.Safety.Request.JC_Largedataanalysislog.LargedataAnalysisLogGetListRequest jC_Largedataanalysislogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisLog/GetJC_LargedataanalysislogList?token=" + Token, JSONHelper.ToJSONString(jC_Largedataanalysislogrequest));
            return JSONHelper.ParseJSONString<Basic.Framework.Web.BasicResponse<List<DataContract.JC_LargedataAnalysisLogInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_LargedataAnalysisLogInfo> GetJC_LargedataanalysislogById(Sys.Safety.Request.JC_Largedataanalysislog.LargedataAnalysisLogGetRequest jC_Largedataanalysislogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisLog/GetJC_LargedataanalysislogById?token=" + Token, JSONHelper.ToJSONString(jC_Largedataanalysislogrequest));
            return JSONHelper.ParseJSONString<Basic.Framework.Web.BasicResponse<JC_LargedataAnalysisLogInfo>>(responseStr);
        }

        public BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetLargedataAnalysisLogListByAnalysisModelId(LargedataAnalysisLogGetByAnalysisModelIdRequest largedataAnalysisLogGetByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisLog/GetLargedataAnalysisLogListByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(largedataAnalysisLogGetByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisLogInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetLargedataAnalysisLogListByModelIdAndTime(LargedataAnalysisLogGetListByModelIdAndTimeRequest largedataAnalysisLogGetListByModelIdAndTimeRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisLog/GetLargedataAnalysisLogListByModelIdAndTime?token=" + Token, JSONHelper.ToJSONString(largedataAnalysisLogGetListByModelIdAndTimeRequest));
            return JSONHelper.ParseJSONString<Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisLogInfo>>>(responseStr);
        }

        public BasicResponse<JC_LargedataAnalysisLogInfo> GetLargedataAnalysisLogLatestByAnalysisModelId(LargedataAnalysisLogGetByAnalysisModelIdRequest largedataAnalysisLogGetByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisLog/GetLargedataAnalysisLogLatestByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(largedataAnalysisLogGetByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<Basic.Framework.Web.BasicResponse<JC_LargedataAnalysisLogInfo>>(responseStr);
        }
    }
}

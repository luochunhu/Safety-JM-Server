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
using Sys.Safety.Request.EmergencyLinkHistory;

namespace Sys.Safety.WebApiAgent
{
    public class EmergencyLinkHistoryControllerProxy : BaseProxy,IEmergencyLinkHistoryService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.EmergencyLinkHistoryInfo> AddEmergencyLinkHistory(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryAddRequest emergencyLinkHistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/AddEmergencyLinkHistory?token=" + Token, JSONHelper.ToJSONString(emergencyLinkHistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<EmergencyLinkHistoryInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.EmergencyLinkHistoryInfo> UpdateEmergencyLinkHistory(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryUpdateRequest emergencyLinkHistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/UpdateEmergencyLinkHistory?token=" + Token, JSONHelper.ToJSONString(emergencyLinkHistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<EmergencyLinkHistoryInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteEmergencyLinkHistory(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryDeleteRequest emergencyLinkHistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/DeleteEmergencyLinkHistory?token=" + Token, JSONHelper.ToJSONString(emergencyLinkHistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.EmergencyLinkHistoryInfo>> GetEmergencyLinkHistoryList(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryGetListRequest emergencyLinkHistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/GetEmergencyLinkHistoryList?token=" + Token, JSONHelper.ToJSONString(emergencyLinkHistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<EmergencyLinkHistoryInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.EmergencyLinkHistoryInfo> GetEmergencyLinkHistoryById(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryGetRequest emergencyLinkHistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/GetEmergencyLinkHistoryById?token=" + Token, JSONHelper.ToJSONString(emergencyLinkHistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<EmergencyLinkHistoryInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.EmergencyLinkHistoryInfo> GetEmergencyLinkHistoryByEmergency(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryGetByEmergencyRequest emergencyLinkHistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/GetEmergencyLinkHistoryByEmergency?token=" + Token, JSONHelper.ToJSONString(emergencyLinkHistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<EmergencyLinkHistoryInfo>>(responseStr);
        }

        public BasicResponse BatchAddEmergencyLinkHistory(BatchAddEmergencyLinkHistoryRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/BatchAddEmergencyLinkHistory?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse EndAll(EndAllRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/EndAll?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse EndByLinkageId(EndByLinkageIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/EndByLinkageId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<IList<EmergencyLinkageHistoryMasterPointAssInfo>> GetNotEndLastLinkageHistoryMasterPointByLinkageId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/GetNotEndLastLinkageHistoryMasterPointByLinkageId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<IList<EmergencyLinkageHistoryMasterPointAssInfo>>>(responseStr);
        }

        public BasicResponse<IList<SysEmergencyLinkageInfo>> GetDeleteButNotEndLinkageIds()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/GetDeleteButNotEndLinkageIds?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<IList<SysEmergencyLinkageInfo>>>(responseStr);
        }

        public BasicResponse AddEmergencyLinkHistoryAndAss(AddEmergencyLinkHistoryAndAssRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkHistory/AddEmergencyLinkHistoryAndAss?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}

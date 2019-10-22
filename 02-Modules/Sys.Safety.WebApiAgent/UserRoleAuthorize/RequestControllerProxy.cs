using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.UserRoleAuthorize
{
    public class RequestControllerProxy : BaseProxy, IRequestService
    {
        
        public BasicResponse<RequestInfo> AddRequest(RequestAddRequest requestrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/Add?token=" + Token, JSONHelper.ToJSONString(requestrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RequestInfo>>(responseStr);
        }        
        public BasicResponse<RequestInfo> UpdateRequest(RequestUpdateRequest requestrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/Update?token=" + Token, JSONHelper.ToJSONString(requestrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RequestInfo>>(responseStr);
        }        
        public BasicResponse DeleteRequest(RequestDeleteRequest requestrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/Delete?token=" + Token, JSONHelper.ToJSONString(requestrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }        
        public BasicResponse<RequestInfo> GetRequestById(RequestGetRequest requestrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/Get?token=" + Token, JSONHelper.ToJSONString(requestrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RequestInfo>>(responseStr);
        }        
        public BasicResponse<List<RequestInfo>> GetRequestList(RequestGetListRequest requestrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/GetPageList?token=" + Token, JSONHelper.ToJSONString(requestrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RequestInfo>>>(responseStr);
        }       
        public BasicResponse<List<RequestInfo>> GetRequestList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<RequestInfo>>>(responseStr);
        }        
        public BasicResponse<RequestInfo> GetRequestByCode(RequestGetByCodeRequest requestrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/GetByCode?token=" + Token, JSONHelper.ToJSONString(requestrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RequestInfo>>(responseStr);
        }        
        public BasicResponse<DataTable> GetRequestMenuByCode(RequestGetMenuByCodeRequest requestrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/GetRequestMenuByCode?token=" + Token, JSONHelper.ToJSONString(requestrequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }        
        public BasicResponse<RequestInfo> AddRequestEx(RequestAddRequest requestrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Request/AddRequestEx?token=" + Token, JSONHelper.ToJSONString(requestrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RequestInfo>>(responseStr);
        }
    }
}

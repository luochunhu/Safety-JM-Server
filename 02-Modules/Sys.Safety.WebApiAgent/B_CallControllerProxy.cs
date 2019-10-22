using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Call;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class B_CallControllerProxy : BaseProxy, IB_CallService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.B_CallInfo> AddCall(Sys.Safety.Request.R_Call.B_CallAddRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/AddCall?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<B_CallInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.B_CallInfo> UpdateCall(Sys.Safety.Request.R_Call.B_CallUpdateRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/UpdateCall?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<B_CallInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteCall(Sys.Safety.Request.R_Call.B_CallDeleteRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/DeleteCall?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<B_CallInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.B_CallInfo>> GetCallList(Sys.Safety.Request.R_Call.B_CallGetListRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/GetCallList?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<B_CallInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.B_CallInfo> GetCallById(Sys.Safety.Request.R_Call.B_CallGetRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/GetCallById?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<B_CallInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.B_CallInfo>> GetAll(Basic.Framework.Web.BasicRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/GetAll?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<B_CallInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.B_CallInfo>> GetBCallInfoByMasterID(Sys.Safety.Request.R_Call.BCallInfoGetByMasterIDRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/GetBCallInfoByMasterID?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<B_CallInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse EndBcallByBcallInfoList(Sys.Safety.Request.R_Call.EndBcallByBcallInfoListRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/EndBcallByBcallInfoList?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<B_CallInfo>> GetAllCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/GetAllCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<B_CallInfo>>>(responseStr);
        }

        public BasicResponse DeleteFinishedBcall()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/DeleteFinishedBcall?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse EndBcallDbByBcallInfoList(EndBcallDbByBcallInfoListRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/EndBcallDbByBcallInfoList?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<List<B_CallInfo>> GetFusionCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Call/GetFusionCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<B_CallInfo>>>(responseStr);
        }
    }
}

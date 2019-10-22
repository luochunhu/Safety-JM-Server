using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class R_CallControllerProxy : BaseProxy, IR_CallService
    {
        public BasicResponse<R_CallInfo> AddCall(Sys.Safety.Request.R_Call.R_CallAddRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/AddCall?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_CallInfo>>(responseStr);
        }

        public BasicResponse<R_CallInfo> UpdateCall(Sys.Safety.Request.R_Call.R_CallUpdateRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/UpdateCall?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_CallInfo>>(responseStr);
        }

        public BasicResponse DeleteCall(Sys.Safety.Request.R_Call.R_CallDeleteRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/DeleteCall?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_CallInfo>>(responseStr);
        }

        public BasicResponse<List<R_CallInfo>> GetCallList(Sys.Safety.Request.R_Call.R_CallGetListRequest callRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_CallInfo> GetCallById(Sys.Safety.Request.R_Call.R_CallGetRequest callRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<R_CallInfo>> GetAllCall()
        {
            throw new NotImplementedException();
        }

        public BasicResponse BachUpdateAlarmInfoProperties(Sys.Safety.Request.R_Call.R_CallUpdateProperitesRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/BachUpdateAlarmInfoProperties?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<R_CallInfo>> GetAllRCallCache(RCallCacheGetAllRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/GetAllRCallCache?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_CallInfo>>>(responseStr);
        }

        public BasicResponse<R_CallInfo> GetByKeyRCallCache(RCallCacheGetByKeyRequest callRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/GetByKeyRCallCache?token=" + Token, JSONHelper.ToJSONString(callRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_CallInfo>>(responseStr);
        }


        public BasicResponse<List<R_CallInfo>> GetRCallInfoByMasterID(RCallInfoGetByMasterIDRequest RCallCacheRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/GetRCallInfoByMasterID?token=" + Token, JSONHelper.ToJSONString(RCallCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_CallInfo>>>(responseStr);
        }
        
        public BasicResponse EndRcallByRcallInfoList(EndRcallByRcallInfoListEequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/EndRcallByRcallInfoList?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_CallInfo>>>(responseStr);
        }

        public BasicResponse EndRcallDbByRcallInfoList(EndRcallDbByRcallInfoListEequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Call/EndRcallDbByRcallInfoList?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_CallInfo>>>(responseStr);
        }


        public BasicResponse DeleteFinishedBcall()
        {
            throw new NotImplementedException();
        }
    }
}

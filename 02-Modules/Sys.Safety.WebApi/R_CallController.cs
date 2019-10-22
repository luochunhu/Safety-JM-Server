using Basic.Framework.Service;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class R_CallController : BasicApiController, IR_CallService
    {
        IR_CallService _R_CallService = ServiceFactory.Create<IR_CallService>();


        [HttpPost]
        [Route("v1/R_Call/AddCall")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_CallInfo> AddCall(Sys.Safety.Request.R_Call.R_CallAddRequest callRequest)
        {
            return _R_CallService.AddCall(callRequest);
        }
        [HttpPost]
        [Route("v1/R_Call/UpdateCall")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_CallInfo> UpdateCall(Sys.Safety.Request.R_Call.R_CallUpdateRequest callRequest)
        {
            return _R_CallService.UpdateCall(callRequest);
        }
        [HttpPost]
        [Route("v1/R_Call/DeleteCall")]
        public Basic.Framework.Web.BasicResponse DeleteCall(Sys.Safety.Request.R_Call.R_CallDeleteRequest callRequest)
        {
            return _R_CallService.DeleteCall(callRequest);
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_CallInfo>> GetCallList(Sys.Safety.Request.R_Call.R_CallGetListRequest callRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_CallInfo> GetCallById(Sys.Safety.Request.R_Call.R_CallGetRequest callRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_CallInfo>> GetAllCall()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("v1/R_Call/GetAllRCallCache")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_CallInfo>> GetAllRCallCache(RCallCacheGetAllRequest callRequest)
        {
            return _R_CallService.GetAllRCallCache(callRequest);
        }

        [HttpPost]
        [Route("v1/R_Call/BachUpdateAlarmInfoProperties")]
        public Basic.Framework.Web.BasicResponse BachUpdateAlarmInfoProperties(Sys.Safety.Request.R_Call.R_CallUpdateProperitesRequest callRequest)
        {
            return _R_CallService.BachUpdateAlarmInfoProperties(callRequest);
        }
        [HttpPost]
        [Route("v1/R_Call/GetByKeyRCallCache")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_CallInfo> GetByKeyRCallCache(RCallCacheGetByKeyRequest callRequest)
        {
            return _R_CallService.GetByKeyRCallCache(callRequest);
        }

        [HttpPost]
        [Route("v1/R_Call/GetRCallInfoByMasterID")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_CallInfo>> GetRCallInfoByMasterID(RCallInfoGetByMasterIDRequest RCallCacheRequest)
        {
            return _R_CallService.GetRCallInfoByMasterID(RCallCacheRequest);
        }

        [HttpPost]
        [Route("v1/R_Call/EndRcallByRcallInfoList")]
        public Basic.Framework.Web.BasicResponse EndRcallByRcallInfoList(EndRcallByRcallInfoListEequest request)
        {
            return _R_CallService.EndRcallByRcallInfoList(request);
        }

        [HttpPost]
        [Route("v1/R_Call/EndRcallDbByRcallInfoList")]
        public BasicResponse EndRcallDbByRcallInfoList(EndRcallDbByRcallInfoListEequest request)
        {
            return _R_CallService.EndRcallDbByRcallInfoList(request);
        }


        public BasicResponse DeleteFinishedBcall()
        {
            throw new NotImplementedException();
        }
    }
}

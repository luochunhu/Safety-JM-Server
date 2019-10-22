using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Call;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class B_CallController : Basic.Framework.Web.WebApi.BasicApiController,IB_CallService
    {
        IB_CallService _bcallService = ServiceFactory.Create<IB_CallService>();

        [HttpPost]
        [Route("v1/B_Call/AddCall")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.B_CallInfo> AddCall(Sys.Safety.Request.R_Call.B_CallAddRequest callRequest)
        {
            return _bcallService.AddCall(callRequest);
        }

        [HttpPost]
        [Route("v1/B_Call/UpdateCall")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.B_CallInfo> UpdateCall(Sys.Safety.Request.R_Call.B_CallUpdateRequest callRequest)
        {
            return _bcallService.UpdateCall(callRequest);
        }

        [HttpPost]
        [Route("v1/B_Call/DeleteCall")]
        public Basic.Framework.Web.BasicResponse DeleteCall(Sys.Safety.Request.R_Call.B_CallDeleteRequest callRequest)
        {
            return _bcallService.DeleteCall(callRequest);
        }

        [HttpPost]
        [Route("v1/B_Call/GetCallList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.B_CallInfo>> GetCallList(Sys.Safety.Request.R_Call.B_CallGetListRequest callRequest)
        {
            return _bcallService.GetCallList(callRequest);
        }

        [HttpPost]
        [Route("v1/B_Call/GetCallById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.B_CallInfo> GetCallById(Sys.Safety.Request.R_Call.B_CallGetRequest callRequest)
        {
            return _bcallService.GetCallById(callRequest);
        }

        [HttpPost]
        [Route("v1/B_Call/GetAll")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.B_CallInfo>> GetAll(Basic.Framework.Web.BasicRequest callRequest)
        {
            return _bcallService.GetAll(callRequest);
        }

        [HttpPost]
        [Route("v1/B_Call/GetBCallInfoByMasterID")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.B_CallInfo>> GetBCallInfoByMasterID(Sys.Safety.Request.R_Call.BCallInfoGetByMasterIDRequest callRequest)
        {
            return _bcallService.GetBCallInfoByMasterID(callRequest);
        }

        [HttpPost]
        [Route("v1/B_Call/EndBcallByBcallInfoList")]
        public Basic.Framework.Web.BasicResponse EndBcallByBcallInfoList(Sys.Safety.Request.R_Call.EndBcallByBcallInfoListRequest request)
        {
            return _bcallService.EndBcallByBcallInfoList(request);
        }

        [HttpPost]
        [Route("v1/B_Call/GetAllCache")]
        public BasicResponse<List<B_CallInfo>> GetAllCache()
        {
            return _bcallService.GetAllCache();
        }

        [HttpPost]
        [Route("v1/B_Call/DeleteFinishedBcall")]
        public BasicResponse DeleteFinishedBcall()
        {
            return _bcallService.DeleteFinishedBcall();
        }

        [HttpPost]
        [Route("v1/B_Call/EndBcallDbByBcallInfoList")]
        public BasicResponse EndBcallDbByBcallInfoList(EndBcallDbByBcallInfoListRequest request)
        {
            return _bcallService.EndBcallDbByBcallInfoList(request);
        }

        [HttpPost]
        [Route("v1/B_Call/GetFusionCache")]
        public BasicResponse<List<B_CallInfo>> GetFusionCache()
        {
            return _bcallService.GetFusionCache();
        }
    }
}

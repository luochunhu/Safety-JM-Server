using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request;
using System.Data;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 请求库管理Api接口
    /// </summary>
    public class RequestController : Basic.Framework.Web.WebApi.BasicApiController, IRequestService
    {
        static RequestController()
        {

        }
        IRequestService _requestService = ServiceFactory.Create<IRequestService>();
        [HttpPost]
        [Route("v1/Request/Add")]
        public BasicResponse<RequestInfo> AddRequest(RequestAddRequest requestrequest)
        {
            return _requestService.AddRequest(requestrequest);
        }
        [HttpPost]
        [Route("v1/Request/Update")]
        public BasicResponse<RequestInfo> UpdateRequest(RequestUpdateRequest requestrequest)
        {
            return _requestService.UpdateRequest(requestrequest);
        }
        [HttpPost]
        [Route("v1/Request/Delete")]
        public BasicResponse DeleteRequest(RequestDeleteRequest requestrequest)
        {
            return _requestService.DeleteRequest(requestrequest);
        }

        [HttpPost]
        [Route("v1/Request/Get")]
        public BasicResponse<RequestInfo> GetRequestById(RequestGetRequest requestrequest)
        {
            return _requestService.GetRequestById(requestrequest);            
        }
        [HttpPost]
        [Route("v1/Request/GetPageList")]
        public BasicResponse<List<RequestInfo>> GetRequestList(RequestGetListRequest requestrequest)
        {
            return _requestService.GetRequestList(requestrequest);
        }
        [HttpPost]
        [Route("v1/Request/GetAllList")]
        public BasicResponse<List<RequestInfo>> GetRequestList()
        {
            return _requestService.GetRequestList();
        }
        [HttpPost]
        [Route("v1/Request/GetByCode")]
        public BasicResponse<RequestInfo> GetRequestByCode(RequestGetByCodeRequest requestrequest)
        {
            return _requestService.GetRequestByCode(requestrequest);
        }
        [HttpPost]
        [Route("v1/Request/GetRequestMenuByCode")]
        public BasicResponse<DataTable> GetRequestMenuByCode(RequestGetMenuByCodeRequest requestrequest)
        {
            return _requestService.GetRequestMenuByCode(requestrequest);
        }
        [HttpPost]
        [Route("v1/Request/AddRequestEx")]
        public BasicResponse<RequestInfo> AddRequestEx(RequestAddRequest requestrequest)
        {
            return _requestService.AddRequestEx(requestrequest);
        }
    }
}

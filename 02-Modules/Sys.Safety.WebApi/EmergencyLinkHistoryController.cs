using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Web;
using Sys.Safety.Request.EmergencyLinkHistory;

namespace Sys.Safety.WebApi
{
    public class EmergencyLinkHistoryController : Basic.Framework.Web.WebApi.BasicApiController,IEmergencyLinkHistoryService
    {
        IEmergencyLinkHistoryService _EmergencyLinkageHistoryService = ServiceFactory.Create<IEmergencyLinkHistoryService>();

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/AddEmergencyLinkHistory")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.EmergencyLinkHistoryInfo> AddEmergencyLinkHistory(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryAddRequest emergencyLinkHistoryRequest)
        {
            return _EmergencyLinkageHistoryService.AddEmergencyLinkHistory(emergencyLinkHistoryRequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/UpdateEmergencyLinkHistory")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.EmergencyLinkHistoryInfo> UpdateEmergencyLinkHistory(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryUpdateRequest emergencyLinkHistoryRequest)
        {
            return _EmergencyLinkageHistoryService.UpdateEmergencyLinkHistory(emergencyLinkHistoryRequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/DeleteEmergencyLinkHistory")]
        public Basic.Framework.Web.BasicResponse DeleteEmergencyLinkHistory(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryDeleteRequest emergencyLinkHistoryRequest)
        {
            return _EmergencyLinkageHistoryService.DeleteEmergencyLinkHistory(emergencyLinkHistoryRequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/GetEmergencyLinkHistoryList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.EmergencyLinkHistoryInfo>> GetEmergencyLinkHistoryList(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryGetListRequest emergencyLinkHistoryRequest)
        {
            return _EmergencyLinkageHistoryService.GetEmergencyLinkHistoryList(emergencyLinkHistoryRequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/GetEmergencyLinkHistoryById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.EmergencyLinkHistoryInfo> GetEmergencyLinkHistoryById(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryGetRequest emergencyLinkHistoryRequest)
        {
            return _EmergencyLinkageHistoryService.GetEmergencyLinkHistoryById(emergencyLinkHistoryRequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/GetEmergencyLinkHistoryByEmergency")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.EmergencyLinkHistoryInfo> GetEmergencyLinkHistoryByEmergency(Sys.Safety.Request.EmergencyLinkHistory.EmergencyLinkHistoryGetByEmergencyRequest emergencyLinkHistoryRequest)
        {
            return _EmergencyLinkageHistoryService.GetEmergencyLinkHistoryByEmergency(emergencyLinkHistoryRequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/BatchAddEmergencyLinkHistory")]
        public BasicResponse BatchAddEmergencyLinkHistory(BatchAddEmergencyLinkHistoryRequest request)
        {
            return _EmergencyLinkageHistoryService.BatchAddEmergencyLinkHistory(request);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/EndAll")]
        public BasicResponse EndAll(EndAllRequest request)
        {
            return _EmergencyLinkageHistoryService.EndAll(request);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/EndByLinkageId")]
        public BasicResponse EndByLinkageId(EndByLinkageIdRequest request)
        {
            return _EmergencyLinkageHistoryService.EndByLinkageId(request);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/GetNotEndLastLinkageHistoryMasterPointByLinkageId")]
        public BasicResponse<IList<Sys.Safety.DataContract.EmergencyLinkageHistoryMasterPointAssInfo>> GetNotEndLastLinkageHistoryMasterPointByLinkageId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _EmergencyLinkageHistoryService.GetNotEndLastLinkageHistoryMasterPointByLinkageId(request);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/AddEmergencyLinkHistoryAndAss")]
        public BasicResponse AddEmergencyLinkHistoryAndAss(AddEmergencyLinkHistoryAndAssRequest request)
        {
            return _EmergencyLinkageHistoryService.AddEmergencyLinkHistoryAndAss(request);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkHistory/GetDeleteButNotEndLinkageIds")]
        public BasicResponse<IList<Sys.Safety.DataContract.SysEmergencyLinkageInfo>> GetDeleteButNotEndLinkageIds()
        {
            return _EmergencyLinkageHistoryService.GetDeleteButNotEndLinkageIds();
        }
    }
}

using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmHandle;
using Basic.Framework.Service;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class MultiplesettingController : Basic.Framework.Web.WebApi.BasicApiController, IJC_MultiplesettingService
    {
        IJC_MultiplesettingService multiplesettingService = ServiceFactory.Create<IJC_MultiplesettingService>();

        
        [HttpPost]
        [Route("v1/Multiplesetting/AddMultiplesetting")]
        public BasicResponse<JC_MultiplesettingInfo> AddMultiplesetting(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingAddRequest multiplesettingrequest)
        {
            return multiplesettingService.AddMultiplesetting(multiplesettingrequest);
        }
        [HttpPost]
        [Route("v1/Multiplesetting/UpdateMultiplesetting")]
        public BasicResponse<JC_MultiplesettingInfo> UpdateMultiplesetting(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingUpdateRequest multiplesettingrequest)
        {
            return multiplesettingService.UpdateMultiplesetting(multiplesettingrequest);
        }
        [HttpPost]
        [Route("v1/Multiplesetting/DeleteMultiplesetting")]
        public BasicResponse DeleteMultiplesetting(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingDeleteRequest multiplesettingrequest)
        {
            return multiplesettingService.DeleteMultiplesetting(multiplesettingrequest);
        }
        [HttpPost]
        [Route("v1/Multiplesetting/GetMultiplesettingList")]
        public BasicResponse<List<JC_MultiplesettingInfo>> GetMultiplesettingList(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingGetListRequest multiplesettingrequest)
        {
            return multiplesettingService.GetMultiplesettingList(multiplesettingrequest);
        }
        [HttpPost]
        [Route("v1/Multiplesetting/GetAllMultiplesettingList")]
        public BasicResponse<List<JC_MultiplesettingInfo>> GetAllMultiplesettingList()
        {
            return multiplesettingService.GetAllMultiplesettingList();
        }
        [HttpPost]
        [Route("v1/Multiplesetting/GetMultiplesettingById")]
        public BasicResponse<JC_MultiplesettingInfo> GetMultiplesettingById(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingGetRequest multiplesettingrequest)
        {
            return multiplesettingService.GetMultiplesettingById(multiplesettingrequest);
        }
    }
}

using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.WebApi
{
    public class PowerboxchargehistoryController : Basic.Framework.Web.WebApi.BasicApiController,IPowerboxchargehistoryService
    {
        static PowerboxchargehistoryController()
        {

        }
        IPowerboxchargehistoryService _IPowerboxchargehistoryService = ServiceFactory.Create<IPowerboxchargehistoryService>();
        
        [HttpPost]
        [Route("v1/Powerboxchargehistory/Add")]
        public BasicResponse<PowerboxchargehistoryInfo> AddPowerboxchargehistory(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryAddRequest powerboxchargehistoryRequest)
        {
           return _IPowerboxchargehistoryService.AddPowerboxchargehistory(powerboxchargehistoryRequest);
        }
        [HttpPost]
        [Route("v1/Powerboxchargehistory/Update")]
        public BasicResponse<PowerboxchargehistoryInfo> UpdatePowerboxchargehistory(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryUpdateRequest powerboxchargehistoryRequest)
        {
            return _IPowerboxchargehistoryService.UpdatePowerboxchargehistory(powerboxchargehistoryRequest);
        }
        [HttpPost]
        [Route("v1/Powerboxchargehistory/Delete")]
        public BasicResponse DeletePowerboxchargehistory(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryDeleteRequest powerboxchargehistoryRequest)
        {
            return _IPowerboxchargehistoryService.DeletePowerboxchargehistory(powerboxchargehistoryRequest);
        }
        [HttpPost]
        [Route("v1/Powerboxchargehistory/GetList")]
        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryList(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryGetListRequest powerboxchargehistoryRequest)
        {
            return _IPowerboxchargehistoryService.GetPowerboxchargehistoryList(powerboxchargehistoryRequest);
        }
        [HttpPost]
        [Route("v1/Powerboxchargehistory/GetById")]
        public BasicResponse<PowerboxchargehistoryInfo> GetPowerboxchargehistoryById(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryGetRequest powerboxchargehistoryRequest)
        {
            return _IPowerboxchargehistoryService.GetPowerboxchargehistoryById(powerboxchargehistoryRequest);
        }
        [HttpPost]
        [Route("v1/Powerboxchargehistory/GetPowerboxchargehistoryByFzhOrMac")]
        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryByFzhOrMac(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryGetByFzhOrMacRequest powerboxchargehistoryRequest)
        {
            return _IPowerboxchargehistoryService.GetPowerboxchargehistoryByFzhOrMac(powerboxchargehistoryRequest);
        }

        [HttpPost]
        [Route("v1/Powerboxchargehistory/GetPowerboxchargehistoryByStime")]
        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryByStime(Request.Powerboxchargehistory.PowerboxchargehistoryGetByStimeRequest powerboxchargehistoryRequest)
        {
            return _IPowerboxchargehistoryService.GetPowerboxchargehistoryByStime(powerboxchargehistoryRequest);
        }
    }
}

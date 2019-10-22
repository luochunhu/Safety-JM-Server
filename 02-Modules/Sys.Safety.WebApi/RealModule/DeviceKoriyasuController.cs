using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi.RealModule
{
    public class DeviceKoriyasuController : Basic.Framework.Web.WebApi.BasicApiController, IDeviceKoriyasuService
    {
        private IDeviceKoriyasuService deviceKoriyasuService = ServiceFactory.Create<IDeviceKoriyasuService>();

        [HttpPost]
        [Route("v1/DeviceKoriyasu/AddDeviceKoriyasu")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_DefwbInfo> AddDeviceKoriyasu(Sys.Safety.Request.Jc_Defwb.DeviceKoriyasuAddRequest jc_Defwbrequest)
        {
            return deviceKoriyasuService.AddDeviceKoriyasu(jc_Defwbrequest);
        }

        [HttpPost]
        [Route("v1/DeviceKoriyasu/UpdateDeviceKoriyasu")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_DefwbInfo> UpdateDeviceKoriyasu(Sys.Safety.Request.Jc_Defwb.Jc_DefwbUpdateRequest jc_Defwbrequest)
        {
            return deviceKoriyasuService.UpdateDeviceKoriyasu(jc_Defwbrequest);
        }

        [HttpPost]
        [Route("v1/DeviceKoriyasu/DeleteDeviceKoriyasu")]
        public Basic.Framework.Web.BasicResponse DeleteDeviceKoriyasu(Sys.Safety.Request.Jc_Defwb.Jc_DefwbDeleteRequest jc_Defwbrequest)
        {
            return deviceKoriyasuService.DeleteDeviceKoriyasu(jc_Defwbrequest);
        }

        [HttpPost]
        [Route("v1/DeviceKoriyasu/GetDeviceKoriyasuList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_DefwbInfo>> GetDeviceKoriyasuList(Sys.Safety.Request.Jc_Defwb.Jc_DefwbGetListRequest jc_Defwbrequest)
        {
            return deviceKoriyasuService.GetDeviceKoriyasuList(jc_Defwbrequest);
        }

        [HttpPost]
        [Route("v1/DeviceKoriyasu/GetDeviceKoriyasuById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_DefwbInfo> GetDeviceKoriyasuById(Sys.Safety.Request.Jc_Defwb.Jc_DefwbGetRequest jc_Defwbrequest)
        {
            return deviceKoriyasuService.GetDeviceKoriyasuById(jc_Defwbrequest);
        }
    }
}

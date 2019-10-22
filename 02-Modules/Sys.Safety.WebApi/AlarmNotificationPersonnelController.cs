using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmNotificationPersonnel;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AlarmNotificationPersonnelController : Basic.Framework.Web.WebApi.BasicApiController, IAlarmNotificationPersonnelService
    {

        IAlarmNotificationPersonnelService _AlarmNotificationPersonnelService = ServiceFactory.Create<IAlarmNotificationPersonnelService>();

        [HttpPost]
        [Route("v1/AlarmNotificationPersonnel/AddJC_AlarmNotificationPersonnel")]
        public BasicResponse<JC_AlarmNotificationPersonnelInfo> AddJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelAddRequest jC_AlarmNotificationPersonnelrequest)
        {
            return _AlarmNotificationPersonnelService.AddJC_AlarmNotificationPersonnel(jC_AlarmNotificationPersonnelrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnel/UpdateJC_AlarmNotificationPersonnel")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.JC_AlarmNotificationPersonnelInfo> UpdateJC_AlarmNotificationPersonnel(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelUpdateRequest jC_AlarmNotificationPersonnelrequest)
        {
            return _AlarmNotificationPersonnelService.UpdateJC_AlarmNotificationPersonnel(jC_AlarmNotificationPersonnelrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnel/DeleteJC_AlarmNotificationPersonnel")]
        public Basic.Framework.Web.BasicResponse DeleteJC_AlarmNotificationPersonnel(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelDeleteRequest jC_AlarmNotificationPersonnelrequest)
        {
            return _AlarmNotificationPersonnelService.DeleteJC_AlarmNotificationPersonnel(jC_AlarmNotificationPersonnelrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnel/GetJC_AlarmNotificationPersonnelList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.JC_AlarmNotificationPersonnelInfo>> GetJC_AlarmNotificationPersonnelList(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelGetListRequest jC_AlarmNotificationPersonnelrequest)
        {
            return _AlarmNotificationPersonnelService.GetJC_AlarmNotificationPersonnelList(jC_AlarmNotificationPersonnelrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnel/GetJC_AlarmNotificationPersonnelById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.JC_AlarmNotificationPersonnelInfo> GetJC_AlarmNotificationPersonnelById(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelGetRequest jC_AlarmNotificationPersonnelrequest)
        {
            return _AlarmNotificationPersonnelService.GetJC_AlarmNotificationPersonnelById(jC_AlarmNotificationPersonnelrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnel/GetJC_AlarmNotificationPersonnelListByAlarmConfigId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.JC_AlarmNotificationPersonnelInfo>> GetJC_AlarmNotificationPersonnelListByAlarmConfigId(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelGetListByAlarmConfigIdRequest jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest)
        {
            return _AlarmNotificationPersonnelService.GetJC_AlarmNotificationPersonnelListByAlarmConfigId(jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnel/GetAlarmNotificationPersonnelByAnalysisModelId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.JC_AlarmNotificationPersonnelInfo>> GetAlarmNotificationPersonnelByAnalysisModelId(AlarmNotificationPersonnelGetListByAlarmConfigIdRequest jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest)
        {
            return _AlarmNotificationPersonnelService.GetAlarmNotificationPersonnelByAnalysisModelId(jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnel/AddJC_AlarmNotificationPersonnelList")]

        public BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> AddJC_AlarmNotificationPersonnelList(AlarmNotificationPersonnelAddRequest jC_AlarmNotificationPersonnelrequest)
        {

            return _AlarmNotificationPersonnelService.AddJC_AlarmNotificationPersonnelList(jC_AlarmNotificationPersonnelrequest);
        }
    }
}

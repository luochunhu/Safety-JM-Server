using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmNotificationPersonnelConfig;
using Basic.Framework.Service;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AlarmNotificationPersonnelConfigController : Basic.Framework.Web.WebApi.BasicApiController, IAlarmNotificationPersonnelConfigService
    {

        IAlarmNotificationPersonnelConfigService _AlarmNotificationPersonnelConfigService = ServiceFactory.Create<IAlarmNotificationPersonnelConfigService>();

        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/AddJC_AlarmNotificationPersonnelConfig")]
        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> AddJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigAddRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            return _AlarmNotificationPersonnelConfigService.AddJC_AlarmNotificationPersonnelConfig(jC_Alarmnotificationpersonnelconfigrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/DeleteJC_AlarmNotificationPersonnelConfig")]
        public BasicResponse DeleteJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigDeleteRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            return _AlarmNotificationPersonnelConfigService.DeleteJC_AlarmNotificationPersonnelConfig(jC_Alarmnotificationpersonnelconfigrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/GetAlarmNotificationPersonnelConfigByAnalysisModelId")]
        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelConfigByAnalysisModelId(AlarmNotificationPersonnelConfigGetListRequest getListByAnalysisModelIdRequest)
        {
            return _AlarmNotificationPersonnelConfigService.GetAlarmNotificationPersonnelConfigByAnalysisModelId(getListByAnalysisModelIdRequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/GetJC_AlarmNotificationPersonnelConfigById")]
        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> GetJC_AlarmNotificationPersonnelConfigById(AlarmNotificationPersonnelConfigGetRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            return _AlarmNotificationPersonnelConfigService.GetJC_AlarmNotificationPersonnelConfigById(jC_Alarmnotificationpersonnelconfigrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/GetJC_AlarmNotificationPersonnelConfigList")]
        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetJC_AlarmNotificationPersonnelConfigList(AlarmNotificationPersonnelConfigGetListRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            return _AlarmNotificationPersonnelConfigService.GetJC_AlarmNotificationPersonnelConfigList(jC_Alarmnotificationpersonnelconfigrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/UpdateJC_AlarmNotificationPersonnelConfig")]
        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> UpdateJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigUpdateRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            return _AlarmNotificationPersonnelConfigService.UpdateJC_AlarmNotificationPersonnelConfig(jC_Alarmnotificationpersonnelconfigrequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/GetAlarmNotificationPersonnelListByAnalysisModeName")]
        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelListByAnalysisModeName(AlarmNotificationPersonnelConfigGetListRequest getListByAnalysisModelIdRequest)
        {
            return _AlarmNotificationPersonnelConfigService.GetAlarmNotificationPersonnelListByAnalysisModeName(getListByAnalysisModelIdRequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/HasAlarmNotificationForAnalysisModel")]
        public BasicResponse<bool> HasAlarmNotificationForAnalysisModel(GetAlarmNotificationByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            return _AlarmNotificationPersonnelConfigService.HasAlarmNotificationForAnalysisModel(getByAnalysisModelIdRequest);
        }
        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/GetAlarmNotificationPersonnelConfigAllList")]
        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelConfigAllList(GetAllAlarmNotificationRequest getAllAlarmNotificationRequest)
        {
            return _AlarmNotificationPersonnelConfigService.GetAlarmNotificationPersonnelConfigAllList(getAllAlarmNotificationRequest);
        }

        [HttpPost]
        [Route("v1/AlarmNotificationPersonnelConfig/AddAlarmNotificationPersonnelConfig")]
        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> AddAlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigListAddRequest addRequest)
        {
            return _AlarmNotificationPersonnelConfigService.AddAlarmNotificationPersonnelConfig(addRequest);
        }
    }
}

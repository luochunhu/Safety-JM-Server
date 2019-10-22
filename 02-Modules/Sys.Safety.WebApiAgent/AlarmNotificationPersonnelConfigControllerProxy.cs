using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmNotificationPersonnelConfig;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class AlarmNotificationPersonnelConfigControllerProxy : BaseProxy, IAlarmNotificationPersonnelConfigService
    {
        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> AddJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigAddRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/AddJC_AlarmNotificationPersonnelConfig?token=" + Token, JSONHelper.ToJSONString(jC_Alarmnotificationpersonnelconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmNotificationPersonnelConfigInfo>>(responseStr);
        }

        public BasicResponse DeleteJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigDeleteRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/DeleteJC_AlarmNotificationPersonnelConfig?token=" + Token, JSONHelper.ToJSONString(jC_Alarmnotificationpersonnelconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelConfigByAnalysisModelId(AlarmNotificationPersonnelConfigGetListRequest getListByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/GetAlarmNotificationPersonnelConfigByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(getListByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>>(responseStr);
        }

        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> GetJC_AlarmNotificationPersonnelConfigById(AlarmNotificationPersonnelConfigGetRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/GetJC_AlarmNotificationPersonnelConfigById?token=" + Token, JSONHelper.ToJSONString(jC_Alarmnotificationpersonnelconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmNotificationPersonnelConfigInfo>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetJC_AlarmNotificationPersonnelConfigList(AlarmNotificationPersonnelConfigGetListRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/GetJC_AlarmNotificationPersonnelConfigList?token=" + Token, JSONHelper.ToJSONString(jC_Alarmnotificationpersonnelconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>>(responseStr);
        }

        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> UpdateJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigUpdateRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/UpdateJC_AlarmNotificationPersonnelConfig?token=" + Token, JSONHelper.ToJSONString(jC_Alarmnotificationpersonnelconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmNotificationPersonnelConfigInfo>>(responseStr);
        }


        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelListByAnalysisModeName(AlarmNotificationPersonnelConfigGetListRequest getListByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/GetAlarmNotificationPersonnelListByAnalysisModeName?token=" + Token, JSONHelper.ToJSONString(getListByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>>(responseStr);
        }

        public BasicResponse<bool> HasAlarmNotificationForAnalysisModel(GetAlarmNotificationByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/HasAlarmNotificationForAnalysisModel?token=" + Token, JSONHelper.ToJSONString(getByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelConfigAllList(GetAllAlarmNotificationRequest getAllAlarmNotificationRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/GetAlarmNotificationPersonnelConfigAllList?token=" + Token, JSONHelper.ToJSONString(getAllAlarmNotificationRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> AddAlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigListAddRequest addRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnelConfig/AddAlarmNotificationPersonnelConfig?token=" + Token, JSONHelper.ToJSONString(addRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>>(responseStr);
        }
    }
}

using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class EmergencyLinkageConfigControllerProxy : BaseProxy, IEmergencyLinkageConfigService
    {
        public BasicResponse<JC_EmergencyLinkageConfigInfo> AddJC_Emergencylinkageconfig(EmergencyLinkageConfigAddRequest jC_Emergencylinkageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkageConfig/AddJC_Emergencylinkageconfig?token=" + Token, JSONHelper.ToJSONString(jC_Emergencylinkageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_EmergencyLinkageConfigInfo>>(responseStr);
        }

        public BasicResponse DeleteJC_Emergencylinkageconfig(EmergencylinkageconfigDeleteRequest jC_Emergencylinkageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkageConfig/DeleteJC_Emergencylinkageconfig?token=" + Token, JSONHelper.ToJSONString(jC_Emergencylinkageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse DeleteJC_EmergencylinkageconfigByAnalysisModelId(EmergencyLinkageConfigGetByAnalysisModelIdRequest emergencyLinkageConfigGetByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkageConfig/DeleteJC_EmergencylinkageconfigByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(emergencyLinkageConfigGetByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<JC_EmergencyLinkageConfigInfo> GetJC_EmergencylinkageconfigById(EmergencyLinkageConfigGetRequest jC_Emergencylinkageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkageConfig/GetJC_EmergencylinkageconfigById?token=" + Token, JSONHelper.ToJSONString(jC_Emergencylinkageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_EmergencyLinkageConfigInfo>>(responseStr);
        }

        public BasicResponse<List<JC_EmergencyLinkageConfigInfo>> GetJC_EmergencylinkageconfigList(EmergencyLinkageConfigGetListRequest jC_Emergencylinkageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkageConfig/GetJC_EmergencylinkageconfigList?token=" + Token, JSONHelper.ToJSONString(jC_Emergencylinkageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_EmergencyLinkageConfigInfo>>>(responseStr);
        }

        public BasicResponse<JC_EmergencyLinkageConfigInfo> GetJC_EmergencylinkageconfigByAnalysisModelId(EmergencyLinkageConfigGetByAnalysisModelIdRequest emergencyLinkageConfigGetByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkageConfig/GetJC_EmergencylinkageconfigByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(emergencyLinkageConfigGetByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_EmergencyLinkageConfigInfo>>(responseStr);
        }

        public BasicResponse<JC_EmergencyLinkageConfigInfo> UpdateJC_Emergencylinkageconfig(EmergencyLinkageConfigUpdateRequest jC_Emergencylinkageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkageConfig/UpdateJC_Emergencylinkageconfig?token=" + Token, JSONHelper.ToJSONString(jC_Emergencylinkageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_EmergencyLinkageConfigInfo>>(responseStr);
        }

        public BasicResponse<List<JC_EmergencyLinkageConfigInfo>> GetEmergencyLinkageConfigAllList(GetAllEmergencyLinkageConfigRequest getAllEmergencyLinkageConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/EmergencyLinkageConfig/GetEmergencyLinkageConfigAllList?token=" + Token, JSONHelper.ToJSONString(getAllEmergencyLinkageConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_EmergencyLinkageConfigInfo>>>(responseStr);
        }
    }
}
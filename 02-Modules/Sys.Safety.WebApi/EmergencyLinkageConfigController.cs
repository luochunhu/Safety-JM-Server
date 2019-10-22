using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Basic.Framework.Service;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class EmergencyLinkageConfigController : Basic.Framework.Web.WebApi.BasicApiController, IEmergencyLinkageConfigService
    {
        IEmergencyLinkageConfigService _EmergencyLinkageConfigService = ServiceFactory.Create<IEmergencyLinkageConfigService>();

        [HttpPost]
        [Route("v1/EmergencyLinkageConfig/AddJC_Emergencylinkageconfig")]
        public BasicResponse<JC_EmergencyLinkageConfigInfo> AddJC_Emergencylinkageconfig(EmergencyLinkageConfigAddRequest jC_Emergencylinkageconfigrequest)
        {
            return _EmergencyLinkageConfigService.AddJC_Emergencylinkageconfig(jC_Emergencylinkageconfigrequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkageConfig/DeleteJC_Emergencylinkageconfig")]
        public BasicResponse DeleteJC_Emergencylinkageconfig(EmergencylinkageconfigDeleteRequest jC_Emergencylinkageconfigrequest)
        {
            return _EmergencyLinkageConfigService.DeleteJC_Emergencylinkageconfig(jC_Emergencylinkageconfigrequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkageConfig/DeleteJC_EmergencylinkageconfigByAnalysisModelId")]
        public BasicResponse DeleteJC_EmergencylinkageconfigByAnalysisModelId(EmergencyLinkageConfigGetByAnalysisModelIdRequest emergencyLinkageConfigGetByAnalysisModelIdRequest)
        {
            return _EmergencyLinkageConfigService.DeleteJC_EmergencylinkageconfigByAnalysisModelId(emergencyLinkageConfigGetByAnalysisModelIdRequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkageConfig/GetJC_EmergencylinkageconfigById")]
        public BasicResponse<JC_EmergencyLinkageConfigInfo> GetJC_EmergencylinkageconfigById(EmergencyLinkageConfigGetRequest jC_Emergencylinkageconfigrequest)
        {
            return _EmergencyLinkageConfigService.GetJC_EmergencylinkageconfigById(jC_Emergencylinkageconfigrequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkageConfig/GetJC_EmergencylinkageconfigList")]
        public BasicResponse<List<JC_EmergencyLinkageConfigInfo>> GetJC_EmergencylinkageconfigList(EmergencyLinkageConfigGetListRequest jC_Emergencylinkageconfigrequest)
        {
            return _EmergencyLinkageConfigService.GetJC_EmergencylinkageconfigList(jC_Emergencylinkageconfigrequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkageConfig/GetJC_EmergencylinkageconfigByAnalysisModelId")]
        public BasicResponse<JC_EmergencyLinkageConfigInfo> GetJC_EmergencylinkageconfigByAnalysisModelId(EmergencyLinkageConfigGetByAnalysisModelIdRequest emergencyLinkageConfigGetByAnalysisModelIdRequest)
        {
            return _EmergencyLinkageConfigService.GetJC_EmergencylinkageconfigByAnalysisModelId(emergencyLinkageConfigGetByAnalysisModelIdRequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkageConfig/UpdateJC_Emergencylinkageconfig")]
        public BasicResponse<JC_EmergencyLinkageConfigInfo> UpdateJC_Emergencylinkageconfig(EmergencyLinkageConfigUpdateRequest jC_Emergencylinkageconfigrequest)
        {
            return _EmergencyLinkageConfigService.UpdateJC_Emergencylinkageconfig(jC_Emergencylinkageconfigrequest);
        }

        [HttpPost]
        [Route("v1/EmergencyLinkageConfig/GetEmergencyLinkageConfigAllList")]
        public BasicResponse<List<JC_EmergencyLinkageConfigInfo>> GetEmergencyLinkageConfigAllList(GetAllEmergencyLinkageConfigRequest getAllEmergencyLinkageConfigRequest)
        {
            return _EmergencyLinkageConfigService.GetEmergencyLinkageConfigAllList(getAllEmergencyLinkageConfigRequest);
        }
    }
}
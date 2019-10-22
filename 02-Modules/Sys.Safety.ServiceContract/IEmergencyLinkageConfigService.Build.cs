using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkageConfigService
    {
        BasicResponse<JC_EmergencyLinkageConfigInfo> AddJC_Emergencylinkageconfig(EmergencyLinkageConfigAddRequest jC_Emergencylinkageconfigrequest);
        BasicResponse<JC_EmergencyLinkageConfigInfo> UpdateJC_Emergencylinkageconfig(EmergencyLinkageConfigUpdateRequest jC_Emergencylinkageconfigrequest);
        BasicResponse DeleteJC_Emergencylinkageconfig(EmergencylinkageconfigDeleteRequest jC_Emergencylinkageconfigrequest);
        BasicResponse DeleteJC_EmergencylinkageconfigByAnalysisModelId(EmergencyLinkageConfigGetByAnalysisModelIdRequest emergencyLinkageConfigGetByAnalysisModelIdRequest);
        BasicResponse<List<JC_EmergencyLinkageConfigInfo>> GetJC_EmergencylinkageconfigList(EmergencyLinkageConfigGetListRequest jC_Emergencylinkageconfigrequest);
        BasicResponse<JC_EmergencyLinkageConfigInfo> GetJC_EmergencylinkageconfigById(EmergencyLinkageConfigGetRequest jC_Emergencylinkageconfigrequest);

        BasicResponse<JC_EmergencyLinkageConfigInfo> GetJC_EmergencylinkageconfigByAnalysisModelId(EmergencyLinkageConfigGetByAnalysisModelIdRequest emergencyLinkageConfigGetByAnalysisModelIdRequest);

        /// <summary>
        /// 获取所有应急联动配置
        /// </summary>
        /// <param name="getAllEmergencyLinkageConfigRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_EmergencyLinkageConfigInfo>> GetEmergencyLinkageConfigAllList(GetAllEmergencyLinkageConfigRequest getAllEmergencyLinkageConfigRequest);
    }
}


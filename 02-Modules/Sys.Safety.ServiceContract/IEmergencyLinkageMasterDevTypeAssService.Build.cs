using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.EmergencyLinkageMasterDevTypeAss;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkageMasterDevTypeAssService
    {
        BasicResponse<EmergencyLinkageMasterDevTypeAssInfo> AddEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssAddRequest emergencyLinkageMasterDevTypeAssRequest);

        BasicResponse<EmergencyLinkageMasterDevTypeAssInfo> UpdateEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssUpdateRequest emergencyLinkageMasterDevTypeAssRequest);

        BasicResponse DeleteEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssDeleteRequest emergencyLinkageMasterDevTypeAssRequest);

        BasicResponse<List<EmergencyLinkageMasterDevTypeAssInfo>> GetEmergencyLinkageMasterDevTypeAssList(
            EmergencyLinkageMasterDevTypeAssGetListRequest emergencyLinkageMasterDevTypeAssRequest);

        BasicResponse<EmergencyLinkageMasterDevTypeAssInfo> GetEmergencyLinkageMasterDevTypeAssById(
            EmergencyLinkageMasterDevTypeAssGetRequest emergencyLinkageMasterDevTypeAssRequest);

        BasicResponse<IList<EmergencyLinkageMasterDevTypeAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(
            LongIdRequest request);
    }
}
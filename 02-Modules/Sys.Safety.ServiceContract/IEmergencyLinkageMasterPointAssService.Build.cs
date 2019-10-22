using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.EmergencyLinkageMasterPointAss;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkageMasterPointAssService
    {
        BasicResponse<EmergencyLinkageMasterPointAssInfo> AddEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssAddRequest emergencyLinkageMasterPointAssRequest);

        BasicResponse<EmergencyLinkageMasterPointAssInfo> UpdateEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssUpdateRequest emergencyLinkageMasterPointAssRequest);

        BasicResponse DeleteEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssDeleteRequest emergencyLinkageMasterPointAssRequest);

        BasicResponse<List<EmergencyLinkageMasterPointAssInfo>> GetEmergencyLinkageMasterPointAssList(
            EmergencyLinkageMasterPointAssGetListRequest emergencyLinkageMasterPointAssRequest);

        BasicResponse<EmergencyLinkageMasterPointAssInfo> GetEmergencyLinkageMasterPointAssById(
            EmergencyLinkageMasterPointAssGetRequest emergencyLinkageMasterPointAssRequest);

        BasicResponse<IList<EmergencyLinkageMasterPointAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(
            LongIdRequest request);
    }
}
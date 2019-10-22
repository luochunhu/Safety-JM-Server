using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.EmergencyLinkageMasterAreaAss;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkageMasterAreaAssService
    {
        BasicResponse<EmergencyLinkageMasterAreaAssInfo> AddEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssAddRequest emergencyLinkageMasterAreaAssRequest);

        BasicResponse<EmergencyLinkageMasterAreaAssInfo> UpdateEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssUpdateRequest emergencyLinkageMasterAreaAssRequest);

        BasicResponse DeleteEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssDeleteRequest emergencyLinkageMasterAreaAssRequest);

        BasicResponse<List<EmergencyLinkageMasterAreaAssInfo>> GetEmergencyLinkageMasterAreaAssList(
            EmergencyLinkageMasterAreaAssGetListRequest emergencyLinkageMasterAreaAssRequest);

        BasicResponse<EmergencyLinkageMasterAreaAssInfo> GetEmergencyLinkageMasterAreaAssById(
            EmergencyLinkageMasterAreaAssGetRequest emergencyLinkageMasterAreaAssRequest);

        BasicResponse<IList<EmergencyLinkageMasterAreaAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(
            LongIdRequest request);
    }
}
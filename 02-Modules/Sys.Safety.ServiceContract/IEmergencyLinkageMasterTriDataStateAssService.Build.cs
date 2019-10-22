using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.EmergencyLinkageMasterTriDataStateAss;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkageMasterTriDataStateAssService
    {
        BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo> AddEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssAddRequest emergencyLinkageMasterTriDataStateAssRequest);

        BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo> UpdateEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssUpdateRequest emergencyLinkageMasterTriDataStateAssRequest);

        BasicResponse DeleteEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssDeleteRequest emergencyLinkageMasterTriDataStateAssRequest);

        BasicResponse<List<EmergencyLinkageMasterTriDataStateAssInfo>> GetEmergencyLinkageMasterTriDataStateAssList(
            EmergencyLinkageMasterTriDataStateAssGetListRequest emergencyLinkageMasterTriDataStateAssRequest);

        BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo> GetEmergencyLinkageMasterTriDataStateAssById(
            EmergencyLinkageMasterTriDataStateAssGetRequest emergencyLinkageMasterTriDataStateAssRequest);

        BasicResponse<IList<EmergencyLinkageMasterTriDataStateAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(
            LongIdRequest request);
    }
}
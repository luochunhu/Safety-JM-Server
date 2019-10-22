using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.EmergencyLinkagePassivePointAss;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkagePassivePointAssService
    {
        BasicResponse<EmergencyLinkagePassivePointAssInfo> AddEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssAddRequest emergencyLinkagePassivePointAssRequest);

        BasicResponse<EmergencyLinkagePassivePointAssInfo> UpdateEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssUpdateRequest emergencyLinkagePassivePointAssRequest);

        BasicResponse DeleteEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssDeleteRequest emergencyLinkagePassivePointAssRequest);

        BasicResponse<List<EmergencyLinkagePassivePointAssInfo>> GetEmergencyLinkagePassivePointAssList(
            EmergencyLinkagePassivePointAssGetListRequest emergencyLinkagePassivePointAssRequest);

        BasicResponse<EmergencyLinkagePassivePointAssInfo> GetEmergencyLinkagePassivePointAssById(
            EmergencyLinkagePassivePointAssGetRequest emergencyLinkagePassivePointAssRequest);

        BasicResponse<IList<EmergencyLinkagePassivePointAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(
            LongIdRequest request);
    }
}
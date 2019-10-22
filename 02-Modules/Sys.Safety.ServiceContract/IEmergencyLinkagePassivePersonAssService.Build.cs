using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.EmergencyLinkagePassivePersonAss;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkagePassivePersonAssService
    {
        BasicResponse<EmergencyLinkagePassivePersonAssInfo> AddEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssAddRequest emergencyLinkagePassivePersonAssRequest);

        BasicResponse<EmergencyLinkagePassivePersonAssInfo> UpdateEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssUpdateRequest emergencyLinkagePassivePersonAssRequest);

        BasicResponse DeleteEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssDeleteRequest emergencyLinkagePassivePersonAssRequest);

        BasicResponse<List<EmergencyLinkagePassivePersonAssInfo>> GetEmergencyLinkagePassivePersonAssList(
            EmergencyLinkagePassivePersonAssGetListRequest emergencyLinkagePassivePersonAssRequest);

        BasicResponse<EmergencyLinkagePassivePersonAssInfo> GetEmergencyLinkagePassivePersonAssById(
            EmergencyLinkagePassivePersonAssGetRequest emergencyLinkagePassivePersonAssRequest);

        BasicResponse<IList<EmergencyLinkagePassivePersonAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(
            LongIdRequest request);
    }
}
using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.EmergencyLinkagePassiveAreaAss;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkagePassiveAreaAssService
    {
        BasicResponse<EmergencyLinkagePassiveAreaAssInfo> AddEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssAddRequest emergencyLinkagePassiveAreaAssRequest);

        BasicResponse<EmergencyLinkagePassiveAreaAssInfo> UpdateEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssUpdateRequest emergencyLinkagePassiveAreaAssRequest);

        BasicResponse DeleteEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssDeleteRequest emergencyLinkagePassiveAreaAssRequest);

        BasicResponse<List<EmergencyLinkagePassiveAreaAssInfo>> GetEmergencyLinkagePassiveAreaAssList(
            EmergencyLinkagePassiveAreaAssGetListRequest emergencyLinkagePassiveAreaAssRequest);

        BasicResponse<EmergencyLinkagePassiveAreaAssInfo> GetEmergencyLinkagePassiveAreaAssById(
            EmergencyLinkagePassiveAreaAssGetRequest emergencyLinkagePassiveAreaAssRequest);

        BasicResponse<IList<EmergencyLinkagePassiveAreaAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(
            LongIdRequest request);
    }
}
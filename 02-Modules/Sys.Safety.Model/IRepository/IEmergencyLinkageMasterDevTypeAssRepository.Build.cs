using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmergencyLinkageMasterDevTypeAssRepository : IRepository<EmergencyLinkageMasterDevTypeAssModel>
    {
        EmergencyLinkageMasterDevTypeAssModel AddEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssModel emergencyLinkageMasterDevTypeAssModel);

        void UpdateEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssModel emergencyLinkageMasterDevTypeAssModel);

        void DeleteEmergencyLinkageMasterDevTypeAss(string id);

        IList<EmergencyLinkageMasterDevTypeAssModel> GetEmergencyLinkageMasterDevTypeAssList(int pageIndex,
            int pageSize, out int rowCount);

        EmergencyLinkageMasterDevTypeAssModel GetEmergencyLinkageMasterDevTypeAssById(string id);

        IList<EmergencyLinkageMasterDevTypeAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id);
    }
}
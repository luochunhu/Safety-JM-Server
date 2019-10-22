using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmergencyLinkageMasterPointAssRepository : IRepository<EmergencyLinkageMasterPointAssModel>
    {
        EmergencyLinkageMasterPointAssModel AddEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssModel emergencyLinkageMasterPointAssModel);

        void UpdateEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssModel emergencyLinkageMasterPointAssModel);

        void DeleteEmergencyLinkageMasterPointAss(string id);

        IList<EmergencyLinkageMasterPointAssModel> GetEmergencyLinkageMasterPointAssList(int pageIndex, int pageSize,
            out int rowCount);

        EmergencyLinkageMasterPointAssModel GetEmergencyLinkageMasterPointAssById(string id);

        IList<EmergencyLinkageMasterPointAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id);
    }
}
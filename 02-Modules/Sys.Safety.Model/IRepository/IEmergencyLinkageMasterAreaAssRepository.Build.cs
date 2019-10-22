using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmergencyLinkageMasterAreaAssRepository : IRepository<EmergencyLinkageMasterAreaAssModel>
    {
        EmergencyLinkageMasterAreaAssModel AddEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssModel emergencyLinkageMasterAreaAssModel);

        void UpdateEmergencyLinkageMasterAreaAss(EmergencyLinkageMasterAreaAssModel emergencyLinkageMasterAreaAssModel);
        void DeleteEmergencyLinkageMasterAreaAss(string id);

        IList<EmergencyLinkageMasterAreaAssModel> GetEmergencyLinkageMasterAreaAssList(int pageIndex, int pageSize,
            out int rowCount);

        EmergencyLinkageMasterAreaAssModel GetEmergencyLinkageMasterAreaAssById(string id);

        IList<EmergencyLinkageMasterAreaAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id);
    }
}
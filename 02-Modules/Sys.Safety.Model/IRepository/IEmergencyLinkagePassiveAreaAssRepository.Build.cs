using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmergencyLinkagePassiveAreaAssRepository : IRepository<EmergencyLinkagePassiveAreaAssModel>
    {
        EmergencyLinkagePassiveAreaAssModel AddEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssModel emergencyLinkagePassiveAreaAssModel);

        void UpdateEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssModel emergencyLinkagePassiveAreaAssModel);

        void DeleteEmergencyLinkagePassiveAreaAss(string id);

        IList<EmergencyLinkagePassiveAreaAssModel> GetEmergencyLinkagePassiveAreaAssList(int pageIndex, int pageSize,
            out int rowCount);

        EmergencyLinkagePassiveAreaAssModel GetEmergencyLinkagePassiveAreaAssById(string id);

        IList<EmergencyLinkagePassiveAreaAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id);
    }
}
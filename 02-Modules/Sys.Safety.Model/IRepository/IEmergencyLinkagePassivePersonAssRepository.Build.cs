using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmergencyLinkagePassivePersonAssRepository : IRepository<EmergencyLinkagePassivePersonAssModel>
    {
        EmergencyLinkagePassivePersonAssModel AddEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssModel emergencyLinkagePassivePersonAssModel);

        void UpdateEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssModel emergencyLinkagePassivePersonAssModel);

        void DeleteEmergencyLinkagePassivePersonAss(string id);

        IList<EmergencyLinkagePassivePersonAssModel> GetEmergencyLinkagePassivePersonAssList(int pageIndex,
            int pageSize, out int rowCount);

        EmergencyLinkagePassivePersonAssModel GetEmergencyLinkagePassivePersonAssById(string id);

        IList<EmergencyLinkagePassivePersonAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id);
    }
}
using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmergencyLinkagePassivePointAssRepository : IRepository<EmergencyLinkagePassivePointAssModel>
    {
        EmergencyLinkagePassivePointAssModel AddEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssModel emergencyLinkagePassivePointAssModel);

        void UpdateEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssModel emergencyLinkagePassivePointAssModel);

        void DeleteEmergencyLinkagePassivePointAss(string id);

        IList<EmergencyLinkagePassivePointAssModel> GetEmergencyLinkagePassivePointAssList(int pageIndex, int pageSize,
            out int rowCount);

        EmergencyLinkagePassivePointAssModel GetEmergencyLinkagePassivePointAssById(string id);

        IList<EmergencyLinkagePassivePointAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id);
    }
}
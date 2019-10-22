using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface
        IEmergencyLinkageHistoryMasterPointAssRepository : IRepository<EmergencyLinkageHistoryMasterPointAssModel>
    {
        EmergencyLinkageHistoryMasterPointAssModel AddEmergencyLinkageHistoryMasterPointAss(
            EmergencyLinkageHistoryMasterPointAssModel emergencyLinkageHistoryMasterPointAssModel);

        void UpdateEmergencyLinkageHistoryMasterPointAss(
            EmergencyLinkageHistoryMasterPointAssModel emergencyLinkageHistoryMasterPointAssModel);

        void DeleteEmergencyLinkageHistoryMasterPointAss(string id);

        IList<EmergencyLinkageHistoryMasterPointAssModel> GetEmergencyLinkageHistoryMasterPointAssList(int pageIndex,
            int pageSize, out int rowCount);

        EmergencyLinkageHistoryMasterPointAssModel GetEmergencyLinkageHistoryMasterPointAssById(string id);

        IList<EmergencyLinkageHistoryMasterPointAssModel>
            GetLinkageHistoryMasterPointAssListByLinkageHistoryId(string id);
    }
}
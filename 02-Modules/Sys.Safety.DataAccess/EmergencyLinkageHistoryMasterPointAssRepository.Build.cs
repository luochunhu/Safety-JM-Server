using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkageHistoryMasterPointAssRepository :
        RepositoryBase<EmergencyLinkageHistoryMasterPointAssModel>, IEmergencyLinkageHistoryMasterPointAssRepository
    {
        public EmergencyLinkageHistoryMasterPointAssModel AddEmergencyLinkageHistoryMasterPointAss(
            EmergencyLinkageHistoryMasterPointAssModel emergencyLinkageHistoryMasterPointAssModel)
        {
            return Insert(emergencyLinkageHistoryMasterPointAssModel);
        }

        public void UpdateEmergencyLinkageHistoryMasterPointAss(
            EmergencyLinkageHistoryMasterPointAssModel emergencyLinkageHistoryMasterPointAssModel)
        {
            Update(emergencyLinkageHistoryMasterPointAssModel);
        }

        public void DeleteEmergencyLinkageHistoryMasterPointAss(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkageHistoryMasterPointAssModel> GetEmergencyLinkageHistoryMasterPointAssList(
            int pageIndex, int pageSize, out int rowCount)
        {
            var emergencyLinkageHistoryMasterPointAssModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkageHistoryMasterPointAssModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkageHistoryMasterPointAssModel GetEmergencyLinkageHistoryMasterPointAssById(string id)
        {
            var emergencyLinkageHistoryMasterPointAssModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkageHistoryMasterPointAssModel;
        }

        public IList<EmergencyLinkageHistoryMasterPointAssModel>
            GetLinkageHistoryMasterPointAssListByLinkageHistoryId(string id)
        {
            var res = Datas.Where(a => a.EmergencyLinkHistoryId == id).ToList();
            return res;
        }
    }
}
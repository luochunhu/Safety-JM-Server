using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkagePassivePersonAssRepository : RepositoryBase<EmergencyLinkagePassivePersonAssModel>,
        IEmergencyLinkagePassivePersonAssRepository
    {
        public EmergencyLinkagePassivePersonAssModel AddEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssModel emergencyLinkagePassivePersonAssModel)
        {
            return Insert(emergencyLinkagePassivePersonAssModel);
        }

        public void UpdateEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssModel emergencyLinkagePassivePersonAssModel)
        {
            Update(emergencyLinkagePassivePersonAssModel);
        }

        public void DeleteEmergencyLinkagePassivePersonAss(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkagePassivePersonAssModel> GetEmergencyLinkagePassivePersonAssList(int pageIndex,
            int pageSize, out int rowCount)
        {
            var emergencyLinkagePassivePersonAssModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkagePassivePersonAssModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkagePassivePersonAssModel GetEmergencyLinkagePassivePersonAssById(string id)
        {
            var emergencyLinkagePassivePersonAssModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkagePassivePersonAssModel;
        }

        public IList<EmergencyLinkagePassivePersonAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id)
        {
            var models = Datas.Where(a => a.PassivePersonAssId == id).ToList();
            return models;
        }
    }
}
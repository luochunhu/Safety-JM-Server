using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkagePassiveAreaAssRepository : RepositoryBase<EmergencyLinkagePassiveAreaAssModel>,
        IEmergencyLinkagePassiveAreaAssRepository
    {
        public EmergencyLinkagePassiveAreaAssModel AddEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssModel emergencyLinkagePassiveAreaAssModel)
        {
            return Insert(emergencyLinkagePassiveAreaAssModel);
        }

        public void UpdateEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssModel emergencyLinkagePassiveAreaAssModel)
        {
            Update(emergencyLinkagePassiveAreaAssModel);
        }

        public void DeleteEmergencyLinkagePassiveAreaAss(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkagePassiveAreaAssModel> GetEmergencyLinkagePassiveAreaAssList(int pageIndex,
            int pageSize, out int rowCount)
        {
            var emergencyLinkagePassiveAreaAssModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkagePassiveAreaAssModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkagePassiveAreaAssModel GetEmergencyLinkagePassiveAreaAssById(string id)
        {
            var emergencyLinkagePassiveAreaAssModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkagePassiveAreaAssModel;
        }

        public IList<EmergencyLinkagePassiveAreaAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id)
        {
            var models = Datas.Where(a => a.PassiveAreaAssId == id).ToList();
            return models;
        }
    }
}
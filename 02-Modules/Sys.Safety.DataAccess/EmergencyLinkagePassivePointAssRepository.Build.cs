using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkagePassivePointAssRepository : RepositoryBase<EmergencyLinkagePassivePointAssModel>,
        IEmergencyLinkagePassivePointAssRepository
    {
        public EmergencyLinkagePassivePointAssModel AddEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssModel emergencyLinkagePassivePointAssModel)
        {
            return Insert(emergencyLinkagePassivePointAssModel);
        }

        public void UpdateEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssModel emergencyLinkagePassivePointAssModel)
        {
            Update(emergencyLinkagePassivePointAssModel);
        }

        public void DeleteEmergencyLinkagePassivePointAss(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkagePassivePointAssModel> GetEmergencyLinkagePassivePointAssList(int pageIndex,
            int pageSize, out int rowCount)
        {
            var emergencyLinkagePassivePointAssModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkagePassivePointAssModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkagePassivePointAssModel GetEmergencyLinkagePassivePointAssById(string id)
        {
            var emergencyLinkagePassivePointAssModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkagePassivePointAssModel;
        }

        public IList<EmergencyLinkagePassivePointAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id)
        {
            var models = Datas.Where(a => a.PassivePointAssId == id).ToList();
            return models;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkageMasterDevTypeAssRepository : RepositoryBase<EmergencyLinkageMasterDevTypeAssModel>,
        IEmergencyLinkageMasterDevTypeAssRepository
    {
        public EmergencyLinkageMasterDevTypeAssModel AddEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssModel emergencyLinkageMasterDevTypeAssModel)
        {
            return Insert(emergencyLinkageMasterDevTypeAssModel);
        }

        public void UpdateEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssModel emergencyLinkageMasterDevTypeAssModel)
        {
            Update(emergencyLinkageMasterDevTypeAssModel);
        }

        public void DeleteEmergencyLinkageMasterDevTypeAss(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkageMasterDevTypeAssModel> GetEmergencyLinkageMasterDevTypeAssList(int pageIndex,
            int pageSize, out int rowCount)
        {
            var emergencyLinkageMasterDevTypeAssModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkageMasterDevTypeAssModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkageMasterDevTypeAssModel GetEmergencyLinkageMasterDevTypeAssById(string id)
        {
            var emergencyLinkageMasterDevTypeAssModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkageMasterDevTypeAssModel;
        }

        public IList<EmergencyLinkageMasterDevTypeAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id)
        {
            var models = Datas.Where(a => a.MasterDevTypeAssId == id).ToList();
            return models;
        }
    }
}
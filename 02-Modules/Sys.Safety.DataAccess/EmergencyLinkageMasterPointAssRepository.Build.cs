using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkageMasterPointAssRepository : RepositoryBase<EmergencyLinkageMasterPointAssModel>,
        IEmergencyLinkageMasterPointAssRepository
    {
        public EmergencyLinkageMasterPointAssModel AddEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssModel emergencyLinkageMasterPointAssModel)
        {
            return Insert(emergencyLinkageMasterPointAssModel);
        }

        public void UpdateEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssModel emergencyLinkageMasterPointAssModel)
        {
            Update(emergencyLinkageMasterPointAssModel);
        }

        public void DeleteEmergencyLinkageMasterPointAss(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkageMasterPointAssModel> GetEmergencyLinkageMasterPointAssList(int pageIndex,
            int pageSize, out int rowCount)
        {
            var emergencyLinkageMasterPointAssModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkageMasterPointAssModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkageMasterPointAssModel GetEmergencyLinkageMasterPointAssById(string id)
        {
            var emergencyLinkageMasterPointAssModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkageMasterPointAssModel;
        }

        public IList<EmergencyLinkageMasterPointAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id)
        {
            var models = Datas.Where(a => a.MasterPointAssId == id).ToList();
            return models;
        }
    }
}
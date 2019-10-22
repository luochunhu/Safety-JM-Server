using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkageMasterAreaAssRepository : RepositoryBase<EmergencyLinkageMasterAreaAssModel>,
        IEmergencyLinkageMasterAreaAssRepository
    {
        public EmergencyLinkageMasterAreaAssModel AddEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssModel emergencyLinkageMasterAreaAssModel)
        {
            return Insert(emergencyLinkageMasterAreaAssModel);
        }

        public void UpdateEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssModel emergencyLinkageMasterAreaAssModel)
        {
            Update(emergencyLinkageMasterAreaAssModel);
        }

        public void DeleteEmergencyLinkageMasterAreaAss(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkageMasterAreaAssModel> GetEmergencyLinkageMasterAreaAssList(int pageIndex,
            int pageSize, out int rowCount)
        {
            var emergencyLinkageMasterAreaAssModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkageMasterAreaAssModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkageMasterAreaAssModel GetEmergencyLinkageMasterAreaAssById(string id)
        {
            var emergencyLinkageMasterAreaAssModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkageMasterAreaAssModel;
        }

        public IList<EmergencyLinkageMasterAreaAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id)
        {
            var models = Datas.Where(a => a.MasterAreaAssId == id).ToList();
            return models;
        }
    }
}
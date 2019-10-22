using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkageMasterTriDataStateAssRepository :
        RepositoryBase<EmergencyLinkageMasterTriDataStateAssModel>, IEmergencyLinkageMasterTriDataStateAssRepository
    {
        public EmergencyLinkageMasterTriDataStateAssModel AddEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssModel emergencyLinkageMasterTriDataStateAssModel)
        {
            return Insert(emergencyLinkageMasterTriDataStateAssModel);
        }

        public void UpdateEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssModel emergencyLinkageMasterTriDataStateAssModel)
        {
            Update(emergencyLinkageMasterTriDataStateAssModel);
        }

        public void DeleteEmergencyLinkageMasterTriDataStateAss(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkageMasterTriDataStateAssModel> GetEmergencyLinkageMasterTriDataStateAssList(
            int pageIndex, int pageSize, out int rowCount)
        {
            var emergencyLinkageMasterTriDataStateAssModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkageMasterTriDataStateAssModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkageMasterTriDataStateAssModel GetEmergencyLinkageMasterTriDataStateAssById(string id)
        {
            var emergencyLinkageMasterTriDataStateAssModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkageMasterTriDataStateAssModel;
        }

        public IList<EmergencyLinkageMasterTriDataStateAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id)
        {
            var models = Datas.Where(a => a.MasterTriDataStateAssId == id).ToList();
            return models;
        }
    }
}
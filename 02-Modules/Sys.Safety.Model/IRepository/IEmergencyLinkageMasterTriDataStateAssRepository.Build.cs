using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface
        IEmergencyLinkageMasterTriDataStateAssRepository : IRepository<EmergencyLinkageMasterTriDataStateAssModel>
    {
        EmergencyLinkageMasterTriDataStateAssModel AddEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssModel emergencyLinkageMasterTriDataStateAssModel);

        void UpdateEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssModel emergencyLinkageMasterTriDataStateAssModel);

        void DeleteEmergencyLinkageMasterTriDataStateAss(string id);

        IList<EmergencyLinkageMasterTriDataStateAssModel> GetEmergencyLinkageMasterTriDataStateAssList(int pageIndex,
            int pageSize, out int rowCount);

        EmergencyLinkageMasterTriDataStateAssModel GetEmergencyLinkageMasterTriDataStateAssById(string id);

        IList<EmergencyLinkageMasterTriDataStateAssModel> GetEmergencyLinkageMasterAreaAssListByAssId(string id);
    }
}
using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmergencyLinkageConfigRepository : IRepository<JC_EmergencylinkageconfigModel>
    {
        JC_EmergencylinkageconfigModel AddJC_Emergencylinkageconfig(JC_EmergencylinkageconfigModel jC_EmergencylinkageconfigModel);
        void UpdateJC_Emergencylinkageconfig(JC_EmergencylinkageconfigModel jC_EmergencylinkageconfigModel);
        void DeleteJC_Emergencylinkageconfig(string id);
        void DeleteJC_EmergencylinkageconfigByAnalysisModelId(string AnalysisModelId); 
        IList<JC_EmergencylinkageconfigModel> GetJC_EmergencylinkageconfigList(int pageIndex, int pageSize, out int rowCount);
        JC_EmergencylinkageconfigModel GetJC_EmergencylinkageconfigById(string id);

        JC_EmergencylinkageconfigModel GetJC_EmergencylinkageconfigByAnalysisModelId(string AnalysisModelId);
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class EmergencyLinkageConfigRepository : RepositoryBase<JC_EmergencylinkageconfigModel>, IEmergencyLinkageConfigRepository
    {

        public JC_EmergencylinkageconfigModel AddJC_Emergencylinkageconfig(JC_EmergencylinkageconfigModel jC_EmergencylinkageconfigModel)
        {
            return base.Insert(jC_EmergencylinkageconfigModel);
        }
        public void UpdateJC_Emergencylinkageconfig(JC_EmergencylinkageconfigModel jC_EmergencylinkageconfigModel)
        {
            base.Update(jC_EmergencylinkageconfigModel);
        }
        public void DeleteJC_Emergencylinkageconfig(string id)
        {
            base.Delete(id);
        }
        public void DeleteJC_EmergencylinkageconfigByAnalysisModelId(string AnalysisModelId)
        {
            var jC_EmergencylinkageconfigModelLists = base.Datas.Where(t => t.AnalysisModelId == AnalysisModelId).ToList();
            base.Delete(jC_EmergencylinkageconfigModelLists);
        }
        public IList<JC_EmergencylinkageconfigModel> GetJC_EmergencylinkageconfigList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_EmergencylinkageconfigModelLists = base.Datas;
            rowCount = jC_EmergencylinkageconfigModelLists.Count();
            return jC_EmergencylinkageconfigModelLists.OrderBy(t=>t.AnalysisModelId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_EmergencylinkageconfigModel GetJC_EmergencylinkageconfigByAnalysisModelId(string AnalysisModelId)
        {
            var jC_EmergencylinkageconfigModel = base.Datas.FirstOrDefault(t => t.AnalysisModelId == AnalysisModelId);
            return jC_EmergencylinkageconfigModel;
        }
        public JC_EmergencylinkageconfigModel GetJC_EmergencylinkageconfigById(string id)
        {
            JC_EmergencylinkageconfigModel jC_EmergencylinkageconfigModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_EmergencylinkageconfigModel;
        }
    }
}

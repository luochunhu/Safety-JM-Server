using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class RegionOutageConfigRepository : RepositoryBase<JC_RegionoutageconfigModel>, IRegionOutageConfigRepository
    {

        public JC_RegionoutageconfigModel AddJC_Regionoutageconfig(JC_RegionoutageconfigModel jC_RegionoutageconfigModel)
        {
            return base.Insert(jC_RegionoutageconfigModel);
        }
        public List<JC_RegionoutageconfigModel> AddJC_RegionOutageConfigList(List<JC_RegionoutageconfigModel> jC_RegionoutageconfigModel)
        {
            return base.Insert(jC_RegionoutageconfigModel).ToList();
        }
        public void UpdateJC_Regionoutageconfig(JC_RegionoutageconfigModel jC_RegionoutageconfigModel)
        {
            base.Update(jC_RegionoutageconfigModel);
        }
        public void DeleteJC_Regionoutageconfig(string id)
        {
            base.Delete(id);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="analysisModelId">模板ID</param>
        public void DeleteUserRoleByAnalysisModelId(string analysisModelId)
        {
            var getObjectList = base.Datas.Where(c => c.AnalysisModelId == analysisModelId).ToList();
            base.Delete(getObjectList);
        }
        public IList<JC_RegionoutageconfigModel> GetJC_RegionoutageconfigList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_RegionoutageconfigModelLists = base.Datas;
            rowCount = jC_RegionoutageconfigModelLists.Count();
            return jC_RegionoutageconfigModelLists.OrderByDescending(t=>t.UpdatedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_RegionoutageconfigModel GetJC_RegionoutageconfigById(string id)
        {
            JC_RegionoutageconfigModel jC_RegionoutageconfigModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_RegionoutageconfigModel;
        }

        public List<JC_RegionoutageconfigModel> GetRegionOutageConfigListByAnalysisModelId(string analysisModelId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q=>q.AnalysisModelId == analysisModelId);

            return query.ToList();
        }

        public bool HasRegionOutageForAnalysisModel(string analysisModelId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == analysisModelId);

            return query.Any();
        }
    }
}

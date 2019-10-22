using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AnalysisTemplateConfigRepository : RepositoryBase<JC_AnalysistemplateconfigModel>, IAnalysisTemplateConfigRepository
    {

        public JC_AnalysistemplateconfigModel AddJC_Analysistemplateconfig(JC_AnalysistemplateconfigModel jC_AnalysistemplateconfigModel)
        {
            return base.Insert(jC_AnalysistemplateconfigModel);
        }

        /// <summary>
        /// 批量新增分析模型配置
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        public List<JC_AnalysistemplateconfigModel> AddAnalysistemplateconfigList(IList<JC_AnalysistemplateconfigModel> jC_AnalysistemplateconfigModelList)
        {
            return base.Insert(jC_AnalysistemplateconfigModelList).ToList();
        }

        public void UpdateJC_Analysistemplateconfig(JC_AnalysistemplateconfigModel jC_AnalysistemplateconfigModel)
        {
            base.Update(jC_AnalysistemplateconfigModel);
        }
        public void DeleteJC_Analysistemplateconfig(string id)
        {
            base.Delete(id);
        }
        public IList<JC_AnalysistemplateconfigModel> GetJC_AnalysistemplateconfigList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_AnalysistemplateconfigModelLists = base.Datas;
            rowCount = jC_AnalysistemplateconfigModelLists.Count();
            return jC_AnalysistemplateconfigModelLists.OrderBy(t=>t.TempleteId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_AnalysistemplateconfigModel GetJC_AnalysistemplateconfigById(string id)
        {
            JC_AnalysistemplateconfigModel jC_AnalysistemplateconfigModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_AnalysistemplateconfigModel;
        }

        /// <summary>
        /// 根据模板ID查询模型信息
        /// </summary>
        /// <param name="TempleteId">模板ID</param>
        /// <returns></returns>
        public List<JC_AnalysistemplateconfigModel> GetJC_AnalysistemplateconfigByTempleteId(string TempleteId)
        {
            return base.Datas.Where(c => c.TempleteId == TempleteId).ToList();
        }
        /// <summary>
        /// 根据模板ID查询模型信息
        /// </summary>
        /// <param name="TempleteId">模板ID</param>
        /// <returns></returns>
        public void DeleteJC_AnalysistemplateconfigByTempleteId(string templeteId)
        {
            base.Delete(c => c.TempleteId == templeteId);
        }
    }
}

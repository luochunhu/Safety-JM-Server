using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAnalysisTemplateConfigRepository : IRepository<JC_AnalysistemplateconfigModel>
    {
        JC_AnalysistemplateconfigModel AddJC_Analysistemplateconfig(JC_AnalysistemplateconfigModel jC_AnalysistemplateconfigModel);
        /// <summary>
        /// 批量新增分析模型配置
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        List<JC_AnalysistemplateconfigModel> AddAnalysistemplateconfigList(IList<JC_AnalysistemplateconfigModel> jC_AnalysistemplateconfigModelList);
        void UpdateJC_Analysistemplateconfig(JC_AnalysistemplateconfigModel jC_AnalysistemplateconfigModel);
        void DeleteJC_Analysistemplateconfig(string id);
        IList<JC_AnalysistemplateconfigModel> GetJC_AnalysistemplateconfigList(int pageIndex, int pageSize, out int rowCount);
        JC_AnalysistemplateconfigModel GetJC_AnalysistemplateconfigById(string id);
        /// <summary>
        /// 根据模板ID查询模型信息
        /// </summary>
        /// <param name="TempleteId">模板ID</param>
        /// <returns></returns>
        List<JC_AnalysistemplateconfigModel> GetJC_AnalysistemplateconfigByTempleteId(string TempleteId);
        /// <summary>
        /// 根据模板ID查询模型信息
        /// </summary>
        /// <param name="TempleteId">模板ID</param>
        /// <returns></returns>
        void DeleteJC_AnalysistemplateconfigByTempleteId(string templeteId);
    }
}

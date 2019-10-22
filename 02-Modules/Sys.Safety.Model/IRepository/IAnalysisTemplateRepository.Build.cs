using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAnalysisTemplateRepository : IRepository<JC_AnalysistemplateModel>
    {
        JC_AnalysistemplateModel AddJC_Analysistemplate(JC_AnalysistemplateModel jC_AnalysistemplateModel);
        void UpdateJC_Analysistemplate(JC_AnalysistemplateModel jC_AnalysistemplateModel);
        void DeleteJC_Analysistemplate(string id);
        IList<JC_AnalysistemplateModel> GetJC_AnalysistemplateList(int pageIndex, int pageSize, out int rowCount);
        /// <summary>
        /// 根据name模糊查询模板列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<JC_AnalysistemplateModel> GetJC_AnalysistemplateListByName(string name, int pageIndex, int pageSize, out int rowCount);
        JC_AnalysistemplateModel GetJC_AnalysistemplateById(string id);
        JC_AnalysistemplateModel GetJC_AnalysistemplateByName(string name);
        JC_AnalysistemplateModel GetJC_AnalysistemplateByTempleteId(string templeteId);

    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAnalyticalExpressionRepository : IRepository<JC_AnalyticalexpressionModel>
    {
        JC_AnalyticalexpressionModel AddJC_Analyticalexpression(JC_AnalyticalexpressionModel jC_AnalyticalexpressionModel);

        /// <summary>
        /// 批量新增表达式
        /// </summary>
        /// <param name="analysistemplateModelList"></param>
        /// <returns></returns>
        List<JC_AnalyticalexpressionModel> AddAnalyticalexpressionList(IList<JC_AnalyticalexpressionModel> jC_AnalyticalexpressionModelList);
        void UpdateJC_Analyticalexpression(JC_AnalyticalexpressionModel jC_AnalyticalexpressionModel);
        void DeleteJC_Analyticalexpression(string id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="TempleteId"></param>
        void DeleteJC_AnalyticalexpressionByTempleteId(string templeteId);
       
        IList<JC_AnalyticalexpressionModel> GetJC_AnalyticalexpressionList(int pageIndex, int pageSize, out int rowCount);
        JC_AnalyticalexpressionModel GetJC_AnalyticalexpressionById(string id);
    }
}

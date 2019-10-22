using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;
using System.Data;
using Basic.Framework.Common;

namespace Sys.Safety.DataAccess
{
    public partial class AnalyticalExpressionRepository : RepositoryBase<JC_AnalyticalexpressionModel>, IAnalyticalExpressionRepository
    {

        public JC_AnalyticalexpressionModel AddJC_Analyticalexpression(JC_AnalyticalexpressionModel jC_AnalyticalexpressionModel)
        {
            return base.Insert(jC_AnalyticalexpressionModel);
        }

        /// <summary>
        /// 批量新增表达式
        /// </summary>
        /// <param name="analysistemplateModelList"></param>
        /// <returns></returns>
        public List<JC_AnalyticalexpressionModel> AddAnalyticalexpressionList(IList<JC_AnalyticalexpressionModel> jC_AnalyticalexpressionModelList)
        {
          
            return base.Insert(jC_AnalyticalexpressionModelList).ToList();  
        }

        public void UpdateJC_Analyticalexpression(JC_AnalyticalexpressionModel jC_AnalyticalexpressionModel)
        {
            base.Update(jC_AnalyticalexpressionModel);
        }
        public void DeleteJC_Analyticalexpression(string id)
        {
            base.Delete(id);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="TempleteId"></param>
        public void DeleteJC_AnalyticalexpressionByTempleteId(string templeteId)
        {
            DataTable dataTable = base.QueryTable("global_AnalyticalExpressionService_GetAnalyticalExpressionListByTempleteId", templeteId);

            List<JC_AnalyticalexpressionModel> listResult = ObjectConverter.Copy<JC_AnalyticalexpressionModel>(dataTable);

            base.Delete(listResult);
        }
        public IList<JC_AnalyticalexpressionModel> GetJC_AnalyticalexpressionList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_AnalyticalexpressionModelLists = base.Datas;
            rowCount = jC_AnalyticalexpressionModelLists.Count();
            return jC_AnalyticalexpressionModelLists.OrderByDescending(t=>t.UpdatedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_AnalyticalexpressionModel GetJC_AnalyticalexpressionById(string id)
        {
            JC_AnalyticalexpressionModel jC_AnalyticalexpressionModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_AnalyticalexpressionModel;
        }
    }
}

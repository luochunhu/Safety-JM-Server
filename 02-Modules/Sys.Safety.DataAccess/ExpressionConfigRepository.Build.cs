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
    public partial class ExpressionConfigRepository : RepositoryBase<JC_ExpressionconfigModel>, IExpressionConfigRepository
    {

        public JC_ExpressionconfigModel AddJC_Expressionconfig(JC_ExpressionconfigModel jC_ExpressionconfigModel)
        {
            return base.Insert(jC_ExpressionconfigModel);
        }
        /// <summary>
        /// 批量新增表达式配置
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        public List<JC_ExpressionconfigModel> AddExpressionconfigList(IList<JC_ExpressionconfigModel> jC_ExpressionconfigModelList)
        {
            return base.Insert(jC_ExpressionconfigModelList).ToList();
        }


        public void UpdateJC_Expressionconfig(JC_ExpressionconfigModel jC_ExpressionconfigModel)
        {
            base.Update(jC_ExpressionconfigModel);
        }
        public void DeleteJC_Expressionconfig(string id)
        {
            base.Delete(id);
        }
        public void DeleteJC_ExpressionconfigByTempleteId(string templeteId)
        {
            DataTable dataTable = base.QueryTable("global_JC_ExpressionConfigService_GetJC_ExpressionConfigListByTempleteId", templeteId);

            List<JC_ExpressionconfigModel> listResult = ObjectConverter.Copy<JC_ExpressionconfigModel>(dataTable);


            base.Delete(listResult);
        }
      
        public IList<JC_ExpressionconfigModel> GetJC_ExpressionconfigList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_ExpressionconfigModelLists = base.Datas;
            rowCount = jC_ExpressionconfigModelLists.Count();
            return jC_ExpressionconfigModelLists.OrderBy(t=>t.ExpressionId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_ExpressionconfigModel GetJC_ExpressionconfigById(string id)
        {
            JC_ExpressionconfigModel jC_ExpressionconfigModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_ExpressionconfigModel;
        }

        /// <summary>
        /// 根据表达式ID 获取表达式配置信息
        /// </summary>
        /// <param name="ExpressionId"></param>
        /// <returns></returns>
        public IList<JC_ExpressionconfigModel> GetJC_ExpressionconfigListByExpressionId(string expressionId)
        {
            var jC_ExpressionconfigModelLists = base.Datas.Where(c => c.ExpressionId == expressionId).ToList();

            return jC_ExpressionconfigModelLists;
        }
    }
}

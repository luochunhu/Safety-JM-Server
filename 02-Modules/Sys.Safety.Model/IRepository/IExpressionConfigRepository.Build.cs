using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IExpressionConfigRepository : IRepository<JC_ExpressionconfigModel>
    {
        JC_ExpressionconfigModel AddJC_Expressionconfig(JC_ExpressionconfigModel jC_ExpressionconfigModel);
        /// <summary>
        /// 批量新增表达式配置
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        List<JC_ExpressionconfigModel> AddExpressionconfigList(IList<JC_ExpressionconfigModel> jC_ExpressionconfigModelList);
        void UpdateJC_Expressionconfig(JC_ExpressionconfigModel jC_ExpressionconfigModel);
        void DeleteJC_Expressionconfig(string id);
        void DeleteJC_ExpressionconfigByTempleteId(string templeteId);  
        IList<JC_ExpressionconfigModel> GetJC_ExpressionconfigList(int pageIndex, int pageSize, out int rowCount);
        /// <summary>
        /// 根据表达式ID 获取表达式配置信息
        /// </summary>
        /// <param name="ExpressionId"></param>
        /// <returns></returns>
        IList<JC_ExpressionconfigModel> GetJC_ExpressionconfigListByExpressionId(string expressionId);
        JC_ExpressionconfigModel GetJC_ExpressionconfigById(string id);
    }
}

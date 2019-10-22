using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Analyticalexpression;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IAnalyticalExpressionService
    {
        BasicResponse<JC_AnalyticalExpressionInfo> AddJC_Analyticalexpression(AnalyticalExpressionAddRequest jC_Analyticalexpressionrequest);
        /// <summary>
        /// 批量新增表达式
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AnalyticalExpressionInfo>> AddAnalyticalExpressionList(AnalyticalExpressionListAddRequest jC_AnalyticalExpressionrequest);
                /// <summary>
        /// 根据模板ID 查询表达式信息
        /// </summary>
        /// <param name="jC_Analysistemplaterequest">模型ID</param>
        /// <returns>模型列表</returns>
         BasicResponse<List<JC_AnalyticalExpressionInfo>> GetAnalysisTemplateListByTempleteId(AnalyticalExpressionGetListRequest jC_Analysistemplaterequest);
        BasicResponse<JC_AnalyticalExpressionInfo> UpdateJC_Analyticalexpression(AnalyticalExpressionUpdateRequest jC_Analyticalexpressionrequest);
        BasicResponse DeleteJC_Analyticalexpression(AnalyticalExpressionDeleteRequest jC_Analyticalexpressionrequest);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="jC_Analysistemplaterequest">模型ID</param>
        /// <returns></returns>
        BasicResponse DeleteJC_AnalyticalexpressionByTempleteId(AnalyticalExpressionGetListRequest jC_Analysistemplaterequest);
        BasicResponse<List<JC_AnalyticalExpressionInfo>> GetJC_AnalyticalexpressionList(AnalyticalExpressionGetListRequest jC_Analyticalexpressionrequest);
        BasicResponse<JC_AnalyticalExpressionInfo> GetJC_AnalyticalexpressionById(AnalyticalExpressionGetRequest jC_Analyticalexpressionrequest);
    }
}


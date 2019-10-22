using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Expressionconfig;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface 
        IExpressionConfigService
    {
        BasicResponse<JC_ExpressionConfigInfo> AddJC_Expressionconfig(ExpressionConfigAddRequest jC_Expressionconfigrequest);
        /// <summary>
        /// 批量新增表达式配置
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        BasicResponse<List<JC_ExpressionConfigInfo>> AddExpressionConfigList(ExpressionConfigListAddRequest jC_ExpressionConfigListAddRequest);
        BasicResponse<JC_ExpressionConfigInfo> UpdateJC_Expressionconfig(ExpressionConfigUpdateRequest jC_Expressionconfigrequest);
        BasicResponse DeleteJC_Expressionconfig(ExpressionconfigDeleteRequest jC_Expressionconfigrequest);
        BasicResponse<List<JC_ExpressionConfigInfo>> GetJC_ExpressionconfigList(ExpressionConfigGetListRequest jC_Expressionconfigrequest);
        /// <summary>
        /// 根据表达式ID 获取表达式配置信息
        /// </summary>
        /// <param name="ExpressionId"></param>
        /// <returns></returns>
        BasicResponse<List<JC_ExpressionConfigInfo>> GetJC_ExpressionconfigListByExpressionId(ExpressionConfigGetByExpressionIdRequest expressionId);
         /// <summary>
        /// 根据模板ID 查询表达式配置信息
        /// </summary>
        /// <param name="jC_ExpressionConfigrequest">模型ID</param>
        /// <returns>模型列表</returns>
         BasicResponse<List<JC_ExpressionConfigInfo>> GetExpressionConfigListByTempleteId(ExpressionConfigGetListRequest jC_Analysistemplaterequest);
        BasicResponse<JC_ExpressionConfigInfo> GetJC_ExpressionconfigById(ExpressionConfigGetRequest jC_Expressionconfigrequest);
                /// <summary>
        /// 根据模板ID 删除表达式配置信息
        /// </summary>
        /// <param name="jC_ExpressionConfigrequest">模型ID</param>
        /// <returns></returns>
         BasicResponse DeleteJC_ExpressionconfigByTempleteId(ExpressionConfigGetListRequest jC_Analysistemplaterequest);
    }
}


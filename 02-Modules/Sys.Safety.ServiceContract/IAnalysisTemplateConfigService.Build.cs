using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.AnalysisTemplateConfig;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IAnalysisTemplateConfigService
    {
        BasicResponse<JC_AnalysisTemplateConfigInfo> AddJC_Analysistemplateconfig(AnalysisTemplateConfigAddRequest jC_Analysistemplateconfigrequest);
        /// <summary>
        /// 批量新增分析模型配置
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AnalysisTemplateConfigInfo>> AddAnalysistemplateconfigList(AnalysisTemplateConfigListAddRequest jC_Analysistemplateconfigrequest);
        BasicResponse<JC_AnalysisTemplateConfigInfo> UpdateJC_Analysistemplateconfig(AnalysisTemplateConfigUpdateRequest jC_Analysistemplateconfigrequest);
        BasicResponse DeleteJC_Analysistemplateconfig(AnalysisTemplateConfigDeleteRequest jC_Analysistemplateconfigrequest);
        BasicResponse<List<JC_AnalysisTemplateConfigInfo>> GetJC_AnalysistemplateconfigList(AnalysisTemplateConfigGetListRequest jC_Analysistemplateconfigrequest);
        BasicResponse<JC_AnalysisTemplateConfigInfo> GetJC_AnalysistemplateconfigById(AnalysisTemplateConfigGetRequest jC_Analysistemplateconfigrequest);
           /// <summary>
        /// 根据模板ID查询模型信息
        /// </summary>
        /// <param name="TempleteId">模板ID</param>
        /// <returns></returns>
        BasicResponse<List<JC_AnalysisTemplateConfigInfo>> GetJC_AnalysistemplateconfigByTempleteId(AnalysisTemplateConfigGetByTempleteIdRequest jC_AnalysisTemplateConfigGetByTempleteIdRequest);
     /// <summary>
        /// 根据模板ID删除模板表达式配置关系
        /// </summary>
        /// <param name="TempleteId">模板ID</param>
        /// <returns></returns>
        BasicResponse DeleteJC_AnalysistemplateconfigByTempleteId(AnalysisTemplateConfigGetByTempleteIdRequest jC_AnalysisTemplateConfigGetByTempleteIdRequest);
    }
}


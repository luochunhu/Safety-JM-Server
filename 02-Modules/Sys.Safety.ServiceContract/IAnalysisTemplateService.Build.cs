using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.AnalysisTemplate;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IAnalysisTemplateService
    {
        BasicResponse<JC_AnalysisTemplateInfo> AddJC_Analysistemplate(AnalysisTemplateAddRequest jC_Analysistemplaterequest);
        BasicResponse<JC_AnalysisTemplateInfo> UpdateJC_Analysistemplate(AnalysisTemplateUpdateRequest jC_Analysistemplaterequest);
        BasicResponse DeleteJC_Analysistemplate(AnalysisTemplateDeleteRequest jC_Analysistemplaterequest);
        BasicResponse<List<JC_AnalysisTemplateInfo>> GetJC_AnalysistemplateList(AnalysisTemplateGetListRequest jC_Analysistemplaterequest);
        BasicResponse<JC_AnalysisTemplateInfo> GetJC_AnalysistemplateById(AnalysisTemplateGetRequest jC_Analysistemplaterequest);
        /// <summary>
        /// 根据模型ID查询模型列表（模型ID为空时，查询所有）.
        /// </summary>
        /// <param name="jC_Analysistemplaterequest">模型ID</param>
        /// <returns>模型列表</returns>
        BasicResponse<List<JC_AnalysisTemplateInfo>> GetAnalysisTemplateListDetail(AnalysisTemplateGetRequest jC_Analysistemplaterequest);
        BasicResponse<JC_AnalysisTemplateInfo> GetJC_AnalysistemplateByTempleteId(AnalysisTemplateGetRequest jC_Analysistemplaterequest);
        /// <summary>
        /// 根据模板name模糊查询模板列表
        /// </summary>
        /// <param name="jC_Analysistemplaterequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AnalysisTemplateInfo>> GetJC_AnalysistemplateListByName(AnalysisTemplateGetListByNameRequest jC_Analysistemplaterequest); 
    }
}


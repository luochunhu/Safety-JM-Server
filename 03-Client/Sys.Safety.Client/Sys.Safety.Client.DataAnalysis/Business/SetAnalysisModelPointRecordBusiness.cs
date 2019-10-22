using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Setanalysismodelpointrecord;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Business
{
    public class SetAnalysisModelPointRecordBusiness
    {

        //配置模型测点
        ISetAnalysisModelPointRecordService setAnalysisModelPointRecordService = ServiceFactory.Create<ISetAnalysisModelPointRecordService>();

        /// <summary>
        /// 根据模板ID查询分析模型测点配置信息
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public List<JC_SetAnalysisModelPointRecordInfo> GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId(string templateId)
        {
            BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> analysisModelPointResponse = setAnalysisModelPointRecordService.GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId(new SetAnalysisModelPointRecordGetTempleteRequest() { TemplateId = templateId });

            if (analysisModelPointResponse.Data != null)
                return analysisModelPointResponse.Data.ToList();
            return new List<JC_SetAnalysisModelPointRecordInfo>();
        }
        /// <summary>
        /// 根据模型ID查询分析模型测点配置信息
        /// </summary>
        /// <param name="analysisModelId"></param>
        /// <returns></returns>
        public List<JC_SetAnalysisModelPointRecordInfo> GetAnalysisModelPointRecordsByAnalysisModelId(string analysisModelId)
        {
            BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> analysisModelPointResponse = setAnalysisModelPointRecordService.GetAnalysisModelPointRecordsByAnalysisModelId(
                new SetAnalysisModelPointRecordByModelIdGetRequest() {AnalysisModelId= analysisModelId });

            if (analysisModelPointResponse.Data != null)
                return analysisModelPointResponse.Data.ToList();
            return new List<JC_SetAnalysisModelPointRecordInfo>();
        }
        /// <summary>
        /// 查询所有模型测点信息
        /// </summary>
        /// <param name="analysisModelId"></param>
        /// <returns></returns>
        public List<JC_SetAnalysisModelPointRecordInfo> GetAllAnalysisModelPointList()
        {
            BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> analysisModelPointResponse = setAnalysisModelPointRecordService.GetAllAnalysisModelPointList();

            if (analysisModelPointResponse.Data != null)
                return analysisModelPointResponse.Data.ToList();
            return new List<JC_SetAnalysisModelPointRecordInfo>();
        }
        
    }
}

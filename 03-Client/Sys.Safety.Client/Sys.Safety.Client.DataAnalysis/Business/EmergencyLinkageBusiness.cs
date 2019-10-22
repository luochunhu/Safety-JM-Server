using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Business
{
    public class EmergencyLinkageBusiness
    {
        //创建业务服务对象（走webapi远程调用）
        //应急联动
        IEmergencyLinkageConfigService emergencyLinkageConfigService = ServiceFactory.Create<IEmergencyLinkageConfigService>();

        /// <summary>
        /// 新增应急联动
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string AddEmergencyLinkageConfig(EmergencyLinkageConfigBusinessModel data)
        {
            try
            {
                string error = "";

                //1.删除
                BasicResponse deleteByAnalysisModelIdResult =
                  emergencyLinkageConfigService.DeleteJC_EmergencylinkageconfigByAnalysisModelId(
                      new EmergencyLinkageConfigGetByAnalysisModelIdRequest() { AnalysisModelId = data.AnalysisModelId }
                    );

                //2.新增
               // 调用接口
                BasicResponse<JC_EmergencyLinkageConfigInfo> analysisTemplateResult =
                   emergencyLinkageConfigService.AddJC_Emergencylinkageconfig(
                   new EmergencyLinkageConfigAddRequest() { JC_EmergencyLinkageConfigInfo=data.EmergencyLinkageConfigInfo});

                if (analysisTemplateResult.IsSuccess == false)
                {
                    error = analysisTemplateResult.Message;
                }
                else
                {
                    if (analysisTemplateResult.Code != 100)
                        error = analysisTemplateResult.Message;
                    else
                        error = "100";
                }


                return error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        /// <summary>
        /// 查询应急联动信息
        /// </summary>
        /// <param name="id">模板ID</param>
        /// <returns></returns>
        public JC_EmergencyLinkageConfigInfo GetEmergencylinkageconfig(string analysisModelId)
        {
            try
            {
                BasicResponse<JC_EmergencyLinkageConfigInfo> emergencyLinkageResult =  emergencyLinkageConfigService.GetJC_EmergencylinkageconfigByAnalysisModelId(
                    new EmergencyLinkageConfigGetByAnalysisModelIdRequest() { AnalysisModelId = analysisModelId }
                    );
                return emergencyLinkageResult.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            return null;
        }
    }
}

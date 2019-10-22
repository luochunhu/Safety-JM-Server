
using Basic.Framework.Data;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.DataContract;
using Sys.Safety.Services;
using Sys.Safety.Request.AnalysisTemplate;
using Sys.Safety.Request.RegionOutageConfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Business
{
    public class RegionOutageBusiness
    {
        //创建业务服务对象（走webapi远程调用）
        //模板配置
        IRegionOutageConfigService regionOutageConfigService = ServiceFactory.Create<IRegionOutageConfigService>();

        //模型
        ILargedataAnalysisConfigService largedataAnalysisConfigService = ServiceFactory.Create<ILargedataAnalysisConfigService>();

        /// <summary>
        /// 新增配置区域断电
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string AddRegionOutageConfig(RegionOutageBusinessModel data)
        {
            try
            {
                string error = "";

                //2.新增
                RegionOutageConfigListAddRequest regionOutageConfigListRequest = new RegionOutageConfigListAddRequest();
                regionOutageConfigListRequest.JC_RegionOutageConfigInfoList = data.RegionOutageConfigInfoList;
               regionOutageConfigListRequest.AnalysisModelId = data.AnalysisModelId;  
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_RegionOutageConfigInfo>> analysisTemplateResult =
                    regionOutageConfigService.AddJC_RegionOutageConfigList(regionOutageConfigListRequest);

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
                return "-100";
            }

        }


        /// <summary>
        /// 查询区域断电信息
        /// </summary>
        /// <param name="id">模型ID</param>
        /// <returns></returns>
        public List<JC_RegionOutageConfigInfo> GetRegionOutage(string analysisModelId)
        {
            try
            {
                RegionOutageConfigGetListRequest regionOutageConfigListRequest = new RegionOutageConfigGetListRequest();
                regionOutageConfigListRequest.AnalysisModelId = analysisModelId;
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_RegionOutageConfigInfo>> analysisTemplateResult =
                    regionOutageConfigService.GetRegionOutageConfigListByAnalysisModelId(regionOutageConfigListRequest);

                if (analysisTemplateResult.Data != null && analysisTemplateResult.Data.Count > 0)
                {
                    return analysisTemplateResult.Data;
                }
                else
                {
                    return new List<JC_RegionOutageConfigInfo>();
                }

            }
            catch (Exception ex)
            {
                return new List<JC_RegionOutageConfigInfo>();
            }

        }
         /// <summary>
        /// 清除区域断电信息
        /// </summary>
        /// <param name="id">模型ID</param>
        /// <returns></returns>
        public string DeleteJC_RegionoutageconfigByAnalysisModelId(string analysisModelId)
        {
            try
            {
                RegionoutageconfigDeleteByAnalysisModelIdRequest regionOutageConfigListRequest = new RegionoutageconfigDeleteByAnalysisModelIdRequest();
                regionOutageConfigListRequest.AnalysisModelId = analysisModelId;
                //调用接口
               BasicResponse analysisTemplateResult =
                    regionOutageConfigService.DeleteJC_RegionoutageconfigByAnalysisModelId(regionOutageConfigListRequest);

                if (analysisTemplateResult.IsSuccess==true)
                {
                    return "100";
                }
                else
                {
                    return analysisTemplateResult.Message;
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string NoReleaseControlForAnalysysModelAndPoint(string analysisModelId, string pointId)
        {
            BasicResponse response = regionOutageConfigService.NoReleaseControlForAnalysysModelAndPoint(new ReleaseControlCheckRequest() { AnalysisModelId = analysisModelId, PointId = pointId });
            if (!response.IsSuccess)
                return response.Message;
            return string.Empty;
        }
    }
}

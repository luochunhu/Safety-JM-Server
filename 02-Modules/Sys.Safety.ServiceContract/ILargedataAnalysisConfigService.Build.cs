using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Largedataanalysisconfig;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface ILargedataAnalysisConfigService
    {
        BasicResponse<JC_LargedataAnalysisConfigInfo> AddLargeDataAnalysisConfig(LargedataAnalysisConfigAddRequest jc_LargedataAnalysisConfigRequest);
        BasicResponse<JC_LargedataAnalysisConfigInfo> UpdateLargeDataAnalysisConfig(LargedataAnalysisConfigUpdateRequest jc_LargedataAnalysisConfigRequest);
        BasicResponse DeleteLargeDataAnalysisConfig(LargedataAnalysisConfigDeleteRequest jc_LargedataAnalysisConfigRequest);
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest);
        /// <summary>
        /// 获取所有分析配置列表
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRequest">分析配置请求对象</param>
        /// <returns>所有分析配置列表</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest);

        /// <summary>
        /// 获取启用并带有关联测点的分析配置列表
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRequest">分析配置请求对象</param>
        /// <returns>启用并带有关联测点的分析配置列表</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllEnabledLargeDataAnalysisConfigWithDetail(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest);
        BasicResponse<JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigById(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRequest);
        /// <summary>
        /// 根据模型ID查询模型详细信息
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRsequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargedataAnalysisConfigDetailById(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRsequest);

        /// <summary>
        /// 根据模板ID查询分析模型配置信息
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListByTempleteId(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRequest);
        /// <summary>
        ///根据模型名称模糊查询模型列表
        /// </summary>
        /// <returns>模型列表</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListByName(LargedataAnalysisConfigGetListByNameRequest jc_LargedataAnalysisConfigRequest);


        /// <summary>
        /// 获取没有关联报警通知的分析模型
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRequest">分析配置请求对象</param>
        /// <returns>获取没有关联报警通知的分析模型</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithoutAlarmConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest);

        /// <summary>
        /// 获取没有关联应急联动的分析模型
        /// </summary>
        /// <param name="largedataAnalysisConfigGetListRequest">分析配置请求对象</param>
        /// <returns>没有关联应急联动的分析模型</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithoutRegionOutage(LargedataAnalysisConfigGetListRequest largedataAnalysisConfigGetListRequest);

        /// <summary>
        /// 获取关联应急联动的分析模型
        /// </summary>
        /// <param name="largedataAnalysisConfigGetListWithRegionOutageRequest">关联应急联动的请求对象</param>
        /// <returns>获取关联应急联动的分析模型</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithRegionOutage(LargedataAnalysisConfigGetListWithRegionOutageRequest largedataAnalysisConfigGetListWithRegionOutageRequest);
        /// <summary>
        /// 获取关联应急联动的分析模型(分页)
        /// </summary>
        /// <param name="largedataAnalysisConfigGetListWithRegionOutageRequest">关联应急联动的请求对象</param>
        /// <returns>获取关联应急联动的分析模型</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithRegionOutagePage(LargedataAnalysisConfigGetListWithRegionOutageRequest largedataAnalysisConfigGetListWithRegionOutageRequest);

        /// <summary>
        /// 大数据曲线查询可查看的模型
        /// </summary>
        /// <param name="largeDataAnalysisConfigListForCurveRequest"></param>
        /// <returns>大数据曲线查询可查看的模型</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListForCurve(LargeDataAnalysisConfigListForCurveRequest largeDataAnalysisConfigListForCurveRequest);
    }
}


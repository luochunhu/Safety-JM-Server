using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.RegionOutageConfig;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRegionOutageConfigService
    {
        /// <summary>
        /// 批量新增区域断点设置
        /// </summary>
        /// <param name="jC_RegionOutageConfigListAddRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_RegionOutageConfigInfo>> AddJC_RegionOutageConfigList(RegionOutageConfigListAddRequest jC_RegionOutageConfigListAddRequest);
        BasicResponse<JC_RegionOutageConfigInfo> AddJC_Regionoutageconfig(RegionOutageConfigAddRequest jC_Regionoutageconfigrequest);
        BasicResponse<JC_RegionOutageConfigInfo> UpdateJC_Regionoutageconfig(RegionOutageConfigUpdateRequest jC_Regionoutageconfigrequest);
        BasicResponse DeleteJC_Regionoutageconfig(RegionoutageconfigDeleteRequest jC_Regionoutageconfigrequest);
        BasicResponse DeleteJC_RegionoutageconfigByAnalysisModelId(RegionoutageconfigDeleteByAnalysisModelIdRequest jC_Regionoutageconfigrequest);
        BasicResponse<List<JC_RegionOutageConfigInfo>> GetJC_RegionoutageconfigList(RegionOutageConfigGetListRequest jC_Regionoutageconfigrequest);
        BasicResponse<JC_RegionOutageConfigInfo> GetJC_RegionoutageconfigById(RegionOutageConfigGetRequest jC_Regionoutageconfigrequest);

        BasicResponse<List<JC_RegionOutageConfigInfo>> GetRegionOutageConfigListByAnalysisModelId(RegionOutageConfigGetListRequest regionOutageConfigGetListRequest);
        /// <summary>
        /// 检查分析模型配置的控制点是否存在解控
        /// </summary>
        /// <param name="releaseControlCheckRequest">检查分析模型配置的控制点是否存在解控的请求对象</param>
        /// <returns>检查分析模型配置的控制点是否存在解控</returns>
        BasicResponse NoReleaseControlForAnalysysModelAndPoint(ReleaseControlCheckRequest releaseControlCheckRequest);

        /// <summary>
        /// 是否有某个分析模型的区域断电配置信息
        /// </summary>
        /// <returns>是否有某个分析模型的区域断电配置信息</returns>
        BasicResponse<bool> HasRegionOutageForAnalysisModel(GetByAnalysisModelIdRequest getByAnalysisModelIdRequest);

        /// <summary>
        /// 获取所有区域断电配置列表
        /// </summary>
        /// <param name="getAllRegionOutageConfigRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_RegionOutageConfigInfo>> GetRegionOutageConfigAllList(GetAllRegionOutageConfigRequest getAllRegionOutageConfigRequest);
    }
}


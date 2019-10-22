using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRegionOutageConfigRepository : IRepository<JC_RegionoutageconfigModel>
    {
        /// <summary>
        /// 批量新增区域断点设置
        /// </summary>
        /// <param name="jC_RegionoutageconfigModel"></param>
        /// <returns></returns>
        List<JC_RegionoutageconfigModel> AddJC_RegionOutageConfigList(List<JC_RegionoutageconfigModel> jC_RegionoutageconfigModel);

        JC_RegionoutageconfigModel AddJC_Regionoutageconfig(JC_RegionoutageconfigModel jC_RegionoutageconfigModel);
        void UpdateJC_Regionoutageconfig(JC_RegionoutageconfigModel jC_RegionoutageconfigModel);
        void DeleteJC_Regionoutageconfig(string id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="analysisModelId">模板ID</param>
        void DeleteUserRoleByAnalysisModelId(string analysisModelId);
        IList<JC_RegionoutageconfigModel> GetJC_RegionoutageconfigList(int pageIndex, int pageSize, out int rowCount);
        JC_RegionoutageconfigModel GetJC_RegionoutageconfigById(string id);
        /// <summary>
        /// 获取和分析模型有关的区域断电配置信息
        /// </summary>
        /// <param name="analysisModelId">分析模型Id</param>
        /// <returns>分析模型有关的区域断电配置信息</returns>
        List<JC_RegionoutageconfigModel> GetRegionOutageConfigListByAnalysisModelId(string analysisModelId);

        /// <summary>
        /// 是否有某个分析模型的区域断电配置信息
        /// </summary>
        bool HasRegionOutageForAnalysisModel(string analysisModelId);
    }
}

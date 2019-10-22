using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ILargedataAnalysisConfigRepository : IRepository<JC_LargedataanalysisconfigModel>
    {
        JC_LargedataanalysisconfigModel AddJC_Largedataanalysisconfig(JC_LargedataanalysisconfigModel jC_LargedataanalysisconfigModel);
        void UpdateJC_Largedataanalysisconfig(JC_LargedataanalysisconfigModel jC_LargedataanalysisconfigModel);
        void DeleteJC_Largedataanalysisconfig(string id);
        IList<JC_LargedataanalysisconfigModel> GetJC_LargedataanalysisconfigList(int pageIndex, int pageSize, out int rowCount);
        JC_LargedataanalysisconfigModel GetJC_LargedataanalysisconfigById(string id);
        /// <summary>
        /// 获取所有分析配置列表
        /// </summary>
        /// <returns>所有分析配置列表</returns>
        IList<JC_LargedataanalysisconfigModel> GetAllLargeDataAnalysisConfigList();

        /// <summary>
        /// 获取所有已启用的分析配置列表
        /// </summary>
        /// <returns>所有已启用的分析配置列表</returns>
        IList<JC_LargedataanalysisconfigModel> GetAllEnabledLargeDataAnalysisConfigList();
        /// <summary>
        /// 根据模板ID查询分析模型配置信息
        /// </summary>
        /// <param name="templeteId">模板Id</param>
        /// <returns></returns>
        IList<JC_LargedataanalysisconfigModel> GetLargeDataAnalysisConfigListByTempleteId(string templeteId);
        /// <summary>
        ///根据模型名称模糊查询模型列表
        /// </summary>
        /// <returns>模型列表</returns>
        IList<JC_LargedataanalysisconfigModel> GetLargeDataAnalysisConfigListByName(string name, int pageIndex, int pageSize, out int rowCount);

        /// <summary>
        /// 获取没有关联报警通知的分析模型
        /// </summary>
        /// <returns>获取没有关联报警通知的分析模型</returns>
        List<JC_LargedataanalysisconfigModel> GetLargeDataAnalysisConfigWithoutAlarmConfigList();

        /// <summary>
        /// 获取没有关联应急联动的分析模型
        /// </summary>
        /// <returns>获取没有关联应急联动的分析模型</returns>
        List<JC_LargedataanalysisconfigModel> GetLargeDataAnalysisConfigWithoutRegionOutage();

        /// <summary>
        /// 通过分析模型名称获获取分析模型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        JC_LargedataanalysisconfigModel GetLargeDataAnalysisConfigByName(string name);

    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ISetAnalysisModelPointRecordRepository : IRepository<JC_SetanalysismodelpointrecordModel>
    {
        JC_SetanalysismodelpointrecordModel AddJC_Setanalysismodelpointrecord(JC_SetanalysismodelpointrecordModel jC_SetanalysismodelpointrecordModel);
        void UpdateJC_Setanalysismodelpointrecord(JC_SetanalysismodelpointrecordModel jC_SetanalysismodelpointrecordModel);
        void DeleteJC_Setanalysismodelpointrecord(string id);
        IList<JC_SetanalysismodelpointrecordModel> GetJC_SetanalysismodelpointrecordList(int pageIndex, int pageSize, out int rowCount);
        JC_SetanalysismodelpointrecordModel GetJC_SetanalysismodelpointrecordById(string id);

        /// <summary>
        /// 通过模板Id, 获取设置表达式测点的模板
        /// </summary>
        /// <param name="templeteId">模板Id</param>
        /// <returns>表达式测点关联的模板列表</returns>
        IList<JC_SetanalysismodelpointrecordModel> GetAnalysisModelPointBindingTemplateByTempleteId(string templeteId);

        /// <summary>
        /// 通过分析模型Id, 删除分析模型测点关联信息.
        /// </summary>
        /// <param name="id">分析模型Id</param>
        void DeleteAnalysisModelPointRecordByAnalysisModelId(string id);

        /// <summary>
        /// 获取分析模型对应的测点关联列表。
        /// </summary>
        /// <param name="id">分析模型Id</param>
        /// <returns>分析模型对应的测点关联列表</returns>
        IList<JC_SetanalysismodelpointrecordModel> GetAnalysisModelPointRecordsByAnalysisModelId(string id);
    }
}

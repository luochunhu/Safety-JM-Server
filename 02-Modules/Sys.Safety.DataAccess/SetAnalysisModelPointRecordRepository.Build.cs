using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;
using System.Data;

namespace Sys.Safety.DataAccess
{
    public partial class SetAnalysisModelPointRecordRepository : RepositoryBase<JC_SetanalysismodelpointrecordModel>, ISetAnalysisModelPointRecordRepository
    {

        public JC_SetanalysismodelpointrecordModel AddJC_Setanalysismodelpointrecord(JC_SetanalysismodelpointrecordModel jC_SetanalysismodelpointrecordModel)
        {
            return base.Insert(jC_SetanalysismodelpointrecordModel);
        }
        public void UpdateJC_Setanalysismodelpointrecord(JC_SetanalysismodelpointrecordModel jC_SetanalysismodelpointrecordModel)
        {
            base.Update(jC_SetanalysismodelpointrecordModel);
        }
        public void DeleteJC_Setanalysismodelpointrecord(string id)
        {
            base.Delete(id);
        }
        public IList<JC_SetanalysismodelpointrecordModel> GetJC_SetanalysismodelpointrecordList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_SetanalysismodelpointrecordModelLists = base.Datas;
            rowCount = jC_SetanalysismodelpointrecordModelLists.Count();
            return jC_SetanalysismodelpointrecordModelLists.OrderBy(t=>t.Id).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_SetanalysismodelpointrecordModel GetJC_SetanalysismodelpointrecordById(string id)
        {
            JC_SetanalysismodelpointrecordModel jC_SetanalysismodelpointrecordModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_SetanalysismodelpointrecordModel;
        }

        /// <summary>
        /// 通过模板Id, 获取设置表达式测点的模板
        /// </summary>
        /// <param name="templeteId">模板Id</param>
        /// <returns>表达式测点关联的模板列表</returns>
        public IList<JC_SetanalysismodelpointrecordModel> GetAnalysisModelPointBindingTemplateByTempleteId(string templeteId)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = base.QueryTable("dataAnalysis_GetAnalysisModelPointBindingTemplateByTempleteId", templeteId);
                return Basic.Framework.Common.ObjectConverter.Copy<JC_SetanalysismodelpointrecordModel>(dataTable);
            }
            catch
            {
                return new List<JC_SetanalysismodelpointrecordModel>();
            }
        }

        /// <summary>
        /// 通过分析模型Id, 删除分析模型测点关联信息.
        /// </summary>
        /// <param name="id">分析模型Id</param>
        public void DeleteAnalysisModelPointRecordByAnalysisModelId(string id)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == id);
            base.Delete(query);
        }

        /// <summary>
        /// 获取分析模型对应的测点关联列表。
        /// </summary>
        /// <param name="id">分析模型Id</param>
        /// <returns>分析模型对应的测点关联列表</returns>
        public IList<JC_SetanalysismodelpointrecordModel> GetAnalysisModelPointRecordsByAnalysisModelId(string id)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == id);

            return query.ToList();
        }
    }
}

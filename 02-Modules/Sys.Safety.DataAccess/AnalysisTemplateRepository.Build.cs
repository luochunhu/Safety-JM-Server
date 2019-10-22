using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AnalysisTemplateRepository : RepositoryBase<JC_AnalysistemplateModel>, IAnalysisTemplateRepository
    {

        public JC_AnalysistemplateModel AddJC_Analysistemplate(JC_AnalysistemplateModel jC_AnalysistemplateModel)
        {
            return base.Insert(jC_AnalysistemplateModel);
        }


        public void UpdateJC_Analysistemplate(JC_AnalysistemplateModel jC_AnalysistemplateModel)
        {
            base.Update(jC_AnalysistemplateModel);
        }
        public void DeleteJC_Analysistemplate(string id)
        {
            base.Delete(id);
        }
        public IList<JC_AnalysistemplateModel> GetJC_AnalysistemplateList(int pageIndex, int pageSize, out int rowCount)
        {
            try
            {
                var jC_AnalysistemplateModelLists = base.Datas;
                rowCount = jC_AnalysistemplateModelLists.Count();
                if (pageSize == 0)
                    return jC_AnalysistemplateModelLists.ToList();
                return jC_AnalysistemplateModelLists.OrderBy(t => t.Name).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            catch
            {
                rowCount = 0;
                return new List<JC_AnalysistemplateModel>();

            }

        }
        public JC_AnalysistemplateModel GetJC_AnalysistemplateById(string id)
        {
            JC_AnalysistemplateModel jC_AnalysistemplateModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_AnalysistemplateModel;
        }
        public JC_AnalysistemplateModel GetJC_AnalysistemplateByTempleteId(string templeteId)
        {
            JC_AnalysistemplateModel jC_AnalysistemplateModel = base.Datas.FirstOrDefault(c => c.Id == templeteId);
            return jC_AnalysistemplateModel;
        }
        public JC_AnalysistemplateModel GetJC_AnalysistemplateByName(string name)
        {
            JC_AnalysistemplateModel jC_AnalysistemplateModel = base.Datas.FirstOrDefault(c => c.Name == name);
            return jC_AnalysistemplateModel;
        }
        /// <summary>
        /// 根据name模糊查询模板列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<JC_AnalysistemplateModel> GetJC_AnalysistemplateListByName(string name, int pageIndex, int pageSize, out int rowCount)
        {
            try
            {
                IList<JC_AnalysistemplateModel> jC_AnalysistemplateModel = null;
                var query = base.Datas.AsQueryable();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(t => t.Name.IndexOf(name) >= 0);
                }
                var modelLists = query.ToList();
                rowCount = modelLists.Count();
                if (pageSize == 0)
                {//查询所有数据
                    jC_AnalysistemplateModel = modelLists.OrderByDescending(t => t.UpdatedTime).ToList();
                }
                else
                {
                    jC_AnalysistemplateModel = modelLists.OrderByDescending(t => t.UpdatedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }

                return jC_AnalysistemplateModel;
            }
            catch
            {
                rowCount = 0;
                return new List<JC_AnalysistemplateModel>();

            }
        }
    }
}

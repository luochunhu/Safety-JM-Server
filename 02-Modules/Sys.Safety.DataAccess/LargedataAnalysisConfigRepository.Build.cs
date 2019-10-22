using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;
using Sys.Safety.Enums.Enums;

namespace Sys.Safety.DataAccess
{
    public partial class LargedataAnalysisConfigRepository : RepositoryBase<JC_LargedataanalysisconfigModel>, ILargedataAnalysisConfigRepository
    {

        public JC_LargedataanalysisconfigModel AddJC_Largedataanalysisconfig(JC_LargedataanalysisconfigModel jC_LargedataanalysisconfigModel)
        {
            return base.Insert(jC_LargedataanalysisconfigModel);
        }
        public void UpdateJC_Largedataanalysisconfig(JC_LargedataanalysisconfigModel jC_LargedataanalysisconfigModel)
        {
            base.Update(jC_LargedataanalysisconfigModel);
        }

        /// <summary>
        /// 删除逻辑改为逻辑删除
        /// </summary>
        /// <param name="id">分析模型配置Id</param>
        public void DeleteJC_Largedataanalysisconfig(string id)
        {
            JC_LargedataanalysisconfigModel jC_LargedataanalysisconfigModel = GetJC_LargedataanalysisconfigById(id);
            if (jC_LargedataanalysisconfigModel != null)
            {
                jC_LargedataanalysisconfigModel.IsDeleted = DeleteState.Yes;
                UpdateJC_Largedataanalysisconfig(jC_LargedataanalysisconfigModel);
            }
        }
        public IList<JC_LargedataanalysisconfigModel> GetJC_LargedataanalysisconfigList(int pageIndex, int pageSize, out int rowCount)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.IsDeleted != DeleteState.Yes);
            rowCount = query.Count();
            return query.OrderBy(t => t.Name).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_LargedataanalysisconfigModel GetJC_LargedataanalysisconfigById(string id)
        {
            JC_LargedataanalysisconfigModel jC_LargedataanalysisconfigModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_LargedataanalysisconfigModel;
        }

        /// <summary>
        /// 获取所有分析配置列表
        /// </summary>
        /// <returns>所有分析配置列表</returns>
        public IList<JC_LargedataanalysisconfigModel> GetAllLargeDataAnalysisConfigList()
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.IsDeleted != DeleteState.Yes);
            return query.ToList();
        }
        /// <summary>
        ///根据模型名称模糊查询模型列表
        /// </summary>
        /// <returns>模型列表</returns>
        public IList<JC_LargedataanalysisconfigModel> GetLargeDataAnalysisConfigListByName(string name, int pageIndex, int pageSize, out int rowCount)
        {
            try
            {
                IList<JC_LargedataanalysisconfigModel> model = null;
                var query = base.Datas.AsQueryable();
                if (string.IsNullOrWhiteSpace(name.Trim()))
                {
                    query = query.Where(q => q.IsDeleted != DeleteState.Yes);
                }
                else
                {
                    query = query.Where(q => q.IsDeleted != DeleteState.Yes && q.Name.IndexOf(name) >= 0);
                }
                var modelLists = query.ToList();
                rowCount = modelLists.Count();
                if (pageSize == 0)
                {//查询所有数据
                    model = modelLists.OrderByDescending(t => t.UpdatedTime).ToList();
                }
                else
                {
                    model = modelLists.OrderByDescending(t => t.UpdatedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                return model;
            }
            catch
            {
                rowCount = 0;
                return new List<JC_LargedataanalysisconfigModel>();

            }
        }
        /// <summary>
        /// 根据模板ID查询分析模型配置信息
        /// </summary>
        /// <param name="templeteId">模板Id</param>
        /// <returns></returns>
        public IList<JC_LargedataanalysisconfigModel> GetLargeDataAnalysisConfigListByTempleteId(string templeteId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.TempleteId == templeteId && q.IsDeleted != DeleteState.Yes);
            return query.ToList();
        }

        /// <summary>
        /// 获取所有已启用的分析配置列表
        /// </summary>
        /// <returns>所有已启用的分析配置列表</returns>
        public IList<JC_LargedataanalysisconfigModel> GetAllEnabledLargeDataAnalysisConfigList()
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.IsDeleted != DeleteState.Yes && q.IsEnabled == EnableState.Yes);
            return query.ToList();
        }

        /// <summary>
        /// 获取没有关联报警通知的分析模型
        /// </summary>
        /// <returns>获取没有关联报警通知的分析模型</returns>
        public List<JC_LargedataanalysisconfigModel> GetLargeDataAnalysisConfigWithoutAlarmConfigList()
        {
            System.Data.DataTable dtResult = base.QueryTable("GetLargeDataAnalysisConfigWithoutAlarmConfigList");
            return Basic.Framework.Common.ObjectConverter.Copy<JC_LargedataanalysisconfigModel>(dtResult);
        }

        /// <summary>
        /// 获取没有关联应急联动的分析模型
        /// </summary>
        /// <returns>获取没有关联应急联动的分析模型</returns>
        public List<JC_LargedataanalysisconfigModel> GetLargeDataAnalysisConfigWithoutRegionOutage()
        {
            System.Data.DataTable dtResult = base.QueryTable("GetLargeDataAnalysisConfigWithoutRegionOutage");
            return Basic.Framework.Common.ObjectConverter.Copy<JC_LargedataanalysisconfigModel>(dtResult);
        }

        public JC_LargedataanalysisconfigModel GetLargeDataAnalysisConfigByName(string name)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.IsDeleted != DeleteState.Yes && q.IsEnabled == EnableState.Yes);
            return query.FirstOrDefault(q => q.Name == name);
        }
    }
}

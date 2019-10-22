using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class LargedatAanalysisLogRepository : RepositoryBase<JC_LargedataanalysislogModel>, ILargedataAnalysisLogRepository
    {

        public JC_LargedataanalysislogModel AddJC_Largedataanalysislog(JC_LargedataanalysislogModel jC_LargedataanalysislogModel)
        {
            return base.Insert(jC_LargedataanalysislogModel);
        }
        public void UpdateJC_Largedataanalysislog(JC_LargedataanalysislogModel jC_LargedataanalysislogModel)
        {
            base.Update(jC_LargedataanalysislogModel);
        }
        public void DeleteJC_Largedataanalysislog(string id)
        {
            base.Delete(id);
        }
        public IList<JC_LargedataanalysislogModel> GetJC_LargedataanalysislogList(int pageIndex, int pageSize, out int rowCount)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q=>q.IsDeleted != Sys.Safety.Enums.Enums.DeleteState.Yes);
            rowCount = query.Count();
            return query.OrderBy(t=>t.Name).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_LargedataanalysislogModel GetJC_LargedataanalysislogById(string id)
        {
            JC_LargedataanalysislogModel jC_LargedataanalysislogModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_LargedataanalysislogModel;
        }

        public IList<JC_LargedataanalysislogModel> GetLargedataAnalysisLogListByAnalysisModelId(string analysisModelId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == analysisModelId && q.IsDeleted != Enums.Enums.DeleteState.Yes);
            return query.ToList();
        }

        public IList<JC_LargedataanalysislogModel> GetLargedataAnalysisLogListByModelIdAndTime(string analysisModelId, DateTime analysisDate)
        {
            var query = base.Datas.AsQueryable();
            if (!string.IsNullOrEmpty(analysisModelId))
                query = query.Where(q => q.AnalysisModelId == analysisModelId );
            if (analysisDate != null)
            {
                DateTime startTime = new DateTime(analysisDate.Year, analysisDate.Month, analysisDate.Day, 0, 0, 0);
                DateTime endTime = new DateTime(analysisDate.Year, analysisDate.Month, analysisDate.Day, 23, 59, 59);
                query = query.Where(q => q.AnalysisTime >= startTime && q.AnalysisTime <= endTime);

            }
            return query.ToList();
        }

        public JC_LargedataanalysislogModel GetLargedataAnalysisLogLatestByAnalysisModelId(string analysisModelId)
        {
            JC_LargedataanalysislogModel jC_LargedataanalysislogModel = base.Datas.OrderByDescending(o=>o.AnalysisTime).FirstOrDefault(q => q.AnalysisModelId == analysisModelId);
            return jC_LargedataanalysislogModel;
        }
    }
}

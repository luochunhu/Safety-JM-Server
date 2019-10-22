using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AlarmHandleRepository : RepositoryBase<JC_AlarmHandleModel>, IAlarmHandleRepository
    {

        public JC_AlarmHandleModel AddJC_AlarmHandle(JC_AlarmHandleModel jC_AlarmHandleModel)
        {
            return base.Insert(jC_AlarmHandleModel);
        }
        public void UpdateJC_AlarmHandle(JC_AlarmHandleModel jC_AlarmHandleModel)
        {
            base.Update(jC_AlarmHandleModel);
        }
        public void DeleteJC_AlarmHandle(string id)
        {
            base.Delete(id);
        }
        public IList<JC_AlarmHandleModel> GetJC_AlarmHandleList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_AlarmHandleModelLists = base.Datas;
            rowCount = jC_AlarmHandleModelLists.Count();
            return jC_AlarmHandleModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_AlarmHandleModel GetJC_AlarmHandleById(string id)
        {
            JC_AlarmHandleModel jC_AlarmHandleModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_AlarmHandleModel;
        }

        public List<JC_AlarmHandleModel> GetUnclosedAlarmList()
        {
            DateTime noneEndTime = new DateTime(1900, 1, 1, 0, 0, 0);
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.EndTime.Equals(noneEndTime));
            return query.ToList();
        }

        public JC_AlarmHandleModel GetUnclosedAlarmByAnalysisModelId(string analysisModelId)
        {
            DateTime noneEndTime = new DateTime(1900, 1, 1, 0, 0, 0);
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == analysisModelId && q.EndTime.Equals(noneEndTime));
            return query.OrderByDescending(q => q.StartTime).FirstOrDefault();
        }

        public List<JC_AlarmHandleModel> GetUnhandledAlarmList()
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => !q.HandlingTime.HasValue);
            return query.ToList();
        }

        public JC_AlarmHandleModel GetUnhandledAlarmByAnalysisModelId(string analysisModelId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == analysisModelId && !q.HandlingTime.HasValue);
            return query.FirstOrDefault();
        }

        public void UpdateAlarmHandleList(IList<JC_AlarmHandleModel> alarmHandleModelList)
        {
            base.Update(alarmHandleModelList);
        }

        public List<JC_AlarmHandleNoEndModel> GetAlarmHandleNoEndListByCondition(string startTime, DateTime? endTime, string personId)
        {
            var jC_AlarmHandleModelLists = new List<JC_AlarmHandleNoEndModel>();
            if (endTime.HasValue && !string.IsNullOrWhiteSpace(personId))
            {
                var datatable = base.QueryTable("global_RealModule_GetAlarmHandleNoEndListByCondition", startTime, endTime.Value, personId);
                jC_AlarmHandleModelLists = base.ToEntityFromTable<JC_AlarmHandleNoEndModel>(datatable).ToList();
            }
            return jC_AlarmHandleModelLists;
        }

        public List<JC_AlarmHandleModel> GetUnclosedAlarmListByAnalysisModelId(string analysisModelId)
        {
            DateTime noneEndTime = new DateTime(1900, 1, 1, 0, 0, 0);
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == analysisModelId && q.EndTime.Equals(noneEndTime));
            return query.OrderByDescending(q => q.StartTime).ToList();
        }
    }
}

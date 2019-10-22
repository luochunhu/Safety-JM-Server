using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AlarmNotificationPersonnelConfigRepository : RepositoryBase<JC_AlarmnotificationpersonnelconfigModel>, IAlarmNotificationPersonnelConfigRepository
    {

        public JC_AlarmnotificationpersonnelconfigModel AddJC_AlarmNotificationPersonnelConfig(JC_AlarmnotificationpersonnelconfigModel jC_AlarmnotificationpersonnelconfigModel)
        {
            return base.Insert(jC_AlarmnotificationpersonnelconfigModel);
        }
        public void UpdateJC_AlarmNotificationPersonnelConfig(JC_AlarmnotificationpersonnelconfigModel jC_AlarmnotificationpersonnelconfigModel)
        {
            base.Update(jC_AlarmnotificationpersonnelconfigModel);
        }
        public void DeleteJC_AlarmNotificationPersonnelConfig(string id)
        {
            base.Delete(id);
        }
        public IList<JC_AlarmnotificationpersonnelconfigModel> GetJC_AlarmNotificationPersonnelConfigList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_AlarmnotificationpersonnelconfigModelLists = base.Datas;
            rowCount = jC_AlarmnotificationpersonnelconfigModelLists.Count();
            return jC_AlarmnotificationpersonnelconfigModelLists.OrderByDescending(t => t.UpdatedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_AlarmnotificationpersonnelconfigModel GetJC_AlarmNotificationPersonnelConfigById(string id)
        {
            JC_AlarmnotificationpersonnelconfigModel jC_AlarmnotificationpersonnelconfigModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_AlarmnotificationpersonnelconfigModel;
        }

        public IList<JC_AlarmnotificationpersonnelconfigModel> GetAlarmNotificationPersonnelConfigByAnalysisModelId(string analysisModelId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == analysisModelId);

            return query.ToList();
        }

        public bool HasAlarmNotificationForAnalysisModel(string analysisModelId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AnalysisModelId == analysisModelId);
            return query.Any();
        }
    }
}

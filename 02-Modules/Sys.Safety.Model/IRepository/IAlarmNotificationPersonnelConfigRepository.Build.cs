using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAlarmNotificationPersonnelConfigRepository : IRepository<JC_AlarmnotificationpersonnelconfigModel>
    {
        JC_AlarmnotificationpersonnelconfigModel AddJC_AlarmNotificationPersonnelConfig(JC_AlarmnotificationpersonnelconfigModel jC_AlarmnotificationpersonnelconfigModel);
        void UpdateJC_AlarmNotificationPersonnelConfig(JC_AlarmnotificationpersonnelconfigModel jC_AlarmnotificationpersonnelconfigModel);
        void DeleteJC_AlarmNotificationPersonnelConfig(string id);
        IList<JC_AlarmnotificationpersonnelconfigModel> GetJC_AlarmNotificationPersonnelConfigList(int pageIndex, int pageSize, out int rowCount);
        JC_AlarmnotificationpersonnelconfigModel GetJC_AlarmNotificationPersonnelConfigById(string id);

        IList<JC_AlarmnotificationpersonnelconfigModel> GetAlarmNotificationPersonnelConfigByAnalysisModelId(string analysisModelId);
        /// <summary>
        /// 是否有某个分析模型的报警通知配置信息
        /// </summary>
        bool HasAlarmNotificationForAnalysisModel(string analysisModelId);
    }
}

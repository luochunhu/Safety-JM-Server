using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.AlarmNotificationPersonnelConfig;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IAlarmNotificationPersonnelConfigService
    {
        BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> AddJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigAddRequest jC_Alarmnotificationpersonnelconfigrequest);
        BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> UpdateJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigUpdateRequest jC_Alarmnotificationpersonnelconfigrequest);
        BasicResponse DeleteJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigDeleteRequest jC_Alarmnotificationpersonnelconfigrequest);
        BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetJC_AlarmNotificationPersonnelConfigList(AlarmNotificationPersonnelConfigGetListRequest jC_Alarmnotificationpersonnelconfigrequest);
        BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> GetJC_AlarmNotificationPersonnelConfigById(AlarmNotificationPersonnelConfigGetRequest jC_Alarmnotificationpersonnelconfigrequest);

        BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelConfigByAnalysisModelId(AlarmNotificationPersonnelConfigGetListRequest getListByAnalysisModelIdRequest);
          /// <summary>
        /// 根据模型名称模糊查询报警推送配置信息
        /// </summary>
        /// <param name="getListByAnalysisModelIdRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelListByAnalysisModeName(AlarmNotificationPersonnelConfigGetListRequest getListByAnalysisModelIdRequest);
        /// <summary>
        /// 是否有某个分析模型的报警通知配置信息
        /// </summary>
        /// <returns>是否有某个分析模型的报警通知配置信息</returns>
        BasicResponse<bool> HasAlarmNotificationForAnalysisModel(GetAlarmNotificationByAnalysisModelIdRequest getByAnalysisModelIdRequest);

        /// <summary>
        /// 获取所有报警通知人员配置列表
        /// </summary>
        /// <param name="getAllAlarmNotificationRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelConfigAllList(GetAllAlarmNotificationRequest getAllAlarmNotificationRequest);

        /// <summary>
        /// 批量添加报警通知人员配置
        /// </summary>
        /// <param name="addRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> AddAlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigListAddRequest addRequest);
    }
}


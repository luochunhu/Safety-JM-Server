using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.AlarmNotificationPersonnel;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IAlarmNotificationPersonnelService
    {
        BasicResponse<JC_AlarmNotificationPersonnelInfo> AddJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelAddRequest jC_AlarmNotificationPersonnelrequest);
        BasicResponse<JC_AlarmNotificationPersonnelInfo> UpdateJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelUpdateRequest jC_AlarmNotificationPersonnelrequest);
        BasicResponse DeleteJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelDeleteRequest jC_AlarmNotificationPersonnelrequest);
        BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> GetJC_AlarmNotificationPersonnelList(AlarmNotificationPersonnelGetListRequest jC_AlarmNotificationPersonnelrequest);
        BasicResponse<JC_AlarmNotificationPersonnelInfo> GetJC_AlarmNotificationPersonnelById(AlarmNotificationPersonnelGetRequest jC_AlarmNotificationPersonnelrequest);
        /// <summary>
        /// 获取报警配置相关的人员信息
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest">获取报警配置相关的人员信息请求对象</param>
        /// <returns>报警配置相关的人员信息</returns>
        BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> GetJC_AlarmNotificationPersonnelListByAlarmConfigId(AlarmNotificationPersonnelGetListByAlarmConfigIdRequest jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest);
        /// <summary>
        /// 根据模型ID查询报警推送人员配置信息 
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelrequest"></param>
        /// <returns>报警配置相关的人员信息</returns>
        BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> GetAlarmNotificationPersonnelByAnalysisModelId(AlarmNotificationPersonnelGetListByAlarmConfigIdRequest jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest);
          /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelrequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> AddJC_AlarmNotificationPersonnelList(AlarmNotificationPersonnelAddRequest jC_AlarmNotificationPersonnelrequest);
    }
}


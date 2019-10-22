using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAlarmNotificationPersonnelRepository : IRepository<JC_AlarmNotificationPersonnelModel>
    {
        JC_AlarmNotificationPersonnelModel AddJC_AlarmNotificationPersonnel(JC_AlarmNotificationPersonnelModel jC_AlarmNotificationPersonnelModel);
        void UpdateJC_AlarmNotificationPersonnel(JC_AlarmNotificationPersonnelModel jC_AlarmNotificationPersonnelModel);
        void DeleteJC_AlarmNotificationPersonnel(string id);

        /// <summary>
        /// 删除报警配置相关的人员信息。
        /// </summary>
        /// <param name="alarmConfigId">报警配置Id</param>
        void DeleteJC_AlarmNotificationPersonnelByAlarmConfigId(string alarmConfigId);

        IList<JC_AlarmNotificationPersonnelModel> GetJC_AlarmNotificationPersonnelList(int pageIndex, int pageSize, out int rowCount);

        /// <summary>
        /// 获取报警配置相关的人员信息
        /// </summary>
        /// <param name="alarmConfigId">报警配置Id</param>
        /// <returns>报警配置相关的人员信息</returns>
        IList<JC_AlarmNotificationPersonnelModel> GetJC_AlarmNotificationPersonnelListByAlarmConfigId(string alarmConfigId);

        JC_AlarmNotificationPersonnelModel GetJC_AlarmNotificationPersonnelById(string id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelModelList"></param>
        void DeleteJC_AlarmNotificationPersonnelList(List<JC_AlarmNotificationPersonnelModel> jC_AlarmNotificationPersonnelModelList);
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelModelList"></param>
        /// <returns></returns>
        List<JC_AlarmNotificationPersonnelModel> AddJC_AlarmNotificationPersonnelList(List<JC_AlarmNotificationPersonnelModel> jC_AlarmNotificationPersonnelModelList);
    }
}

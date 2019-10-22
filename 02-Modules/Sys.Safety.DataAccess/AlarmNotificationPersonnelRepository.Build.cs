using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AlarmNotificationPersonnelRepository : RepositoryBase<JC_AlarmNotificationPersonnelModel>, IAlarmNotificationPersonnelRepository
    {

        public JC_AlarmNotificationPersonnelModel AddJC_AlarmNotificationPersonnel(JC_AlarmNotificationPersonnelModel jC_AlarmNotificationPersonnelModel)
        {
            return base.Insert(jC_AlarmNotificationPersonnelModel);
        }

        public void UpdateJC_AlarmNotificationPersonnel(JC_AlarmNotificationPersonnelModel jC_AlarmNotificationPersonnelModel)
        {
            base.Update(jC_AlarmNotificationPersonnelModel);
        }
        public void DeleteJC_AlarmNotificationPersonnel(string id)
        {
            base.Delete(id);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelModelList"></param>
        public void DeleteJC_AlarmNotificationPersonnelList(List<JC_AlarmNotificationPersonnelModel> jC_AlarmNotificationPersonnelModelList)
        {
            base.Delete(jC_AlarmNotificationPersonnelModelList);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelModelList"></param>
        /// <returns></returns>
        public List<JC_AlarmNotificationPersonnelModel> AddJC_AlarmNotificationPersonnelList(List<JC_AlarmNotificationPersonnelModel> jC_AlarmNotificationPersonnelModelList)
        {
            return base.Insert(jC_AlarmNotificationPersonnelModelList).ToList();
        }

        public IList<JC_AlarmNotificationPersonnelModel> GetJC_AlarmNotificationPersonnelList(int pageIndex, int pageSize, out int rowCount)
        {
            var jC_AlarmNotificationPersonnelModelLists = base.Datas;
            rowCount = jC_AlarmNotificationPersonnelModelLists.Count();
            return jC_AlarmNotificationPersonnelModelLists.OrderByDescending(t=>t.AlarmConfigId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_AlarmNotificationPersonnelModel GetJC_AlarmNotificationPersonnelById(string id)
        {
            JC_AlarmNotificationPersonnelModel jC_AlarmNotificationPersonnelModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_AlarmNotificationPersonnelModel;
        }

        /// <summary>
        /// 删除报警配置相关的人员信息。
        /// </summary>
        /// <param name="alarmConfigId">报警配置Id</param>
        public void DeleteJC_AlarmNotificationPersonnelByAlarmConfigId(string alarmConfigId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AlarmConfigId == alarmConfigId);
            base.Delete(query);
        }

        /// <summary>
        /// 获取报警配置相关的人员信息
        /// </summary>
        /// <param name="alarmConfigId">报警配置Id</param>
        /// <returns>报警配置相关的人员信息</returns>
        public IList<JC_AlarmNotificationPersonnelModel> GetJC_AlarmNotificationPersonnelListByAlarmConfigId(string alarmConfigId)
        {
            var query = base.Datas.AsQueryable();
            query = query.Where(q => q.AlarmConfigId == alarmConfigId);
            return query.ToList();
        }
    }
}

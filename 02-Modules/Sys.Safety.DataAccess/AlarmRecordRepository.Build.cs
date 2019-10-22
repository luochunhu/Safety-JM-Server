using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;
using System.Data;
using Basic.Framework.Logging;

namespace Sys.Safety.DataAccess
{
    public partial class AlarmRecordRepository : RepositoryBase<Jc_BModel>, IAlarmRecordRepository
    {

        public Jc_BModel AddAlarmRecord(Jc_BModel jc_BModel)
        {
            return base.Insert(jc_BModel);
        }
        public void UpdateAlarmRecord(Jc_BModel jc_BModel)
        {
            base.Update(jc_BModel);
        }
        public void DeleteAlarmRecord(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_BModel> GetAlarmRecordList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_BModelLists = base.Datas;
            rowCount = jc_BModelLists.Count();
            return jc_BModelLists.OrderBy(p => p.Stime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// 获取当前正在报警的数据
        /// </summary>
        /// <returns></returns>
        public List<Jc_BModel> GetAlarmedDataList()
        {
            try
            {
                DataTable AlarmedDataList = base.QueryTable("global_AlarmModelService_GetAlarmDataList", DateTime.Now.ToString("yyyyMM"));
                return Basic.Framework.Common.ObjectConverter.Copy<Jc_BModel>(AlarmedDataList);
            }
            catch(Exception ex)
            {
                LogHelper.Error("获取当前正在报警的数据失败：" + "\r\n" + ex.Message);
                return null;
            }

        }
        /// <summary>
        /// 查找当前需要处理的主控点
        /// </summary>
        /// <param name="QueryTable">表名称</param>
        /// <returns></returns>
        public DataTable GetAlarmFeedingList(string QueryTable, string stime, string etime)
        {
            DataTable RequestTable = new DataTable();
            RequestTable = base.QueryTable("global_AlarmRecordRepository_FindAlarmRecordByMaster", QueryTable, stime, etime);
            return RequestTable;
        }
        /// <summary>
        /// 查找对应主控点的控制记录【结束时间有flag=true,无结束时间flag=false】
        /// </summary>
        /// <param name="queryTable">表名称</param>
        /// <param name="etime">主控的结束时间</param>
        /// <param name="kzklst">控制口列表以'001C001','001C002'表示</param>
        /// <param name="stime">主控的开始时间</param>
        /// <returns></returns>
        public DataTable GetAlarmFeedingControlList(string queryTable, string stime, string etime, string kzklst, bool flag = true)
        {
            DataTable RequestTable = new DataTable();
            if (flag)
                RequestTable = base.QueryTable("global_AlarmRecordRepository_FindAlarmRecordByControl", queryTable, etime, stime, kzklst);
            else
                RequestTable = base.QueryTable("global_AlarmRecordRepository_FindAlarmRecordByControlNotime", queryTable, stime, kzklst);
            return RequestTable;
        }
        public Jc_BModel GetAlarmRecordById(string id)
        {
            Jc_BModel jc_BModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_BModel;
        }
    }
}

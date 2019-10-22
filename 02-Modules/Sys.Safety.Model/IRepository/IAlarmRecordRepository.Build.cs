using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data;

namespace Sys.Safety.Model
{
    public interface IAlarmRecordRepository : IRepository<Jc_BModel>
    {
        Jc_BModel AddAlarmRecord(Jc_BModel jc_BModel);
        void UpdateAlarmRecord(Jc_BModel jc_BModel);
        void DeleteAlarmRecord(string id);
        IList<Jc_BModel> GetAlarmRecordList(int pageIndex, int pageSize, out int rowCount);
        Jc_BModel GetAlarmRecordById(string id);
        /// <summary>
        /// 获取当前正在报警的数据
        /// </summary>
        /// <returns></returns>
        List<Jc_BModel> GetAlarmedDataList();
        DataTable GetAlarmFeedingList(string QueryTable, string stime, string etime);
        DataTable GetAlarmFeedingControlList(string queryTable, string stime, string etime, string kzklst, bool flag = true);
    }
}

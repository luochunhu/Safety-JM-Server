using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.LocalDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:报警数据补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AlarmLocalDataToDbTask : LocalDataToDbManager<Jc_BInfo>
    {
        private static volatile AlarmLocalDataToDbTask _instance;
        public static AlarmLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AlarmLocalDataToDbTask(1000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IAlarmRecordRepository alarmRepository;

        string[] columns = new string[] {
                    "ID",
                    "pointID",
                    "fzh",
                    "kh",
                    "dzh",
                    "devid",
                    "wzid",
                    "point",
                    "type",
                    "state",
                    "stime",
                    "etime",
                    "ssz",
                    "zdz",
                    "pjz",
                    "zdzs",
                    "cs",
                    "kzk",
                    "kdid",
                    "isalarm",
                    "upflag",
                    "remark",
                    "Bz1",
                    "Bz2",
                    "Bz3",
                    "Bz4",
                    "Bz5"

                };

        private AlarmLocalDataToDbTask(int interval)
            : base("报警补录任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\Alarm\\";
            alarmRepository = ServiceFactory.Create<IAlarmRecordRepository>();
        }

        protected override bool AddLocalDataToDb(List<Jc_BInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                var key = addLocalItems[0].Stime.ToString("yyyyMM");
                var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(addLocalItems);
                return alarmRepository.BulkCopy("KJ_DataAlarm" + key, alarmModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<Jc_BInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                var key = updateLocalItems[0].Stime.ToString("yyyyMM");
                var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(updateLocalItems);
                return alarmRepository.BulkUpdate("KJ_DataAlarm" + key, alarmModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

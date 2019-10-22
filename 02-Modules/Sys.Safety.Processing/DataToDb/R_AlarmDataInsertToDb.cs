using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.DataToDb
{
    public class R_AlarmDataInsertToDb : DataToDbManager<R_BInfo>
    {
        public static DataToDbManager<R_BInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new R_AlarmDataInsertToDb();
                        }
                    }
                }
                return _instance;
            }
        }

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

        private IAlarmRecordRepository alarmRepository;

        public R_AlarmDataInsertToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\R_Alarm\\";
            alarmRepository = ServiceFactory.Create<IAlarmRecordRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "R_B_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override bool AddItemsToDb(List<R_BInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_BInfo>> groupYYYYMM = addItems.GroupBy(p => p.Stime.ToString("yyyyMM"));
                foreach (IGrouping<string, R_BInfo> info in groupYYYYMM)
                {
                    var alarminfolist = info.ToList();
                    var alarmModels = ObjectConverter.CopyList<R_BInfo, Jc_BModel>(alarminfolist);
                    if (!alarmRepository.BulkCopy("PE_DataAlarm" + info.Key, alarmModels, BuildDataColumn(columns)))
                    {
                        int isconn = alarmRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            if (alarminfolist.Any())
                            {
                                AddDataToLocal(alarminfolist);
                            }
                            else 
                            {
                                LogHelper.Error("报警记录分组为空,需要分析原因。 key 为：" + info.Key + "数据为：" + JSONHelper.ToJSONString(addItems));
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("报警数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<R_BInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_BInfo>> groupYYYYMM = updateItems.GroupBy(p => p.Stime.ToString("yyyyMM"));
                foreach (IGrouping<string, R_BInfo> info in groupYYYYMM)
                {
                    var alarminfolist = info.ToList();
                    var alarmModels = ObjectConverter.CopyList<R_BInfo, Jc_BModel>(alarminfolist);
                    if (!alarmRepository.BulkUpdate("PE_DataAlarm" + info.Key, alarmModels, BuildDataColumn(columns), "ID"))
                    {
                        int isconn = alarmRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            if (alarminfolist.Any())
                            {
                                AddDataToLocal(alarminfolist);
                            }
                            else 
                            {
                                LogHelper.Error("报警记录分组为空,需要分析原因。 key 为：" + info.Key + "数据为：" + JSONHelper.ToJSONString(updateItems));
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("报警数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<R_BInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.Bz4 = "2");
                var key = addLocalItems[0].Stime.ToString("yyyyMM");
                var alarmModels = ObjectConverter.CopyList<R_BInfo, Jc_BModel>(addLocalItems);
                return alarmRepository.BulkCopy("PE_DataAlarm" + key, alarmModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<R_BInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.Bz4 = "2");
                var key = updateLocalItems[0].Stime.ToString("yyyyMM");
                var alarmModels = ObjectConverter.CopyList<R_BInfo, Jc_BModel>(updateLocalItems);
                return alarmRepository.BulkUpdate("PE_DataAlarm" + key, alarmModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.ProcessDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:报警数据入库线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AlarmDataToDbTask : ProcessDataToDbManager<Jc_BInfo>
    {
        private static volatile AlarmDataToDbTask _instance;
        public static AlarmDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AlarmDataToDbTask(1000);
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

        private AlarmDataToDbTask(int interval)
            : base("报警数据入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\Alarm\\";
            alarmRepository = ServiceFactory.Create<IAlarmRecordRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_DataAlarm_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override void AddItemsToDb(List<Jc_BInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_BInfo>> groupYYYYMM = addItems.GroupBy(p => p.Stime.ToString("yyyyMM"));
                foreach (IGrouping<string, Jc_BInfo> info in groupYYYYMM)
                {
                    var alarminfolist = info.ToList();
                    var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(alarminfolist);
                    if (!alarmRepository.BulkCopy("KJ_DataAlarm" + info.Key, alarmModels, BuildDataColumn(columns)))
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("报警数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override void UpdateItemsToDb(List<Jc_BInfo> updateItems)
        {
                        try
            {
                IEnumerable<IGrouping<string, Jc_BInfo>> groupYYYYMM = updateItems.GroupBy(p => p.Stime.ToString("yyyyMM"));
                foreach (IGrouping<string, Jc_BInfo> info in groupYYYYMM)
                {
                    var alarminfolist = info.ToList();
                    var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(alarminfolist);
                    if (!alarmRepository.BulkUpdate("KJ_DataAlarm" + info.Key, alarmModels, BuildDataColumn(columns), "ID"))
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("报警数据更新失败：" + "\r\n" + ex.Message);
            }
        }
    }
}

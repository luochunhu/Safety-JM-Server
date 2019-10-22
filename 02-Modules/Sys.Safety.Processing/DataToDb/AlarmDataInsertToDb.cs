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
    public class AlarmDataInsertToDb : DataToDbManager<Jc_BInfo>
    {
        public static DataToDbManager<Jc_BInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AlarmDataInsertToDb();
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

        public AlarmDataInsertToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\Alarm\\";
            alarmRepository = ServiceFactory.Create<IAlarmRecordRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_DataAlarm_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override bool AddItemsToDb(List<Jc_BInfo> addItems)
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
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("报警数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<Jc_BInfo> updateItems)
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

                    //查找报警记录中是否存在开始时间和结束时间一样的记录，有则删除
                    foreach (Jc_BInfo jcbinfo in alarminfolist)
                    {
                        if (jcbinfo.Stime.ToString("yyyy-MM-dd HH:mm:ss") == jcbinfo.Etime.ToString("yyyy-MM-dd HH:mm:ss"))
                        {
                            LogHelper.Warn("检测到无用报警记录，进行清除处理，报警测点号：" + jcbinfo.Point + "，开始时间：" + jcbinfo.Stime + "，结束时间：" + jcbinfo.Etime);
                            var alarmmodel = ObjectConverter.Copy<Jc_BInfo, Jc_BModel>(jcbinfo);
                            alarmRepository.Delete(alarmmodel.ID);
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

        protected override bool AddLocalDataToDb(List<Jc_BInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                //addLocalItems.ForEach(o => o.Bz4 = "2");
                addLocalItems.ForEach(o => o.Upflag = "2");//2018.2.27 by 
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
                //updateLocalItems.ForEach(o => o.Bz4 = "2");
                updateLocalItems.ForEach(o => o.Upflag = "2");//2018.2.27 by 
                var key = updateLocalItems[0].Stime.ToString("yyyyMM");
                var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(updateLocalItems);
                return alarmRepository.BulkUpdate("KJ_DataAlarm" + key, alarmModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

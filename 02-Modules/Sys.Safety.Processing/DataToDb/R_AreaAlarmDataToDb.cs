using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.DataToDb
{
    public class R_AreaAlarmDataToDb : DataToDbManager<R_AreaAlarmInfo>
    {
        public static DataToDbManager<R_AreaAlarmInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new R_AreaAlarmDataToDb();
                        }
                    }
                }
                return _instance;
            }
        }

        string[] columns = new string[] {
                    "Id",
                    "Hjlx",
                    "Bh",
                    "Yid",
                    "PointId",
                    "CallTime",
                    "Tsycs",
                    "Type",
                    "Card",
                    "Username",
                    "IP",
                    "Timer",
                    "Flag",
                    "Sysflag",
                    "By1",
                    "By2",
                    "By3",
                    "By4",
                    "By5"
                };

        private readonly IR_AreaAlarmRepository r_AreaAlarmRepositoty;

        public R_AreaAlarmDataToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\RAreaAlarm\\";
            r_AreaAlarmRepositoty = ServiceFactory.Create<IR_AreaAlarmRepository>();
        }

        protected override bool AddItemsToDb(List<R_AreaAlarmInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_AreaAlarmInfo>> groupYYYYMMDD = addItems.GroupBy(p => p.Stime.ToString("yyyyMM"));
                foreach (IGrouping<string, R_AreaAlarmInfo> info in groupYYYYMMDD)
                {
                    var phjinfolist = info.ToList();
                    var phjModels = ObjectConverter.CopyList<R_AreaAlarmInfo, R_AreaAlarmModel>(phjinfolist);
                    if (!r_AreaAlarmRepositoty.BulkCopy("PE_AreaAlarm" + info.Key, phjModels, BuildDataColumn(columns)))
                    {
                        int isconn = r_AreaAlarmRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(phjinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("区域报警数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<R_AreaAlarmInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_AreaAlarmInfo>> groupYYYYMMDD = updateItems.GroupBy(p => p.Stime.ToString("yyyyMM"));
                foreach (IGrouping<string, R_AreaAlarmInfo> info in groupYYYYMMDD)
                {
                    var phjinfolist = info.ToList();
                    var phjModels = ObjectConverter.CopyList<R_AreaAlarmInfo, R_AreaAlarmModel>(phjinfolist);
                    if (!r_AreaAlarmRepositoty.BulkUpdate("PE_AreaAlarm" + info.Key, phjModels, BuildDataColumn(columns), "Id"))
                    {
                        int isconn = r_AreaAlarmRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(phjinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("区域报警数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<R_AreaAlarmInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.Bz4 = "2");

                var key = addLocalItems[0].Stime.ToString("yyyyMM");
                var phjModels = ObjectConverter.CopyList<R_AreaAlarmInfo, R_AreaAlarmModel>(addLocalItems);
                return r_AreaAlarmRepositoty.BulkCopy("PE_AreaAlarm" + key, phjModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<R_AreaAlarmInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.Bz4 = "2");

                var key = updateLocalItems[0].Stime.ToString("yyyyMM");
                var phjModels = ObjectConverter.CopyList<R_AreaAlarmInfo, R_AreaAlarmModel>(updateLocalItems);
                return r_AreaAlarmRepositoty.BulkUpdate("PE_AreaAlarm" + key, phjModels, BuildDataColumn(columns), "Id");
            }
            return true;
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "PE_AreaAlarm" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }
    }
}

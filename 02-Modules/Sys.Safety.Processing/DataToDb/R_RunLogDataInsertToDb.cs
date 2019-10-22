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
    public class R_RunLogDataInsertToDb : DataToDbManager<R_RInfo>
    {
        public static DataToDbManager<R_RInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new R_RunLogDataInsertToDb();
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
                    "val",
                    "timer",
                    "remark",
                    "upflag",
                    "Bz1",
                    "Bz2",
                    "Bz3",
                    "Bz4",
                    "Bz5"
                };


        private readonly IJc_RRepository runLogRepositoty;

        public R_RunLogDataInsertToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\R_RunLog\\";
            runLogRepositoty = ServiceFactory.Create<IJc_RRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "R_R_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override bool AddItemsToDb(List<R_RInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_RInfo>> groupYYYYMM = addItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, R_RInfo> info in groupYYYYMM)
                {
                    var runloginfolist = info.ToList();
                    var runlogModels = ObjectConverter.CopyList<R_RInfo, Jc_RModel>(runloginfolist);
                    //if (!runLogRepositoty.BulkCopy("Jc_R" + info.Key, runlogModels, BuildDataColumn(columns)))
                    if (!runLogRepositoty.BulkCopy("PE_DataRunRecord" + info.Key, runlogModels, null)) //2017.9.16 by 解决入库时间带毫秒问题
                    {
                        int isconn = runLogRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(runloginfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("运行记录数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<R_RInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_RInfo>> groupYYYYMM = updateItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, R_RInfo> info in groupYYYYMM)
                {
                    var runloginfolist = info.ToList();
                    var runlogModels = ObjectConverter.CopyList<R_RInfo, Jc_RModel>(runloginfolist);
                    if (!runLogRepositoty.BulkUpdate("PE_DataRunRecord" + info.Key, runlogModels, BuildDataColumn(columns), "ID"))
                    {
                        int isconn = runLogRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(runloginfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("运行记录数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<R_RInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.Bz4 = "2");

                var key = addLocalItems[0].Timer.ToString("yyyyMM");
                var initialModels = ObjectConverter.CopyList<R_RInfo, Jc_RModel>(addLocalItems);
                return runLogRepositoty.BulkCopy("PE_DataRunRecord" + key, initialModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<R_RInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.Bz4 = "2");

                var key = updateLocalItems[0].Timer.ToString("yyyyMM");
                var initialModels = ObjectConverter.CopyList<R_RInfo, Jc_RModel>(updateLocalItems);
                return runLogRepositoty.BulkUpdate("PE_DataRunRecord" + key, initialModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

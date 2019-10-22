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
    public class InitialDataInsertToDb : DataToDbManager<Jc_McInfo>
    {
               

        public static DataToDbManager<Jc_McInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new InitialDataInsertToDb();
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
                    "point",
                    "devid",
                    "wzid",
                    "timer",
                    "ssz",
                    "voltage",
                    "type",
                    "state",
                    "upflag",
                    "BZ1",
                    "BZ2",
                    "BZ3",
                    "BZ4",
                    "BZ5",
                    "BZ6"
                };

        private readonly IJc_McRepository runLogRepositoty;

        public InitialDataInsertToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\Initial\\";
            runLogRepositoty = ServiceFactory.Create<IJc_McRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_DataDetail_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override bool AddItemsToDb(List<Jc_McInfo> addItems)
        {
            try
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                IEnumerable<IGrouping<string, Jc_McInfo>> groupYYYYMMDD = addItems.GroupBy(p => p.Timer.ToString("yyyyMMdd"));
                foreach (IGrouping<string, Jc_McInfo> info in groupYYYYMMDD)
                {
                    var initialinfolist = info.ToList();
                    var initialModels = ObjectConverter.CopyList<Jc_McInfo, Jc_McModel>(initialinfolist);
                    //if (!runLogRepositoty.BulkCopy("KJ_DataDetail" + info.Key, initialModels, BuildDataColumn(columns)))
                    if (!runLogRepositoty.BulkCopy("KJ_DataDetail" + info.Key, initialModels, null)) //2017.9.16 by 解决入库时间带毫秒问题
                    {
                        //数据库连接失败则缓存数据至本地文件
                        int isconn = runLogRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(initialinfolist);
                        }
                    }
                }

                sw.Stop();
                LogHelper.Info(string.Format("密采批量入库：{0} 条，耗时：{1}", addItems.Count, sw.ElapsedMilliseconds));
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("密采数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<Jc_McInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_McInfo>> groupYYYYMMDD = updateItems.GroupBy(p => p.Timer.ToString("yyyyMMdd"));
                foreach (IGrouping<string, Jc_McInfo> info in groupYYYYMMDD)
                {
                    var initialinfolist = info.ToList();
                    var initialModels = ObjectConverter.CopyList<Jc_McInfo, Jc_McModel>(initialinfolist);
                    if (!runLogRepositoty.BulkUpdate("KJ_DataDetail" + info.Key, initialModels, BuildDataColumn(columns), "ID"))
                    {
                        int isconn = runLogRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(initialinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("密采数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<Jc_McInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                var key = addLocalItems[0].Timer.ToString("yyyyMMdd");
                var initialModels = ObjectConverter.CopyList<Jc_McInfo, Jc_McModel>(addLocalItems);
                return runLogRepositoty.BulkCopy("KJ_DataDetail" + key, initialModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<Jc_McInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                var key = updateLocalItems[0].Timer.ToString("yyyyMMdd");
                var initialModels = ObjectConverter.CopyList<Jc_McInfo, Jc_McModel>(updateLocalItems);
                return runLogRepositoty.BulkUpdate("KJ_DataDetail" + key, initialModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

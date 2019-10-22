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
    public class FiveMinDataInsertToDb:DataToDbManager<Jc_MInfo>
    {
        public static DataToDbManager<Jc_MInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new FiveMinDataInsertToDb();
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
                    "zdz",
                    "zxz",
                    "pjz",
                    "ssz",
                    "type",
                    "state",
                    "zdzs",
                    "zxzs",
                    "sj",
                    "timer",
                    "upflag",
                    "Bz4"
                };

        private readonly IJc_MRepository fiveMinRepository;

        public FiveMinDataInsertToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\FiveMin\\";
            fiveMinRepository = ServiceFactory.Create<IJc_MRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_StaFiveMinute_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override bool AddItemsToDb(List<Jc_MInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_MInfo>> groupYYYYMMDD = addItems.GroupBy(p => p.Timer.ToString("yyyyMMdd"));
                foreach (IGrouping<string, Jc_MInfo> info in groupYYYYMMDD)
                {
                    var fivemininfolist = info.ToList();
                    var fivemModels = ObjectConverter.CopyList<Jc_MInfo, Jc_MModel>(fivemininfolist);
                    if (!fiveMinRepository.BulkCopy("KJ_StaFiveMinute" + info.Key, fivemModels, BuildDataColumn(columns)))
                    {
                        int isconn = fiveMinRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(fivemininfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("五分钟数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<Jc_MInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_MInfo>> groupYYYYMMDD = updateItems.GroupBy(p => p.Timer.ToString("yyyyMMdd"));
                foreach (IGrouping<string, Jc_MInfo> info in groupYYYYMMDD)
                {
                    var fivemininfolist = info.ToList();
                    var fivemModels = ObjectConverter.CopyList<Jc_MInfo, Jc_MModel>(fivemininfolist);
                    if (!fiveMinRepository.BulkUpdate("KJ_StaFiveMinute" + info.Key, fivemModels, BuildDataColumn(columns), "ID"))
                    {
                        int isconn = fiveMinRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(fivemininfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("五分钟数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<Jc_MInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.Bz4 = "2");
                var key = addLocalItems[0].Timer.ToString("yyyyMMdd");
                var fivemModels = ObjectConverter.CopyList<Jc_MInfo, Jc_MModel>(addLocalItems);
                return fiveMinRepository.BulkCopy("KJ_StaFiveMinute" + key, fivemModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<Jc_MInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.Bz4 = "2");
                var key = updateLocalItems[0].Timer.ToString("yyyyMMdd");
                var fivemModels = ObjectConverter.CopyList<Jc_MInfo, Jc_MModel>(updateLocalItems);
                return fiveMinRepository.BulkUpdate("KJ_StaFiveMinute" + key, fivemModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

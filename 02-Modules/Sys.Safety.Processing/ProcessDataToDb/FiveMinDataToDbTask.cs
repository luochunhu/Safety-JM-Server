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
    /// 描述:五分钟数据入库线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class FiveMinDataToDbTask : ProcessDataToDbManager<Jc_MInfo>
    {
        private static volatile FiveMinDataToDbTask _instance;
        public static FiveMinDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new FiveMinDataToDbTask(1000);
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

        private FiveMinDataToDbTask(int interval)
            : base("五分钟数据入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\FiveMin\\";
            fiveMinRepository = ServiceFactory.Create<IJc_MRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_StaFiveMinute_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override void AddItemsToDb(List<Jc_MInfo> addItems)
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("五分钟数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override void UpdateItemsToDb(List<Jc_MInfo> updateItems)
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("五分钟数据更新失败：" + "\r\n" + ex.Message);
            }
        }
    }
}

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
    /// 描述:运行记录数据入库线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class RunLogDataToDbTask : ProcessDataToDbManager<Jc_RInfo>
    {
        private static volatile RunLogDataToDbTask _instance;
        public static RunLogDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RunLogDataToDbTask(1000);
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

        private RunLogDataToDbTask(int interval)
            : base("运行记录入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\RunLog\\";
            runLogRepositoty = ServiceFactory.Create<IJc_RRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_DataRunRecord_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override void AddItemsToDb(List<Jc_RInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_RInfo>> groupYYYYMM = addItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, Jc_RInfo> info in groupYYYYMM)
                {
                    var runloginfolist = info.ToList();
                    var runlogModels = ObjectConverter.CopyList<Jc_RInfo, Jc_RModel>(runloginfolist);
                    if (!runLogRepositoty.BulkCopy("KJ_DataRunRecord" + info.Key, runlogModels, BuildDataColumn(columns)))
                    {
                        int isconn = runLogRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(runloginfolist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("运行记录数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override void UpdateItemsToDb(List<Jc_RInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_RInfo>> groupYYYYMM = updateItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, Jc_RInfo> info in groupYYYYMM)
                {
                    var runloginfolist = info.ToList();
                    var runlogModels = ObjectConverter.CopyList<Jc_RInfo, Jc_RModel>(runloginfolist);
                    if (!runLogRepositoty.BulkUpdate("KJ_DataRunRecord" + info.Key, runlogModels, BuildDataColumn(columns), "ID"))
                    {
                        int isconn = runLogRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(runloginfolist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("运行记录数据更新失败：" + "\r\n" + ex.Message);
            }
        }
    }
}

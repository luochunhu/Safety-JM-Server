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
    public class InitialDataToDbTask : ProcessDataToDbManager<Jc_McInfo>
    {
        private static volatile InitialDataToDbTask _instance;
        public static InitialDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new InitialDataToDbTask(1000);
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

        private InitialDataToDbTask(int interval)
            : base("密采数据入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\Initial\\";
            runLogRepositoty = ServiceFactory.Create<IJc_McRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_DataDetail_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override void AddItemsToDb(List<Jc_McInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_McInfo>> groupYYYYMMDD = addItems.GroupBy(p => p.Timer.ToString("yyyyMMdd"));
                foreach (IGrouping<string, Jc_McInfo> info in groupYYYYMMDD)
                {
                    var initialinfolist = info.ToList();
                    var initialModels = ObjectConverter.CopyList<Jc_McInfo, Jc_McModel>(initialinfolist);
                    if (!runLogRepositoty.BulkCopy("KJ_DataDetail" + info.Key, initialModels, BuildDataColumn(columns)))
                    {
                        //数据库连接失败则缓存数据至本地文件
                        int isconn = runLogRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(initialinfolist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("密采数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override void UpdateItemsToDb(List<Jc_McInfo> updateItems)
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("密采数据更新失败：" + "\r\n" + ex.Message);
            }
        }
    }
}

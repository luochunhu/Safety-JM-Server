using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.LocalDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:运行记录数据补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class RunLogLocalDataToDbTask : LocalDataToDbManager<Jc_RInfo>
    {
        private static volatile RunLogLocalDataToDbTask _instance;
        public static RunLogLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RunLogLocalDataToDbTask(1000);
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

        private RunLogLocalDataToDbTask(int interval)
            : base("运行记录数据补录任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\RunLog\\";
            runLogRepositoty = ServiceFactory.Create<IJc_RRepository>();
        }

        protected override bool AddLocalDataToDb(List<Jc_RInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                var key = addLocalItems[0].Timer.ToString("yyyyMM");
                var initialModels = ObjectConverter.CopyList<Jc_RInfo, Jc_RModel>(addLocalItems);
                return runLogRepositoty.BulkCopy("KJ_DataRunRecord" + key, initialModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<Jc_RInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                var key = updateLocalItems[0].Timer.ToString("yyyyMM");
                var initialModels = ObjectConverter.CopyList<Jc_RInfo, Jc_RModel>(updateLocalItems);
                return runLogRepositoty.BulkUpdate("KJ_DataRunRecord" + key, initialModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

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
    /// 描述:密采数据补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class InitialLocalDataToDbTask : LocalDataToDbManager<Jc_McInfo>
    {
        private static volatile InitialLocalDataToDbTask _instance;
        public static InitialLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new InitialLocalDataToDbTask(1000);
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

        private readonly IJc_McRepository initialRepositoty;

        private InitialLocalDataToDbTask(int interval)
            : base("密采数据补录任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\Initial\\";
            initialRepositoty = ServiceFactory.Create<IJc_McRepository>();
        }

        protected override bool AddLocalDataToDb(List<Jc_McInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                var key = addLocalItems[0].Timer.ToString("yyyyMMdd");
                var initialModels = ObjectConverter.CopyList<Jc_McInfo, Jc_McModel>(addLocalItems);
                return initialRepositoty.BulkCopy("KJ_DataDetail" + key, initialModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<Jc_McInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                var key = updateLocalItems[0].Timer.ToString("yyyyMMdd");
                var initialModels = ObjectConverter.CopyList<Jc_McInfo, Jc_McModel>(updateLocalItems);
                return initialRepositoty.BulkUpdate("KJ_DataDetail" + key, initialModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

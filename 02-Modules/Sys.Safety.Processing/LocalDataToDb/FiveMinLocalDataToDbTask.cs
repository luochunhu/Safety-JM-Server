using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.LocalDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:五分钟数据补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class FiveMinLocalDataToDbTask : LocalDataToDbManager<Jc_MInfo>
    {
        private static volatile FiveMinLocalDataToDbTask _instance;
        public static FiveMinLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new FiveMinLocalDataToDbTask(1000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IJc_MRepository fiveMinRepository;

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

        private FiveMinLocalDataToDbTask(int interval)
            : base("五分钟数据补录任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\FiveMin\\";
            fiveMinRepository = ServiceFactory.Create<IJc_MRepository>();
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

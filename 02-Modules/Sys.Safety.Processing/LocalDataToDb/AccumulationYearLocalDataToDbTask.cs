using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System.Collections.Generic;

namespace Sys.Safety.Processing.LocalDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:累积量年表补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AccumulationYearLocalDataToDbTask : LocalDataToDbManager<Jc_Ll_YInfo>
    {
        private static volatile AccumulationYearLocalDataToDbTask _instance;
        public static AccumulationYearLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationYearLocalDataToDbTask(1000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IAccumulationYearRepository accumulationRepository;

        string[] columns = new string[] {
            "ID",
            "PointID",
            "BHL",
            "BCL",
            "GHL",
            "GCL",
            "Timer",
            "Bz4"
        };

        private AccumulationYearLocalDataToDbTask(int interval)
            : base("累积量年表补录任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\AccumulationYear\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationYearRepository>();
        }

        protected override bool AddLocalDataToDb(List<Jc_Ll_YInfo> addLocalItems)
        {
            addLocalItems.ForEach(o => o.Bz4 = "2");
            var accumulationYModels = ObjectConverter.CopyList<Jc_Ll_YInfo, Jc_Ll_YModel>(addLocalItems);
            return accumulationRepository.BulkCopy("CF_Year", accumulationYModels, BuildDataColumn(columns));
        }

        protected override bool UpdateLocalDataToDb(List<Jc_Ll_YInfo> updateLocalItems)
        {
            updateLocalItems.ForEach(o => o.Bz4 = "2");
            var accumulationDModels = ObjectConverter.CopyList<Jc_Ll_YInfo, Jc_Ll_YModel>(updateLocalItems);
            return accumulationRepository.BulkUpdate("CF_Year", accumulationDModels, BuildDataColumn(columns), "ID");
        }
    }
}

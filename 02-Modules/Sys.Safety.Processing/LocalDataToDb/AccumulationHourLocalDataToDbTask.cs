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
    /// 描述:累积量时表补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AccumulationHourLocalDataToDbTask : LocalDataToDbManager<Jc_Ll_HInfo>
    {
        private static volatile AccumulationHourLocalDataToDbTask _instance;
        public static AccumulationHourLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationHourLocalDataToDbTask(1000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IAccumulationHourRepository accumulationRepository;

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

        private AccumulationHourLocalDataToDbTask(int interval)
            : base("累积量时表补录任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\AccumulationHour\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationHourRepository>();
        }

        protected override bool AddLocalDataToDb(List<Jc_Ll_HInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.Bz4 = "2");
                var key = addLocalItems[0].Timer.ToString("yyyyMM");
                var accumulationHModels = ObjectConverter.CopyList<Jc_Ll_HInfo, Jc_Ll_HModel>(addLocalItems);
                return accumulationRepository.BulkCopy("CF_Hour" + key, accumulationHModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<Jc_Ll_HInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.Bz4 = "2");
                var key = updateLocalItems[0].Timer.ToString("yyyyMM");
                var accumulationHModels = ObjectConverter.CopyList<Jc_Ll_HInfo, Jc_Ll_HModel>(updateLocalItems);
                return accumulationRepository.BulkUpdate("CF_Hour" + key, accumulationHModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

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
    /// 描述:累积量日表补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AccumulationDayLocalDataToDbTask : LocalDataToDbManager<Jc_Ll_DInfo>
    {
        private static volatile AccumulationDayLocalDataToDbTask _instance;
        public static AccumulationDayLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationDayLocalDataToDbTask(1000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IAccumulationDayRepository accumulationRepository;

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

        private AccumulationDayLocalDataToDbTask(int interval)
            : base("累积量日表补录任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\AccumulationDay\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationDayRepository>();
        }

        protected override bool AddLocalDataToDb(List<Jc_Ll_DInfo> addLocalItems)
        {
            addLocalItems.ForEach(o => o.Bz4 = "2");
            var accumulationDModels = ObjectConverter.CopyList<Jc_Ll_DInfo, Jc_Ll_DModel>(addLocalItems);
            return accumulationRepository.BulkCopy("CF_Day", accumulationDModels, BuildDataColumn(columns));
        }

        protected override bool UpdateLocalDataToDb(List<Jc_Ll_DInfo> updateLocalItems)
        {
            updateLocalItems.ForEach(o => o.Bz4 = "2");
            var accumulationDModels = ObjectConverter.CopyList<Jc_Ll_DInfo, Jc_Ll_DModel>(updateLocalItems);
            return accumulationRepository.BulkUpdate("CF_Day", accumulationDModels, BuildDataColumn(columns), "ID");
        }
    }
}

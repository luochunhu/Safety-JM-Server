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
    /// 描述:累积量月表补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AccumulationMonthLocalDataToDbTask : LocalDataToDbManager<Jc_Ll_MInfo>
    {
        private static volatile AccumulationMonthLocalDataToDbTask _instance;
        public static AccumulationMonthLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationMonthLocalDataToDbTask(1000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IAccumulationMonthRepository accumulationRepository;

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

        private AccumulationMonthLocalDataToDbTask(int interval)
            :base("累积量月表补录任务",interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\AccumulationMonth\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationMonthRepository>();
        }

        protected override bool AddLocalDataToDb(List<Jc_Ll_MInfo> addLocalItems)
        {
            addLocalItems.ForEach(o => o.Bz4 = "2");
            var accumulationMModels = ObjectConverter.CopyList<Jc_Ll_MInfo, Jc_Ll_MModel>(addLocalItems);
            return accumulationRepository.BulkCopy("CF_Month", accumulationMModels, BuildDataColumn(columns));
        }

        protected override bool UpdateLocalDataToDb(List<Jc_Ll_MInfo> updateLocalItems)
        {
            updateLocalItems.ForEach(o => o.Bz4 = "2");
            var accumulationMModels = ObjectConverter.CopyList<Jc_Ll_MInfo, Jc_Ll_MModel>(updateLocalItems);
            return accumulationRepository.BulkUpdate("CF_Month", accumulationMModels, BuildDataColumn(columns), "ID");
        }
    }
}

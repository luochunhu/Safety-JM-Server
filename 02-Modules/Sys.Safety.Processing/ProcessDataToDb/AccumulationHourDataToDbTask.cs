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
    /// 描述:累积量时数据入库线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AccumulationHourDataToDbTask : ProcessDataToDbManager<Jc_Ll_HInfo>
    {
        private static AccumulationHourDataToDbTask _instance;

        public static AccumulationHourDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationHourDataToDbTask(1000);
                        }
                    }
                }
                return _instance;
            }
        }

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

        private readonly IAccumulationHourRepository accumulationRepository;

        private AccumulationHourDataToDbTask(int interval)
            : base("累积量小时数据入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\AccumulationHour\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationHourRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "CF_Hour_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override void AddItemsToDb(List<Jc_Ll_HInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_Ll_HInfo>> groupYYYYMM = addItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, Jc_Ll_HInfo> info in groupYYYYMM)
                {
                    var accumulationHinfolist = info.ToList();
                    var accumulationHModels = ObjectConverter.CopyList<Jc_Ll_HInfo, Jc_Ll_HModel>(accumulationHinfolist);
                    if (!accumulationRepository.BulkCopy("CF_Hour" + info.Key, accumulationHModels, BuildDataColumn(columns)))
                    {
                        int isconn = accumulationRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(accumulationHinfolist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("累积量小时数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override void UpdateItemsToDb(List<Jc_Ll_HInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_Ll_HInfo>> groupYYYYMM = updateItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, Jc_Ll_HInfo> info in groupYYYYMM)
                {
                    var accumulationHinfolist = info.ToList();
                    var accumulationHModels = ObjectConverter.CopyList<Jc_Ll_HInfo, Jc_Ll_HModel>(accumulationHinfolist);
                    if (!accumulationRepository.BulkUpdate("CF_Hour" + info.Key, accumulationHModels, BuildDataColumn(columns), "ID"))
                    {
                        int isconn = accumulationRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(accumulationHinfolist);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("累积量小时数据更新失败：" + "\r\n" + ex.Message);
            }
        }
    }
}

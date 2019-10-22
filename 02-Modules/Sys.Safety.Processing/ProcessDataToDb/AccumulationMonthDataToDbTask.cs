using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System;
using System.Collections.Generic;

namespace Sys.Safety.Processing.ProcessDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:累积量月数据入库线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AccumulationMonthDataToDbTask : ProcessDataToDbManager<Jc_Ll_MInfo>
    {
        private static volatile AccumulationMonthDataToDbTask _instance;

        public static AccumulationMonthDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationMonthDataToDbTask(1000);
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

        private readonly IAccumulationMonthRepository accumulationRepository;

        private AccumulationMonthDataToDbTask(int interval)
            : base("累积量月表数据入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\AccumulationMonth\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationMonthRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "CF_Month_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }
        protected override void AddItemsToDb(List<Jc_Ll_MInfo> addItems)
        {
            try
            {
                var accumulationMModels = ObjectConverter.CopyList<Jc_Ll_MInfo, Jc_Ll_MModel>(addItems);
                if (!accumulationRepository.BulkCopy("CF_Month", accumulationMModels, BuildDataColumn(columns)))
                {
                    int isconn = accumulationRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                    if (isconn <= 0)
                    {
                        AddDataToLocal(addItems);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("累积量月数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override void UpdateItemsToDb(List<Jc_Ll_MInfo> updateItems)
        {
            try
            {
                var accumulationMModels = ObjectConverter.CopyList<Jc_Ll_MInfo, Jc_Ll_MModel>(updateItems);
                if (!accumulationRepository.BulkUpdate("CF_Month", accumulationMModels, BuildDataColumn(columns), "ID"))
                {
                    int isconn = accumulationRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                    if (isconn <= 0)
                    {
                        AddDataToLocal(updateItems);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("累积量月数据更新失败：" + "\r\n" + ex.Message);
            }
        }
    }
}

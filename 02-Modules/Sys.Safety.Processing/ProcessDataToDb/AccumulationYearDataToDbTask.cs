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
    /// 描述:累积量年数据入库线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AccumulationYearDataToDbTask : ProcessDataToDbManager<Jc_Ll_YInfo>
    {
        private static volatile AccumulationYearDataToDbTask _instance;
        public static AccumulationYearDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationYearDataToDbTask(1000);
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

        private readonly IAccumulationYearRepository accumulationRepository;

        private AccumulationYearDataToDbTask(int interval)
            : base("累积量年表数据入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\AccumulationYear\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationYearRepository>();
        }

        protected override void AddItemsToDb(List<Jc_Ll_YInfo> addItems)
        {
            try
            {
                var accumulationYModels = ObjectConverter.CopyList<Jc_Ll_YInfo, Jc_Ll_YModel>(addItems);
                if (!accumulationRepository.BulkCopy("CF_Year", accumulationYModels, BuildDataColumn(columns)))
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
                LogHelper.Error("累积量年数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "CF_Year_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override void UpdateItemsToDb(List<Jc_Ll_YInfo> updateItems)
        {
            try
            {
                var accumulationDModels = ObjectConverter.CopyList<Jc_Ll_YInfo, Jc_Ll_YModel>(updateItems);
                if (!accumulationRepository.BulkUpdate("CF_Year", accumulationDModels, BuildDataColumn(columns), "ID"))
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
                LogHelper.Error("累积量年数据更新失败：" + "\r\n" + ex.Message);
            }
        }

    }
}

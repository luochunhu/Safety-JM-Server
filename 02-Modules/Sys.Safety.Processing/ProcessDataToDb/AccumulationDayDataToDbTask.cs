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
    /// 描述:累积量日数据入库线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AccumulationDayDataToDbTask : ProcessDataToDbManager<Jc_Ll_DInfo>
    {
        public static AccumulationDayDataToDbTask _instance;
        public static AccumulationDayDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationDayDataToDbTask(500);
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

        private readonly IAccumulationDayRepository accumulationRepository;

        private AccumulationDayDataToDbTask(int interval)
            : base("累积量日表数据入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\AccumulationDay\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationDayRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "CF_Day_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override void AddItemsToDb(List<Jc_Ll_DInfo> addItems)
        {
            try
            {
                var accumulationDModels = ObjectConverter.CopyList<Jc_Ll_DInfo, Jc_Ll_DModel>(addItems);
                //如果插入失败,则判断数据库连接是否成功
                if (!accumulationRepository.BulkCopy("CF_Day", accumulationDModels, BuildDataColumn(columns)))
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
                LogHelper.Error("累积量日表数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override void UpdateItemsToDb(List<Jc_Ll_DInfo> updateItems)
        {
            try
            {
                var accumulationDModels = ObjectConverter.CopyList<Jc_Ll_DInfo, Jc_Ll_DModel>(updateItems);
                if (!accumulationRepository.BulkUpdate("CF_Day", accumulationDModels, BuildDataColumn(columns), "ID"))
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
                LogHelper.Error("累积量日表数据更新失败：" + "\r\n" + ex.Message);
            }
        }
    }
}

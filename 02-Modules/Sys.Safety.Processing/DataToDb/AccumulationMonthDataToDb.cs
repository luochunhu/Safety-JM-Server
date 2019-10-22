using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.DataToDb
{
    public class AccumulationMonthDataToDb : DataToDbManager<Jc_Ll_MInfo>
    {
        public static DataToDbManager<Jc_Ll_MInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationMonthDataToDb();
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
        private readonly IAccumulationMonthService accumulationService;

        public AccumulationMonthDataToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\AccumulationMonth\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationMonthRepository>();
            accumulationService = ServiceFactory.Create<IAccumulationMonthService>();
        }

        protected override bool AddItemsToDb(List<Jc_Ll_MInfo> addItems)
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
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("累积量月数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "CF_Month_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override bool UpdateItemsToDb(List<Jc_Ll_MInfo> updateItems)
        {
            try
            {
                var accumulationMModels = ObjectConverter.CopyList<Jc_Ll_MInfo, Jc_Ll_MModel>(updateItems);
                if (!accumulationRepository.BulkUpdate("CF_Month", accumulationMModels, BuildDataColumn(columns), "ID"))
                {
                    //int isconn = accumulationRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                    //if (isconn <= 0)
                    //{
                    //    AddDataToLocal(updateItems);
                    //}
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("累积量月数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<Jc_Ll_MInfo> addLocalItems)
        {
            //addLocalItems.ForEach(o => o.Bz4 = "2");
            //var accumulationMModels = ObjectConverter.CopyList<Jc_Ll_MInfo, Jc_Ll_MModel>(addLocalItems);
            //return accumulationRepository.BulkCopy("Jc_Ll_M", accumulationMModels, BuildDataColumn(columns));

            try
            {
                IEnumerable<IGrouping<string, Jc_Ll_MInfo>> groupPointId = addLocalItems.GroupBy(p => p.PointID);
                foreach (var item in groupPointId)
                {
                    List<Jc_Ll_MInfo> accumulationDInfos = new List<Jc_Ll_MInfo>();
                    //累计量获取最后一条数据
                    var accumulationitem = item.Last();
                    if (accumulationitem != null)
                    {
                        var existsitemresponse = accumulationService.ExistsAccumulationMonthInfo(new AccumulationMonthExistsRequest { PointId = accumulationitem.PointID, Timer = accumulationitem.Timer });
                        //如果存在同一测点的累积量数据，则判断补录数据与存在数据先后顺序。当补录数据timer>存在数据timer时,做补录操作。
                        if (existsitemresponse.IsSuccess && existsitemresponse.Data != null)
                        {
                            //累积量更新补录时不能判断先后顺序。先不做
                            //var existsmodel = existsitemresponse.Data;
                            //existsmodel.BHL = accumulationitem.BHL;
                            //existsmodel.BCL = accumulationitem.BCL;
                            //existsmodel.GHL = accumulationitem.GHL;
                            //existsmodel.GCL = accumulationitem.GCL;
                            //existsmodel.Timer = accumulationitem.Timer;
                            //existsmodel.Bz4 = "2";
                            //existsmodel.InfoState = Framework.Web.InfoState.Modified;

                            //accumulationDInfos.Add(existsmodel);
                            //UpdateItemsToDb(accumulationDInfos);
                        }
                        //不存在则新增数据
                        else
                        {
                            accumulationitem.Bz4 = "2";
                            accumulationDInfos.Add(accumulationitem);
                            AddItemsToDb(accumulationDInfos);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Info("累计量日表数据补录失败！" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateLocalDataToDb(List<Jc_Ll_MInfo> updateLocalItems)
        {
            //updateLocalItems.ForEach(o => o.Bz4 = "2");
            //var accumulationMModels = ObjectConverter.CopyList<Jc_Ll_MInfo, Jc_Ll_MModel>(updateLocalItems);
            //return accumulationRepository.BulkUpdate("Jc_Ll_M", accumulationMModels, BuildDataColumn(columns), "ID");

            return true;
        }
    }
}

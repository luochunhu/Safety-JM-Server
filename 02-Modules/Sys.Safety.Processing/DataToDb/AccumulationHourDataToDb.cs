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
    public class AccumulationHourDataToDb: DataToDbManager<Jc_Ll_HInfo>
    {
        public static DataToDbManager<Jc_Ll_HInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccumulationHourDataToDb();
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
        private readonly IAccumulationHourService accumulationService;

        public AccumulationHourDataToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\AccumulationHour\\";
            accumulationRepository = ServiceFactory.Create<IAccumulationHourRepository>();
            accumulationService = ServiceFactory.Create<IAccumulationHourService>();
        }

        protected override bool AddItemsToDb(List<Jc_Ll_HInfo> addItems)
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
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Info("累积量小时数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "CF_Hour_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override bool UpdateItemsToDb(List<Jc_Ll_HInfo> updateItems)
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
                        //int isconn = accumulationRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        //if (isconn <= 0)
                        //{
                        //    AddDataToLocal(accumulationHinfolist);
                        //}

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Info("累积量小时数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }



        protected override bool AddLocalDataToDb(List<Jc_Ll_HInfo> addLocalItems)
        {
            //if (addLocalItems.Any())
            //{
            //    addLocalItems.ForEach(o => o.Bz4 = "2");
            //    var key = addLocalItems[0].Timer.ToString("yyyyMM");
            //    var accumulationHModels = ObjectConverter.CopyList<Jc_Ll_HInfo, Jc_Ll_HModel>(addLocalItems);
            //    return accumulationRepository.BulkCopy("Jc_LL_H" + key, accumulationHModels, BuildDataColumn(columns));
            //}
            //return true;

            try
            {
                IEnumerable<IGrouping<string, Jc_Ll_HInfo>> groupPointId = addLocalItems.GroupBy(p => p.PointID);
                foreach (var item in groupPointId)
                {
                    List<Jc_Ll_HInfo> accumulationDInfos = new List<Jc_Ll_HInfo>();
                    //累计量获取最后一条数据
                    var accumulationitem = item.Last();
                    if (accumulationitem != null)
                    {
                        var existsitemresponse = accumulationService.ExistsAccumulationHourInfo(new AccumulationHourExistsRequest { PointId = accumulationitem.PointID, Timer = accumulationitem.Timer });
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

        protected override bool UpdateLocalDataToDb(List<Jc_Ll_HInfo> updateLocalItems)
        {
            //if (updateLocalItems.Any())
            //{
            //    updateLocalItems.ForEach(o => o.Bz4 = "2");
            //    var key = updateLocalItems[0].Timer.ToString("yyyyMM");
            //    var accumulationHModels = ObjectConverter.CopyList<Jc_Ll_HInfo, Jc_Ll_HModel>(updateLocalItems);
            //    return accumulationRepository.BulkUpdate("Jc_LL_H" + key, accumulationHModels, BuildDataColumn(columns), "ID");
            //}
            return true;
        }
    }
}

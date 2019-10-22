using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.DataToDb
{
    public class R_PHistoryDataToDb : DataToDbManager<R_PhistoryInfo>
    {
        public static DataToDbManager<R_PhistoryInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new R_PHistoryDataToDb();
                        }
                    }
                }
                return _instance;
            }
        }

        string[] columns = new string[] {
                    "Id",
                    "Bh",
                    "Yid",
                    "PointId",
                    "rtime",
                    "timer",
                    "flag",
                    "sysflag",
                    "cwflag",
                    "By1",
                    "By2",
                    "By3",
                    "By4",
                    "By5"
                };

        private readonly IR_PhistoryRepository phistoryRepositoty;

        public R_PHistoryDataToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\PHistory\\";
            phistoryRepositoty = ServiceFactory.Create<IR_PhistoryRepository>();
        }

        protected override bool AddItemsToDb(List<R_PhistoryInfo> addItems)
        {
            try
            {
                //修改，根据识别器上传的采集时间进行存储  20171213
                IEnumerable<IGrouping<string, R_PhistoryInfo>> groupYYYYMMDD = addItems.GroupBy(p => p.Rtime.ToString("yyyyMMdd"));
                foreach (IGrouping<string, R_PhistoryInfo> info in groupYYYYMMDD)
                {
                    var historyinfolist = info.ToList();
                    var historyModels = ObjectConverter.CopyList<R_PhistoryInfo, R_PhistoryModel>(historyinfolist);
                    if (!phistoryRepositoty.BulkCopy("PE_Trajectory" + info.Key, historyModels, BuildDataColumn(columns)))
                    {
                        int isconn = phistoryRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(historyinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("人员轨迹数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<R_PhistoryInfo> updateItems)
        {
            try
            {
                //修改，根据识别器上传的采集时间进行存储  20171213
                IEnumerable<IGrouping<string, R_PhistoryInfo>> groupYYYYMMDD = updateItems.GroupBy(p => p.Rtime.ToString("yyyyMMdd"));
                foreach (IGrouping<string, R_PhistoryInfo> info in groupYYYYMMDD)
                {
                    var historyinfolist = info.ToList();
                    var historyModels = ObjectConverter.CopyList<R_PhistoryInfo, R_PhistoryModel>(historyinfolist);
                    if (!phistoryRepositoty.BulkUpdate("PE_Trajectory" + info.Key, historyModels, BuildDataColumn(columns), "Id"))
                    {
                        int isconn = phistoryRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(historyinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("人员轨迹数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<R_PhistoryInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.By1 = "2");

                var key = addLocalItems[0].Timer.ToString("yyyyMMMMdd");
                var historyModels = ObjectConverter.CopyList<R_PhistoryInfo, R_PhistoryModel>(addLocalItems);
                return phistoryRepositoty.BulkCopy("PE_Trajectory" + key, historyModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<R_PhistoryInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.By1 = "2");

                var key = updateLocalItems[0].Timer.ToString("yyyyMMdd");
                var historyModels = ObjectConverter.CopyList<R_PhistoryInfo, R_PhistoryModel>(updateLocalItems);
                return phistoryRepositoty.BulkUpdate("PE_Trajectory" + key, historyModels, BuildDataColumn(columns), "Id");
            }
            return true;
        }

        protected override string GetFileName(DateTime nowtime) 
        {
            return FilePath + "PE_Trajectory" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }
    }
}

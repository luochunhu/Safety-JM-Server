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
    public class R_PhjDataToDb : DataToDbManager<R_PhjInfo>
    {
        public static DataToDbManager<R_PhjInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new R_PhjDataToDb();
                        }
                    }
                }
                return _instance;
            }
        }

        string[] columns = new string[] {
                    "Id",
                    "Hjlx",
                    "Bh",
                    "Yid",
                    "PointId",
                    "CallTime",
                    "Tsycs",
                    "Type",
                    "Card",
                    "Username",
                    "IP",
                    "Timer",
                    "Flag",
                    "Sysflag",
                    "By1",
                    "By2",
                    "By3",
                    "By4",
                    "By5"
                };

        private readonly IR_PhjRepository phjRepositoty;

        public R_PhjDataToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\PHj\\";
            phjRepositoty = ServiceFactory.Create<IR_PhjRepository>();
        }

        protected override bool AddItemsToDb(List<R_PhjInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_PhjInfo>> groupYYYYMMDD = addItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, R_PhjInfo> info in groupYYYYMMDD)
                {
                    var phjinfolist = info.ToList();
                    var phjModels = ObjectConverter.CopyList<R_PhjInfo, R_PhjModel>(phjinfolist);
                    if (!phjRepositoty.BulkCopy("PE_CallHistory" + info.Key, phjModels, BuildDataColumn(columns)))
                    {
                        int isconn = phjRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(phjinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("双向呼叫数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<R_PhjInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_PhjInfo>> groupYYYYMMDD = updateItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, R_PhjInfo> info in groupYYYYMMDD)
                {
                    var phjinfolist = info.ToList();
                    var phjModels = ObjectConverter.CopyList<R_PhjInfo, R_PhjModel>(phjinfolist);
                    if (!phjRepositoty.BulkUpdate("PE_CallHistory" + info.Key, phjModels, BuildDataColumn(columns), "Id"))
                    {
                        int isconn = phjRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(phjinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("双向呼叫数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<R_PhjInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.By1 = "2");

                var key = addLocalItems[0].Timer.ToString("yyyyMM");
                var phjModels = ObjectConverter.CopyList<R_PhjInfo, R_PhjModel>(addLocalItems);
                return phjRepositoty.BulkCopy("PE_CallHistory" + key, phjModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<R_PhjInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.By1 = "2");

                var key = updateLocalItems[0].Timer.ToString("yyyyMM");
                var phjModels = ObjectConverter.CopyList<R_PhjInfo, R_PhjModel>(updateLocalItems);
                return phjRepositoty.BulkUpdate("PE_CallHistory" + key, phjModels, BuildDataColumn(columns), "Id");
            }
            return true;
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "PE_CallHistory" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }
    }
}

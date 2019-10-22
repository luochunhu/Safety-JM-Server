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
    public class R_PbDataToDb : DataToDbManager<R_PbInfo>
    {
        public static DataToDbManager<R_PbInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new R_PbDataToDb();
                        }
                    }
                }
                return _instance;
            }
        }

        string[] columns = new string[] {
                    "Id",
                    "Areaid",
                    "Yid",
                    "Pointid",
                    "Zdzs",
                    "Starttime",
                    "Endtime",
                    "Type",
                    "Z1",
                    "Z2",
                    "Z3",
                    "Z4",
                    "Z5",
                    "Z6",
                    "Upflag"
                };

        private readonly IR_PbRepository pbRepositoty;

        public R_PbDataToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\RPB\\";
            pbRepositoty = ServiceFactory.Create<IR_PbRepository>();
        }

        protected override bool AddItemsToDb(List<R_PbInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_PbInfo>> groupYYYYMMDD = addItems.GroupBy(p => p.Starttime.ToString("yyyyMM"));
                foreach (IGrouping<string, R_PbInfo> info in groupYYYYMMDD)
                {
                    var phjinfolist = info.ToList();
                    var phjModels = ObjectConverter.CopyList<R_PbInfo, R_PbModel>(phjinfolist);
                    if (!pbRepositoty.BulkCopy("PE_PersonAlarm" + info.Key, phjModels, BuildDataColumn(columns)))
                    {
                        int isconn = pbRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
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
                LogHelper.Error("人员报警数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<R_PbInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, R_PbInfo>> groupYYYYMMDD = updateItems.GroupBy(p => p.Starttime.ToString("yyyyMM"));
                foreach (IGrouping<string, R_PbInfo> info in groupYYYYMMDD)
                {
                    var phjinfolist = info.ToList();
                    var phjModels = ObjectConverter.CopyList<R_PbInfo, R_PbModel>(phjinfolist);
                    if (!pbRepositoty.BulkUpdate("PE_PersonAlarm" + info.Key, phjModels, BuildDataColumn(columns), "Id"))
                    {
                        int isconn = pbRepositoty.GetTotalRecord("DataToDb_GetDbServerIsNormal");
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
                LogHelper.Error("人员报警数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<R_PbInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.Z2 = "2");

                var key = addLocalItems[0].Starttime.ToString("yyyyMM");
                var phjModels = ObjectConverter.CopyList<R_PbInfo, R_PbModel>(addLocalItems);
                return pbRepositoty.BulkCopy("PE_PersonAlarm" + key, phjModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<R_PbInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.Z2 = "2");

                var key = updateLocalItems[0].Starttime.ToString("yyyyMM");
                var phjModels = ObjectConverter.CopyList<R_PbInfo, R_PbModel>(updateLocalItems);
                return pbRepositoty.BulkUpdate("PE_PersonAlarm" + key, phjModels, BuildDataColumn(columns), "Id");
            }
            return true;
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "PE_PersonAlarm" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }
    }
}

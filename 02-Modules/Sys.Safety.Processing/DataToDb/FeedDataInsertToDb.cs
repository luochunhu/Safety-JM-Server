using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.DataToDb
{
    public class FeedDataInsertToDb:DataToDbManager<Jc_KdInfo>
    {
        public static DataToDbManager<Jc_KdInfo> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new FeedDataInsertToDb();
                        }
                    }
                }
                return _instance;
            }
        }

        string[] columns = new string[] {
                    "ID",
                    "BJID",
                    "KDID",
                    "timer",
                    "Bz4"
                };

        private readonly IJc_KdRepository feedRepository;
        public FeedDataInsertToDb()
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:/LocalDb") + "\\Feed\\";
            feedRepository = ServiceFactory.Create<IJc_KdRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_FeedInfo_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override bool AddItemsToDb(List<Jc_KdInfo> addItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_KdInfo>> groupYYYYMM = addItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, Jc_KdInfo> info in groupYYYYMM)
                {
                    var feedinfolist = info.ToList();
                    var feedModels = ObjectConverter.CopyList<Jc_KdInfo, Jc_KdModel>(feedinfolist);
                    if (!feedRepository.BulkCopy("KJ_FeedInfo" + info.Key, feedModels, BuildDataColumn(columns)))
                    {
                        int isconn = feedRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(feedinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("馈电数据入库失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool UpdateItemsToDb(List<Jc_KdInfo> updateItems)
        {
            try
            {
                IEnumerable<IGrouping<string, Jc_KdInfo>> groupYYYYMM = updateItems.GroupBy(p => p.Timer.ToString("yyyyMM"));
                foreach (IGrouping<string, Jc_KdInfo> info in groupYYYYMM)
                {
                    var feedinfolist = info.ToList();
                    var feedModels = ObjectConverter.CopyList<Jc_KdInfo, Jc_KdModel>(feedinfolist);
                    if (!feedRepository.BulkUpdate("KJ_FeedInfo" + info.Key, feedModels, BuildDataColumn(columns), "ID"))
                    {
                        int isconn = feedRepository.GetTotalRecord("DataToDb_GetDbServerIsNormal");
                        if (isconn <= 0)
                        {
                            AddDataToLocal(feedinfolist);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("馈电数据更新失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

        protected override bool AddLocalDataToDb(List<Jc_KdInfo> addLocalItems)
        {
            if (addLocalItems.Any())
            {
                addLocalItems.ForEach(o => o.Bz4 = "2");
                var key = addLocalItems[0].Timer.ToString("yyyyMM");
                var feedModels = ObjectConverter.CopyList<Jc_KdInfo, Jc_KdModel>(addLocalItems);
                return feedRepository.BulkCopy("KJ_FeedInfo" + key, feedModels, BuildDataColumn(columns));
            }
            return true;
        }

        protected override bool UpdateLocalDataToDb(List<Jc_KdInfo> updateLocalItems)
        {
            if (updateLocalItems.Any())
            {
                updateLocalItems.ForEach(o => o.Bz4 = "2");
                var key = updateLocalItems[0].Timer.ToString("yyyyMM");
                var feedModels = ObjectConverter.CopyList<Jc_KdInfo, Jc_KdModel>(updateLocalItems);
                return feedRepository.BulkUpdate("KJ_FeedInfo" + key, feedModels, BuildDataColumn(columns), "ID");
            }
            return true;
        }
    }
}

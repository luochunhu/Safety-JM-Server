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
    /// 描述:馈电数据入库线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class FeedDataToDbTask : ProcessDataToDbManager<Jc_KdInfo>
    {
        private static volatile FeedDataToDbTask _instance;
        public static FeedDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new FeedDataToDbTask(1000);
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
        private FeedDataToDbTask(int interval)
            : base("馈电数据入库任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\Feed\\";
            feedRepository = ServiceFactory.Create<IJc_KdRepository>();
        }

        protected override string GetFileName(DateTime nowtime)
        {
            return FilePath + "KJ_FeedInfo_" + nowtime.ToString("yyyyMMddHHmmssfffff") + ".txt";
        }

        protected override void AddItemsToDb(List<Jc_KdInfo> addItems)
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("馈电数据入库失败：" + "\r\n" + ex.Message);
            }
        }

        protected override void UpdateItemsToDb(List<Jc_KdInfo> updateItems)
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("馈电数据更新失败：" + "\r\n" + ex.Message);
            }
        }
    }
}

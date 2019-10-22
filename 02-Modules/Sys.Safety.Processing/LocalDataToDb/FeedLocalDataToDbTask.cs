using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.LocalDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:馈电数据补录线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class FeedLocalDataToDbTask : LocalDataToDbManager<Jc_KdInfo>
    {
        private static volatile FeedLocalDataToDbTask _instance;
        public static FeedLocalDataToDbTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new FeedLocalDataToDbTask(1000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IJc_KdRepository feedRepository;

        string[] columns = new string[] {
                    "ID",
                    "BJID",
                    "KDID",
                    "timer",
                    "Bz4"
                };

        private FeedLocalDataToDbTask(int interval)
            : base("馈电数据补录任务", interval)
        {
            FilePath = ConfigurationManager.FileConfiguration.GetString("FileDataToDbPath", @"C:\LocalDb") + "\\Feed\\";
            feedRepository = ServiceFactory.Create<IJc_KdRepository>();
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

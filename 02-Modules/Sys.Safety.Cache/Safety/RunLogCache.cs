using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_R;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:运行日志缓存
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class RunLogCache : KJ73NCacheManager<Jc_RInfo>
    {
        private readonly IJc_RService runLogService;

        private static volatile RunLogCache runLogCahceInstance;
        /// <summary>
        /// 运行日志单例
        /// </summary>
        public static RunLogCache RunLogCahceInstance
        {
            get
            {
                if (runLogCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (runLogCahceInstance == null)
                        {
                            runLogCahceInstance = new RunLogCache();
                        }
                    }
                }
                return runLogCahceInstance;
            }
        }

        private RunLogCache()
        {
            runLogService = ServiceFactory.Create<IJc_RService>();
        }

        public override void Load()
        {
            //try
            //{
                //Jc_RGetListRequest request = new Jc_RGetListRequest();
                //var respose = runLogService.GetJc_RList(request);
                //if (respose != null && respose.IsSuccess)
                //{
                //    _cache.Clear();
                //    _cache = respose.Data;
                //}
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
            //}
        }

        protected override void AddEntityToCache(Jc_RInfo item)
        {
            if (_cache.Count(runlog => runlog.ID == item.ID) < 1)
            {
                item.Counter = DateTime.Now.Ticks;
                _cache.Add(item);
            }
        }

        protected override void DeleteEntityFromCache(Jc_RInfo item)
        {
            var tempitem = _cache.FirstOrDefault(runlog => runlog.ID == item.ID);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(Jc_RInfo item)
        {
            int itemIndex = _cache.FindIndex(runlog => runlog.ID == item.ID);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

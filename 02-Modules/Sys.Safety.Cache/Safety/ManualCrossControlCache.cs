using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.ManualCrossControl;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:手动控制缓存
    /// 修改记录
    /// 2017-05-24
    /// 2017-05-25
    /// </summary>
    public class ManualCrossControlCache : KJ73NCacheManager<Jc_JcsdkzInfo>
    {
        private readonly IManualCrossControlService manualCrossControlService;

        private static volatile ManualCrossControlCache manualCrossControlCahceInstance;
        /// <summary>
        /// 交叉手动控制单例
        /// </summary>
        public static ManualCrossControlCache ManualCrossControlCahceInstance
        {
            get
            {
                if (manualCrossControlCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (manualCrossControlCahceInstance == null)
                        {
                            manualCrossControlCahceInstance = new ManualCrossControlCache();
                        }
                    }
                }
                return manualCrossControlCahceInstance;
            }
        }

        private ManualCrossControlCache()
        {
            manualCrossControlService = ServiceFactory.Create<IManualCrossControlService>();
        }

        public override void Load()
        {
            //try
            //{
                var respose = manualCrossControlService.GetManualCrossControlList();
                if (respose.Data != null && respose.IsSuccess)
                {
                    _cache.Clear();
                    _cache = respose.Data;
                }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
            //}

        }

        protected override void AddEntityToCache(Jc_JcsdkzInfo item)
        {
            if (_cache.Count(manualControl => manualControl.ID == item.ID) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_JcsdkzInfo item)
        {
            var tempitem = _cache.FirstOrDefault(manualControl => manualControl.ID == item.ID);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(Jc_JcsdkzInfo item)
        {
            int itemIndex = _cache.FindIndex(manualControl => manualControl.ID == item.ID);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

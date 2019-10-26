using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_B;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-7-27
    /// 描述:报警缓存
    /// 修改记录
    /// </summary>
    public class RatioAlarmCache : KJ73NCacheManager<JC_MbInfo>
    {
        private readonly IJC_MbService ratioAlarmService;
        private static volatile RatioAlarmCache ratioAlarmCahceInstance;
        /// <summary>
        /// 报警单例
        /// </summary>
        public static RatioAlarmCache AlarmCacheInstance
        {
            get
            {
                if (ratioAlarmCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (ratioAlarmCahceInstance == null)
                        {
                            ratioAlarmCahceInstance = new RatioAlarmCache();
                        }
                    }
                }
                return ratioAlarmCahceInstance;
            }
        }

        private RatioAlarmCache()
        {
            ratioAlarmService = ServiceFactory.Create<IJC_MbService>();
        }

        public override void Load()
        {
            //try
            //{
            //var respose = ratioAlarmService.GetAlarmedDataList();
            //    if (respose.Data != null && respose.IsSuccess)
            //    {
            //        _cache.Clear();
            //        _cache = respose.Data;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
            //}
        }

        /// <summary>
        /// 清除报警缓存
        /// </summary>
        public void ClearUp()
        {
            _cache.Clear();
        }

        protected override void AddEntityToCache(JC_MbInfo item)
        {
            if (_cache.Count(alarm => alarm.Id == item.Id) < 1)
            {
                _cache.Add(item);
            }
        }

        protected override void DeleteEntityFromCache(JC_MbInfo item)
        {
            var alarmitem = _cache.FirstOrDefault(alarm => alarm.Id == item.Id);
            if (alarmitem != null)
            {
                _cache.Remove(alarmitem);
            }
        }

        protected override void UpdateEntityToCache(JC_MbInfo item)
        {
            int itemIndex = _cache.FindIndex(alarm => alarm.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }

        public void UpdateAlarmInfoProperties(string AlarmKey, Dictionary<string, object> updateItems)
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                JC_MbInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.Id == AlarmKey);
                if (iteminfo != null)
                {
                    iteminfo.CopyProperties(updateItems);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新报警缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}

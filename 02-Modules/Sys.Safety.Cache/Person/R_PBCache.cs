using Basic.Framework.Logging;
using Basic.Framework.Service;
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
    /// 时间:2017-05-22
    /// 描述:报警缓存
    /// 修改记录
    /// 2017-02-22
    /// 2017-05-23
    /// 2017-05-24
    /// 2017-05-25
    /// </summary>
    public class R_PBCache : KJ73NCacheManager<R_PbInfo>
    {
        private readonly IR_PbService r_pbService;

        private static volatile R_PBCache r_PBCahceInstance;
        /// <summary>
        /// 报警单例
        /// </summary>
        public static R_PBCache CahceInstance
        {
            get
            {
                if (r_PBCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (r_PBCahceInstance == null)
                        {
                            r_PBCahceInstance = new R_PBCache();
                        }
                    }
                }
                return r_PBCahceInstance;
            }
        }

        private R_PBCache()
        {
            r_pbService = ServiceFactory.Create<IR_PbService>();
        }

        public override void Load()
        {
            var respose = r_pbService.GetAlarmedDataList();
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                _cache = respose.Data;
            }
        }
        /// <summary>
        /// 清除报警缓存
        /// </summary>
        public void ClearUp()
        {
            _cache.Clear();
        }

        protected override void AddEntityToCache(R_PbInfo item)
        {
            if (_cache.Count(alarm => alarm.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(R_PbInfo item)
        {
            var alarmitem = _cache.FirstOrDefault(alarm => alarm.Id == item.Id);
            if (alarmitem != null)
                _cache.Remove(alarmitem);
        }

        protected override void UpdateEntityToCache(R_PbInfo item)
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
                R_PbInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.Id == AlarmKey);
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

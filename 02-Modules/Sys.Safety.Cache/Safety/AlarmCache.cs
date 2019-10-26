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
    public class AlarmCache : KJ73NCacheManager<Jc_BInfo>
    {
        private readonly IAlarmRecordService alarmService;

        private static volatile AlarmCache alarmCahceInstance;
        /// <summary>
        /// 报警单例
        /// </summary>
        public static AlarmCache AlarmCacheInstance
        {
            get
            {
                if (alarmCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (alarmCahceInstance == null)
                        {
                            alarmCahceInstance = new AlarmCache();
                        }
                    }
                }
                return alarmCahceInstance;
            }
        }

        private AlarmCache()
        {
            alarmService = ServiceFactory.Create<IAlarmRecordService>();
        }

        public override void Load()
        {
            try
            {
                var respose = alarmService.GetAlarmedDataList();
                if (respose.Data != null && respose.IsSuccess)
                {
                    _cache.Clear();
                    _cache = respose.Data;
                }
                ////追加人员定位的报警数据到缓存中  20171208
                //var resposePerson = alarmService.GetR_AlarmedDataList();
                //if (respose.Data != null && respose.IsSuccess)
                //{
                //    _cache.AddRange(resposePerson.Data);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
            }
        }

        /// <summary>
        /// 清除报警缓存
        /// </summary>
        public void ClearUp()
        {
            _cache.Clear();
        }

        protected override void AddEntityToCache(Jc_BInfo item)
        {
            if (_cache.Count(alarm => alarm.ID == item.ID) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_BInfo item)
        {
            var alarmitem = _cache.FirstOrDefault(alarm => alarm.ID == item.ID);
            if (alarmitem != null)
                _cache.Remove(alarmitem);
        }

        protected override void UpdateEntityToCache(Jc_BInfo item)
        {
            int itemIndex = _cache.FindIndex(alarm => alarm.ID == item.ID);
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
                Jc_BInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.ID == AlarmKey);
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

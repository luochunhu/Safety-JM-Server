using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Setting;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-23
    /// 描述:设置缓存
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public class SettingCache : KJ73NCacheManager<SettingInfo>
    {
        private readonly ISettingService settingService;

        private static volatile SettingCache settingCahceInstance;
        /// <summary>
        /// 设置缓存单例
        /// </summary>
        public static SettingCache SettingCahceInstance
        {
            get
            {
                if (settingCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (settingCahceInstance == null)
                        {
                            settingCahceInstance = new SettingCache();
                        }
                    }
                }
                return settingCahceInstance;
            }
        }

        private SettingCache()
        {
            settingService = ServiceFactory.Create<ISettingService>();
        }

        public override void Load()
        {
            //try
            //{
            var respose = settingService.GetSettingList();
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

        protected override void AddEntityToCache(SettingInfo item)
        {
            if (_cache.Count(i => i.StrKey == item.StrKey) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(SettingInfo item)
        {
            var tempitem = _cache.FirstOrDefault(i => i.StrKey == item.StrKey);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(SettingInfo item)
        {
            int itemIndex = _cache.FindIndex(i => i.StrKey == item.StrKey);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Config;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-23
    /// 描述:配置缓存
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public class ConfigCache : KJ73NCacheManager<ConfigInfo>
    {
        private readonly IConfigService configService;

        private static volatile ConfigCache configCahceInstance;
        /// <summary>
        /// 配置缓存单例
        /// </summary>
        public static ConfigCache ConfigCacheInstance
        {
            get
            {
                if (configCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (configCahceInstance == null)
                        {
                            configCahceInstance = new ConfigCache();
                        }
                    }
                }
                return configCahceInstance;
            }
        }

        private ConfigCache()
        {
            configService = ServiceFactory.Create<IConfigService>();
        }

        public override void Load()
        {
            //try
            //{
                var respose = configService.GetConfigList();
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

        protected override void AddEntityToCache(ConfigInfo item)
        {
            if (_cache.Count(i => i.Name == item.Name) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(ConfigInfo item)
        {
            var tempitem = _cache.FirstOrDefault(i => i.Name == item.Name);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(ConfigInfo item)
        {
            int itemIndex = _cache.FindIndex(i => i.Name == item.Name);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

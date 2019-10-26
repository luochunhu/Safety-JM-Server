using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:设备定义缓存
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class DeviceDefineCache : KJ73NCacheManager<Jc_DevInfo>
    {
        private readonly IDeviceDefineService deviceDefineService;

        private static volatile DeviceDefineCache deviceDefineCahceInstance;
        /// <summary>
        /// 设备定义单例
        /// </summary>
        public static DeviceDefineCache DeviceDefineCahceInstance
        {
            get
            {
                if (deviceDefineCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (deviceDefineCahceInstance == null)
                        {
                            deviceDefineCahceInstance = new DeviceDefineCache();
                        }
                    }
                }
                return deviceDefineCahceInstance;
            }
        }

        private DeviceDefineCache()
        {
            deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        }

        public override void Load()
        {
            //try
            //{
                var respose = deviceDefineService.GetDeviceDefineList();
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

        protected override void AddEntityToCache(Jc_DevInfo item)
        {
            if (_cache.Count(devdefine => devdefine.Devid == item.Devid) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_DevInfo item)
        {
            var tempitem = _cache.FirstOrDefault(devdefine => devdefine.Devid == item.Devid);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(Jc_DevInfo item)
        {
            int itemIndex = _cache.FindIndex(devdefine => devdefine.Devid == item.Devid);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}

using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:设备种类缓存
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class DeviceClassCache : KJ73NCacheManager<EnumcodeInfo>
    {
        private readonly IEnumcodeService enumCodeService;

        private static volatile DeviceClassCache deviceClassCahceInstance;
        /// <summary>
        /// 设备种类单例
        /// </summary>
        public static DeviceClassCache DeviceClassCahceInstance
        {
            get
            {
                if (deviceClassCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (deviceClassCahceInstance == null)
                        {
                            deviceClassCahceInstance = new DeviceClassCache();
                        }
                    }
                }
                return deviceClassCahceInstance;
            }
        }

        private DeviceClassCache()
        {
            enumCodeService = ServiceFactory.Create<IEnumcodeService>();
        }

        public override void Load()
        {
            //try
            //{              
                var respose = enumCodeService.GetEnumcodeList();
                if (respose.Data != null && respose.IsSuccess)
                {
                    _cache.Clear();
                    _cache = respose.Data.Where(en => en.EnumTypeID == ((int)EnumTypeEnum.DeviceClass).ToString()).ToList();
                }
            //}
            //catch(Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message,CacheName));
            //}
        }

        protected override void AddEntityToCache(EnumcodeInfo item)
        {

        }

        protected override void DeleteEntityFromCache(EnumcodeInfo item)
        {

        }

        protected override void UpdateEntityToCache(EnumcodeInfo item)
        {

        }
    }
}

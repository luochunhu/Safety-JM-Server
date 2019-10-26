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
    /// 描述:设备型号缓存
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class DeviceTypeCache : KJ73NCacheManager<EnumcodeInfo>
    {
        private readonly IEnumcodeService enumCodeService;

        private static volatile DeviceTypeCache deviceTypeCahceInstance;
        /// <summary>
        /// 设备型号单例
        /// </summary>
        public static DeviceTypeCache DeviceTypeCahceInstance
        {
            get
            {
                if (deviceTypeCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (deviceTypeCahceInstance == null)
                        {
                            deviceTypeCahceInstance = new DeviceTypeCache();
                        }
                    }
                }
                return deviceTypeCahceInstance;
            }
        }

        private DeviceTypeCache()
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
                    _cache = respose.Data.Where(en => en.EnumTypeID == ((int)EnumTypeEnum.DeviceType).ToString()).ToList();
                }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
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

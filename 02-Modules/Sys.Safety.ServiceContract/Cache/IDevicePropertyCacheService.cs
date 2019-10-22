using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-23
    /// 描述:设备性质缓存业务RPC接口
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public interface IDevicePropertyCacheService
    {
        /// <summary>
        /// 加载设备性质缓存
        /// </summary>
        /// <param name="devicePropertyCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadDevicePropertyCache(DevicePropertyCacheLoadRequest devicePropertyCacheRequest);

        /// <summary>
        /// 获取所有设备性质
        /// </summary>
        /// <param name="devicePropertyCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache(DevicePropertyCacheGetAllRequest devicePropertyCacheRequest);

        /// <summary>
        /// 根据条件获取设备性质
        /// </summary>
        /// <param name="devicePropertyCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetDevicePropertyCache(DevicePropertyCacheGetByConditionRequest devicePropertyCacheRequest);

        /// <summary>
        /// 根据Key获取设备性质
        /// </summary>
        /// <param name="devicePropertyCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<EnumcodeInfo> GetDevicePropertyByKey(DevicePropertyCacheGetByKeyRequest devicePropertyCacheRequest);

        /// <summary>
        /// 设备性质是否存在
        /// </summary>
        /// <param name="devicePropertyCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsDeviceProperty(DevicePropertyCacheIsExistsRequest deviceTypeCacheRequest);
    }
}

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
    /// 时间:2017-05-22
    /// 描述:设备种类缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface IDeviceClassCacheService
    {
        /// <summary>
        /// 加载设备种类缓存
        /// </summary>
        /// <param name="deviceClassCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadDeviceClassCache(DeviceClassCacheLoadRequest deviceClassCacheRequest);

        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <param name="deviceClassCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache(DeviceClassCacheGetAllRequest deviceClassCacheRequest);

        /// <summary>
        /// 根据条件获取设备种类
        /// </summary>
        /// <param name="deviceClassCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetDeviceClassCache(DeviceClassCacheGetByConditionRequest deviceClassCacheRequest);

        /// <summary>
        /// 根据设备性质获取设备种类
        /// </summary>
        /// <param name="deviceClassCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetDeviceClassByDeviciProperty(DeviceClassCacheGetByDevicePropertyRequest deviceClassCacheRequest);

        /// <summary>
        /// 根据Key获取设备种类
        /// </summary>
        /// <param name="deviceClassCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<EnumcodeInfo> GetDeviceClassByKey(DeviceClassCacheGetByKeyRequest deviceClassCacheRequest);

        /// <summary>
        /// 设备种类是否存在
        /// </summary>
        /// <param name="deviceClassCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsDeviceClass(DeviceClassCacheIsExistsRequest deviceClassCacheRequest);

    }
}

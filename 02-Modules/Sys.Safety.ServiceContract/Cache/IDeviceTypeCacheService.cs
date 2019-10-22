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
    /// 时间:2017-05-2
    /// 描述:设备型号缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface IDeviceTypeCacheService
    {
        /// <summary>
        /// 加载设备型号缓存
        /// </summary>
        /// <param name="deviceTypeCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadDeviceTypeCache(DeviceTypeCacheLoadRequest deviceTypeCacheRequest);

        /// <summary>
        /// 获取所有设备型号
        /// </summary>
        /// <param name="deviceTypeCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDeviceTypeCache(DeviceTypeCacheGetAllRequest deviceTypeCacheRequest);

        /// <summary>
        /// 根据条件获取设备型号
        /// </summary>
        /// <param name="deviceTypeCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetDeviceTypeCache(DeviceTypeCacheGetByConditionRequest deviceTypeCacheRequest);

        /// <summary>
        /// 根据Key获取设备型号
        /// </summary>
        /// <param name="deviceTypeCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<EnumcodeInfo> GetDeviceTypeByKey(DeviceTypeCacheGetByKeyRequest deviceTypeCacheRequest);

        /// <summary>
        /// 设备型号是否存在
        /// </summary>
        /// <param name="deviceTypeCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsDeviceType(DeviceTypeCacheIsExistsRequest deviceTypeCacheRequest);
    }
}

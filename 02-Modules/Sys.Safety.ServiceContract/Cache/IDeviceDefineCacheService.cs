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
    /// 描述:设备定义缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface IDeviceDefineCacheService
    {
        /// <summary>
        /// 加载设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadDeviceDefineCache(DeviceDefineCacheLoadRequest deviceDefineCacheRequest);

        /// <summary>
        /// 添加设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddPointDefineCache(DeviceDefineCacheAddRequest deviceDefineCacheRequest);

        /// <summary>
        /// 批量添加设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BacthAddPointDefineCache(DeviceDefineCacheBatchAddRequest deviceDefineCacheRequest);

        /// <summary>
        /// 更新设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefineCahce(DeviceDefineCacheUpdateRequest deviceDefineCacheRequest);

        /// <summary>
        /// 批量更新设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdatePointDefineCache(DeviceDefineCacheBatchUpdateRequest deviceDefineCacheRequest);

        /// <summary>
        /// 删除设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeletePointDefineCache(DeviceDefineCacheDeleteRequest deviceDefineCacheRequest);

        /// <summary>
        /// 获取所有设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DevInfo>> GetAllPointDefineCache(DeviceDefineCacheGetAllRequest deviceDefineCacheRequest);

        /// <summary>
        /// 根据Key(Point)获取设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_DevInfo> GetPointDefineCacheByKey(DeviceDefineCacheGetByKeyRequest deviceDefineCacheRequest);

        /// <summary>
        /// 获取设备定义缓存
        /// </summary>
        /// <param name="deviceDefineCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<Jc_DevInfo>> GetPointDefineCache(DeviceDefineCacheGetByConditonRequest deviceDefineCacheRequest);
    }
}

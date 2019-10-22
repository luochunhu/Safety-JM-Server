using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.KJ237Cache
{
    /// <summary>
    /// 人员定位未定义设备缓存
    /// </summary>
    public interface IRUndefinedDefCacheService
    {
        /// <summary>
        /// 加载未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadRUndefinedDefCache(RUndefinedDefCacheLoadRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 添加未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddRUndefinedDefCache(RUndefinedDefCacheAddRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 批量添加未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddRUndefinedDefCache(RUndefinedDefCacheBatchAddRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 更新未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRUndefinedDefCache(RUndefinedDefCacheUpdateRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 批量更新未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateRUndefinedDefCache(RUndefinedDefCacheBatchUpdateRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 删除未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRUndefinedDefCache(RUndefinedDefCacheDeleteRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 批量删除未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteRUndefinedDefCache(RUndefinedDefCacheBatchDeleteRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 获取所有未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_UndefinedDefInfo>> GetAllRUndefinedDefCache(RUndefinedDefCacheGetAllRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 根据Key获取未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<R_UndefinedDefInfo> GetByKeyRUndefinedDefCache(RUndefinedDefCacheGetByKeyRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 根据条件获取未定义设备缓存
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_UndefinedDefInfo>> GetRUndefinedDefCache(RUndefinedDefCacheGetByConditionRequest RUndefinedDefCacheRequest);

        /// <summary>
        /// 未定义设备缓存是否存在
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsRUndefinedDefCache(RUndefinedDefCacheIsExistsRequest RUndefinedDefCacheRequest);
    }
}

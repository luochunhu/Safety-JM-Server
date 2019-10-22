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
    /// 人员定位班次缓存
    /// </summary>
    public interface IRKqbcCacheService
    {
        /// <summary>
        /// 加载班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadRKqbcCache(RKqbcCacheLoadRequest RKqbcCacheRequest);

        /// <summary>
        /// 添加班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddRKqbcCache(RKqbcCacheAddRequest RKqbcCacheRequest);

        /// <summary>
        /// 批量添加班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddRKqbcCache(RKqbcCacheBatchAddRequest RKqbcCacheRequest);

        /// <summary>
        /// 更新班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRKqbcCache(RKqbcCacheUpdateRequest RKqbcCacheRequest);

        /// <summary>
        /// 批量更新班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateRKqbcCache(RKqbcCacheBatchUpdateRequest RKqbcCacheRequest);

        /// <summary>
        /// 删除班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRKqbcCache(RKqbcCacheDeleteRequest RKqbcCacheRequest);

        /// <summary>
        /// 批量删除班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteRKqbcCache(RKqbcCacheBatchDeleteRequest RKqbcCacheRequest);

        /// <summary>
        /// 获取所有班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_KqbcInfo>> GetAllRKqbcCache(RKqbcCacheGetAllRequest RKqbcCacheRequest);

        /// <summary>
        /// 根据Key获取班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<R_KqbcInfo> GetByKeyRKqbcCache(RKqbcCacheGetByKeyRequest RKqbcCacheRequest);

        /// <summary>
        /// 根据条件获取班次缓存
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_KqbcInfo>> GetRKqbcCache(RKqbcCacheGetByConditionRequest RKqbcCacheRequest);

        /// <summary>
        /// 班次缓存是否存在
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsRKqbcCache(RKqbcCacheIsExistsRequest RKqbcCacheRequest);
    }
}

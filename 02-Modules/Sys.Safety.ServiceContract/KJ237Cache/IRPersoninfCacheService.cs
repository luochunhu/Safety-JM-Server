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
    public interface IRPersoninfCacheService
    {
        /// <summary>
        /// 加载人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadRPersoninfCache(RPersoninfCacheLoadRequest RPersoninfCacheRequest);

        /// <summary>
        /// 添加人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddRPersoninfCache(RPersoninfCacheAddRequest RPersoninfCacheRequest);

        /// <summary>
        /// 批量添加人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddRPersoninfCache(RPersoninfCacheBatchAddRequest RPersoninfCacheRequest);

        /// <summary>
        /// 更新人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRPersoninfCache(RPersoninfCacheUpdateRequest RPersoninfCacheRequest);

        /// <summary>
        /// 批量更新人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateRPersoninfCache(RPersoninfCacheBatchUpdateRequest RPersoninfCacheRequest);

        /// <summary>
        /// 删除人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRPersoninfCache(RPersoninfCacheDeleteRequest RPersoninfCacheRequest);

        /// <summary>
        /// 批量删除人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteRPersoninfCache(RPersoninfCacheBatchDeleteRequest RPersoninfCacheRequest);

        /// <summary>
        /// 获取所有人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_PersoninfInfo>> GetAllRPersoninfCache(RPersoninfCacheGetAllRequest RPersoninfCacheRequest);

        /// <summary>
        /// 根据Key获取人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<R_PersoninfInfo> GetByKeyRPersoninfCache(RPersoninfCacheGetByKeyRequest RPersoninfCacheRequest);

        /// <summary>
        /// 根据条件获取人员基本信息缓存
        /// </summary>
        /// <param name="RPersoninfCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_PersoninfInfo>> GetRPersoninfCache(RPersoninfCacheGetByConditionRequest RPersoninfCacheRequest);
    }
}

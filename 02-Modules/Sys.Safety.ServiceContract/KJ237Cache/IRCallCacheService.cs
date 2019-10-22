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
    /// 人员定位控制缓存
    /// </summary>
    public interface IRCallCacheService
    {
        /// <summary>
        /// 加载呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadRCallCache(RCallCacheLoadRequest RCallCacheRequest);

        /// <summary>
        /// 添加呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddRCallCache(RCallCacheAddRequest RCallCacheRequest);

        /// <summary>
        /// 批量添加呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddRCallCache(RCallCacheBatchAddRequest RCallCacheRequest);

        /// <summary>
        /// 更新呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRCallCache(RCallCacheUpdateRequest RCallCacheRequest);

        /// <summary>
        /// 批量更新呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateRCallCache(RCallCacheBatchUpdateRequest RCallCacheRequest);

        /// <summary>
        /// 删除呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRCallCache(RCallCacheDeleteRequest RCallCacheRequest);

        /// <summary>
        /// 批量删除呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteRCallCache(RCallCacheBatchDeleteRequest RCallCacheRequest);

        /// <summary>
        /// 获取所有呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_CallInfo>> GetAllRCallCache(RCallCacheGetAllRequest RCallCacheRequest);

        /// <summary>
        /// 根据Key获取呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<R_CallInfo> GetByKeyRCallCache(RCallCacheGetByKeyRequest RCallCacheRequest);

        /// <summary>
        /// 根据条件获取呼叫控制缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_CallInfo>> GetRCallCache(RCallCacheGetByConditionRequest RCallCacheRequest);

        /// <summary>
        /// 呼叫控制缓存是否存在
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsRCallCache(RCallCacheIsExistsRequest RCallCacheRequest);
    }
}

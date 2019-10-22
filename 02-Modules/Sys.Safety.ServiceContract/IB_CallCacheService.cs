using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    public interface IB_CallCacheService
    {
        /// <summary>
        /// 加载广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadBCallCache(BCallCacheLoadRequest BCallCacheRequest);

        /// <summary>
        /// 添加广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddBCallCache(BCallCacheAddRequest BCallCacheRequest);

        /// <summary>
        /// 批量添加广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddBCallCache(BCallCacheBatchAddRequest BCallCacheRequest);

        /// <summary>
        /// 更新广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateBCallCache(BCallCacheUpdateRequest BCallCacheRequest);

        /// <summary>
        /// 批量更新广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateBCallCache(BCallCacheBatchUpdateRequest BCallCacheRequest);

        /// <summary>
        /// 删除广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteBCallCache(BCallCacheDeleteRequest BCallCacheRequest);

        /// <summary>
        /// 批量删除广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteBCallCache(BCallCacheBatchDeleteRequest BCallCacheRequest);

        /// <summary>
        /// 获取所有广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<B_CallInfo>> GetAllBCallCache(BCallCacheGetAllRequest BCallCacheRequest);

        /// <summary>
        /// 根据Key获取广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<B_CallInfo> GetByKeyBCallCache(BCallCacheGetByKeyRequest BCallCacheRequest);

        /// <summary>
        /// 根据条件获取广播控制缓存
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<B_CallInfo>> GetBCallCache(BCallCacheGetByConditionRequest BCallCacheRequest);

        /// <summary>
        /// 广播控制缓存是否存在
        /// </summary>
        /// <param name="BCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsBCallCache(BCallCacheIsExistsRequest BCallCacheRequest);

        BasicResponse BatchUpdatePointInfo(BatchUpdatePointInfoRequest Request);
    }
}

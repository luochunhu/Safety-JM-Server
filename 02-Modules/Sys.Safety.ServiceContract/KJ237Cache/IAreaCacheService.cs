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
    public interface IAreaCacheService
    {
        /// <summary>
        /// 加载班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadAreaCache(AreaCacheLoadRequest AreaCacheRequest);

        /// <summary>
        /// 添加班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddAreaCache(AreaCacheAddRequest AreaCacheRequest);

        /// <summary>
        /// 批量添加班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddAreaCache(AreaCacheBatchAddRequest AreaCacheRequest);

        /// <summary>
        /// 更新班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateAreaCache(AreaCacheUpdateRequest AreaCacheRequest);

        /// <summary>
        /// 批量更新班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateAreaCache(AreaCacheBatchUpdateRequest AreaCacheRequest);

        /// <summary>
        /// 删除班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteAreaCache(AreaCacheDeleteRequest AreaCacheRequest);

        /// <summary>
        /// 批量删除班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteAreaCache(AreaCacheBatchDeleteRequest AreaCacheRequest);

        /// <summary>
        /// 获取所有班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<AreaInfo>> GetAllAreaCache(AreaCacheGetAllRequest AreaCacheRequest);

        /// <summary>
        /// 根据Key获取班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<AreaInfo> GetByKeyAreaCache(AreaCacheGetByKeyRequest AreaCacheRequest);

        /// <summary>
        /// 根据条件获取班次缓存
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<AreaInfo>> GetAreaCache(AreaCacheGetByConditionRequest AreaCacheRequest);

        /// <summary>
        /// 班次缓存是否存在
        /// </summary>
        /// <param name="AreaCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsAreaCache(AreaCacheIsExistsRequest AreaCacheRequest);
    }
}

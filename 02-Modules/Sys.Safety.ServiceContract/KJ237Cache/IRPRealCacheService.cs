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
    /// 人员定位实时信息缓存
    /// </summary>
    public interface IRPRealCacheService
    {
        /// <summary>
        /// 加载人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadRRealCache(RPralCacheLoadRequest RealCacheRequest);

        /// <summary>
        /// 添加人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddRRealCache(RPrealCacheAddRequest RealCacheRequest);

        /// <summary>
        /// 批量添加人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BacthAddRRealCache(RPrealCacheBatchAddRequest RealCacheRequest);

        /// <summary>
        /// 更新人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRRealCahce(RPrealCacheUpdateRequest RealCacheRequest);

        /// <summary>
        /// 批量更新人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchRUpdateRealCache(RPrealCacheBatchUpdateRequest RealCacheRequest);

        /// <summary>
        /// 删除人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRRealCache(RPrealCacheDeleteRequest RealCacheRequest);

        /// <summary>
        /// 批量删除人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteRRealCache(RPrealCacheBatchDeleteRequest  RealCacheRequest);

        /// <summary>
        /// 获取所有人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_PrealInfo>> GetAllRRealCache(RPrealCacheGetAllRequest RealCacheRequest);

        /// <summary>
        /// 根据测点ID获取人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<R_PrealInfo> RealCacheByPointIdRequeest(RPrealCacheByPointIdRequeest RealCacheRequest);

        /// <summary>
        /// 获取人员实时信息信息缓存
        /// </summary>
        /// <param name="RealCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<R_PrealInfo>> GetRealCache(RPrealCacheGetByConditonRequest RealCacheRequest);

        /// <summary>
        /// 部分更新测点信息缓存
        /// </summary>
        /// <param name="RealCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRRealInfo(RPrealCacheUpdatePropertiesRequest RealCacheRequest);

        /// <summary>
        /// 批量更新测点缓存字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateRealInfo(RPrealCacheBatchUpdatePropertiesRequest request);

    }
}

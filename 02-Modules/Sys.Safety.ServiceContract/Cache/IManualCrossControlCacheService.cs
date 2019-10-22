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
    /// 描述:手动交叉控制缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface IManualCrossControlCacheService
    {
        /// <summary>
        /// 加载手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadManualCrossControlCache(ManualCrossControlCacheLoadRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 添加手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddManualCrossControlCache(ManualCrossControlCacheAddRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 批量添加手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddManualCrossControlCache(ManualCrossControlCacheBatchAddRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 更新手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateManualCrossControlCache(ManualCrossControlCacheUpdateRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 批量更新手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateManualCrossControlCache(ManualCrossControlCacheBatchUpdateRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 删除手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteManualCrossControlCache(ManualCrossControlCacheDeleteRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 批量删除手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteManualCrossControlCache(ManualCrossControlCacheBatchDeleteRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 获取所有手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControlCache(ManualCrossControlCacheGetAllRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 根据Key获取手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_JcsdkzInfo> GetByKeyManualCrossControlCache(ManualCrossControlCacheGetByKeyRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 根据条件获取手动交叉控制缓存
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlCache(ManualCrossControlCacheGetByConditionRequest manualCrossControlCacheRequest);

        /// <summary>
        /// 手动交叉控制缓存是否存在
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsManualCrossControlCache(ManualCrossControlCacheIsExistsRequest manualCrossControlCacheRequest);
    }
}

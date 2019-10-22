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
    /// 描述:安装位置缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface IPositionCacheService
    {
        /// <summary>
        /// 加载安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadPositionCache(PositonCacheLoadRequest positionCacheRequest);

        /// <summary>
        /// 添加安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddPositionCache(PositionCacheAddRequest positionCacheRequest);

        /// <summary>
        /// 批量添加安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddPositionCache(PositionCacheBatchAddRequest positionCacheRequest);

        /// <summary>
        /// 更新安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePositionCache(PositionCacheUpdateRequest positionCacheRequest);

        /// <summary>
        /// 批量更新安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdatePositionCache(PositionCacheBatchUpdateRequest positionCacheRequest);

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeletePositionCache(PositionCacheDeleteRequest positionCacheRequest);

        /// <summary>
        /// 获取所有安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_WzInfo>> GetAllPositongCache(PositionCacheGetAllRequest positionCacheRequest);

        /// <summary>
        /// 根据Key获取安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_WzInfo> GetPositionCacheByKey(PositionCacheGetByKeyRequest positionCacheRequest);

        /// <summary>
        /// 根据条件获取安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_WzInfo>> GetPositionCache(PositionCacheGetByConditionRequest positionCacheRequest);

        /// <summary>
        /// 安装位置缓存是否存在
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> IsExistsPositionCache(PositionCacheIsExistsRequest positionCacheRequest);

        /// <summary>
        /// 获取安装位置缓存最大Id
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<string> GetMaxPositiongId(PositionCacheGetMaxIdRequest positionCacheRequest);
    }
}

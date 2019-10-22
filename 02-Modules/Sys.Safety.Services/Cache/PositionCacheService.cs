using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:安装位置缓存业务
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class PositionCacheService : IPositionCacheService
    {
        public BasicResponse AddPositionCache(PositionCacheAddRequest positionCacheRequest)
        {
            PositionCache.PositionCahceInstance.AddItem(positionCacheRequest.PositionInfo);
            return new BasicResponse();
        }

        public BasicResponse BatchAddPositionCache(PositionCacheBatchAddRequest positionCacheRequest)
        {
            PositionCache.PositionCahceInstance.AddItems(positionCacheRequest.PositionInfos);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdatePositionCache(PositionCacheBatchUpdateRequest positionCacheRequest)
        {
            PositionCache.PositionCahceInstance.UpdateItems(positionCacheRequest.PositionInfos);
            return new BasicResponse();
        }

        public BasicResponse DeletePositionCache(PositionCacheDeleteRequest positionCacheRequest)
        {
            PositionCache.PositionCahceInstance.DeleteItem(positionCacheRequest.PositionInfo);
            return new BasicResponse();
        }

        public BasicResponse<List<Jc_WzInfo>> GetAllPositongCache(PositionCacheGetAllRequest positionCacheRequest)
        {
            var positionCache = PositionCache.PositionCahceInstance.Query();
            var positionCacheResponse = new BasicResponse<List<Jc_WzInfo>>();
            positionCacheResponse.Data = positionCache;
            return positionCacheResponse;
        }

        public BasicResponse<string> GetMaxPositiongId(PositionCacheGetMaxIdRequest positionCacheRequest)
        {
            var positionCache = PositionCache.PositionCahceInstance.Query().Max(position=>Convert.ToInt64(position.WzID));
            var positionCacheResponse = new BasicResponse<string>();
            positionCacheResponse.Data = positionCache.ToString() ;
            return positionCacheResponse;
        }

        public BasicResponse<List<Jc_WzInfo>> GetPositionCache(PositionCacheGetByConditionRequest positionCacheRequest)
        {
            var positionCache = PositionCache.PositionCahceInstance.Query(positionCacheRequest.Predicate);
            var positionCacheResponse = new BasicResponse<List<Jc_WzInfo>>();
            positionCacheResponse.Data = positionCache;
            return positionCacheResponse;
        }

        public BasicResponse<Jc_WzInfo> GetPositionCacheByKey(PositionCacheGetByKeyRequest positionCacheRequest)
        {
            var positionCache = PositionCache.PositionCahceInstance.Query(position=>position.WzID== positionCacheRequest.PositionId).FirstOrDefault();
            var positionCacheResponse = new BasicResponse<Jc_WzInfo>();
            positionCacheResponse.Data = positionCache;
            return positionCacheResponse;
        }

        public BasicResponse<bool> IsExistsPositionCache(PositionCacheIsExistsRequest positionCacheRequest)
        {
            var positionCache = PositionCache.PositionCahceInstance.Query(position => position.WzID == positionCacheRequest.PositionId).FirstOrDefault();
            var positionCacheResponse = new BasicResponse<bool>();
            positionCacheResponse.Data = positionCache!=null;
            return positionCacheResponse;
        }

        public BasicResponse LoadPositionCache(PositonCacheLoadRequest positionCacheRequest)
        {
            PositionCache.PositionCahceInstance.Load();
            return new BasicResponse();
        }

        public BasicResponse UpdatePositionCache(PositionCacheUpdateRequest positionCacheRequest)
        {
            PositionCache.PositionCahceInstance.UpdateItem(positionCacheRequest.PositionInfo);
            return new BasicResponse();
        }
    }
}

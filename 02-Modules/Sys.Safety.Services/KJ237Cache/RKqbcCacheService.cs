using Basic.Framework.Web;
using Sys.Safety.Cache.Person;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract.KJ237Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.KJ237Cache
{
    public class RKqbcCacheService : IRKqbcCacheService
    {
        public Basic.Framework.Web.BasicResponse LoadRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheLoadRequest RKqbcCacheRequest)
        {
            RKqbcCache.RKqbcCacheInstance.Load();
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheAddRequest RKqbcCacheRequest)
        {
            RKqbcCache.RKqbcCacheInstance.AddItem(RKqbcCacheRequest.RKqbcInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchAddRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheBatchAddRequest RKqbcCacheRequest)
        {
            RKqbcCache.RKqbcCacheInstance.AddItems(RKqbcCacheRequest.RKqbcInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheUpdateRequest RKqbcCacheRequest)
        {
            RKqbcCache.RKqbcCacheInstance.UpdateItem(RKqbcCacheRequest.RKqbcInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheBatchUpdateRequest RKqbcCacheRequest)
        {
            RKqbcCache.RKqbcCacheInstance.UpdateItems(RKqbcCacheRequest.RKqbcInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheDeleteRequest RKqbcCacheRequest)
        {
            RKqbcCache.RKqbcCacheInstance.DeleteItem(RKqbcCacheRequest.RKqbcInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDeleteRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheBatchDeleteRequest RKqbcCacheRequest)
        {
            RKqbcCache.RKqbcCacheInstance.DeleteItems(RKqbcCacheRequest.RKqbcInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_KqbcInfo>> GetAllRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheGetAllRequest RKqbcCacheRequest)
        {
            var _RKqbcCache = RKqbcCache.RKqbcCacheInstance.Query();
            var RKqbcCacheResponse = new BasicResponse<List<R_KqbcInfo>>();
            RKqbcCacheResponse.Data = _RKqbcCache;
            return RKqbcCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_KqbcInfo> GetByKeyRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheGetByKeyRequest RKqbcCacheRequest)
        {
            var _RKqbcCache = RKqbcCache.RKqbcCacheInstance.Query(o => o.Bcid == RKqbcCacheRequest.Bcid).FirstOrDefault();
            var RKqbcCacheResponse = new BasicResponse<R_KqbcInfo>();
            RKqbcCacheResponse.Data = _RKqbcCache;
            return RKqbcCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_KqbcInfo>> GetRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheGetByConditionRequest RKqbcCacheRequest)
        {
            var _RKqbcCache = RKqbcCache.RKqbcCacheInstance.Query(RKqbcCacheRequest.Predicate);
            var RKqbcCacheResponse = new BasicResponse<List<R_KqbcInfo>>();
            RKqbcCacheResponse.Data = _RKqbcCache;
            return RKqbcCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<bool> IsExistsRKqbcCache(Sys.Safety.Request.PersonCache.RKqbcCacheIsExistsRequest RKqbcCacheRequest)
        {
            var _RKqbcCache = RKqbcCache.RKqbcCacheInstance.Query(call => call.Bcid == RKqbcCacheRequest.Bcid).FirstOrDefault();
            var RKqbcCacheResponse = new BasicResponse<bool>();
            RKqbcCacheResponse.Data = _RKqbcCache != null;
            return RKqbcCacheResponse;
        }
    }
}

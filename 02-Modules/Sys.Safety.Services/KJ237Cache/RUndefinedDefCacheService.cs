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
    public class RUndefinedDefCacheService : IRUndefinedDefCacheService
    {
        public Basic.Framework.Web.BasicResponse LoadRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheLoadRequest RUndefinedDefCacheRequest)
        {
            RUndefinedDefCache.RUndefinedDefCacheInstance.Load();
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheAddRequest RUndefinedDefCacheRequest)
        {
            RUndefinedDefCache.RUndefinedDefCacheInstance.AddItem(RUndefinedDefCacheRequest.RUndefinedDefInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchAddRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheBatchAddRequest RUndefinedDefCacheRequest)
        {
            RUndefinedDefCache.RUndefinedDefCacheInstance.AddItems(RUndefinedDefCacheRequest.RUndefinedDefInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheUpdateRequest RUndefinedDefCacheRequest)
        {
            RUndefinedDefCache.RUndefinedDefCacheInstance.UpdateItem(RUndefinedDefCacheRequest.RUndefinedDefInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheBatchUpdateRequest RUndefinedDefCacheRequest)
        {
            RUndefinedDefCache.RUndefinedDefCacheInstance.UpdateItems(RUndefinedDefCacheRequest.RUndefinedDefInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheDeleteRequest RUndefinedDefCacheRequest)
        {
            RUndefinedDefCache.RUndefinedDefCacheInstance.DeleteItem(RUndefinedDefCacheRequest.RUndefinedDefInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDeleteRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheBatchDeleteRequest RUndefinedDefCacheRequest)
        {
            RUndefinedDefCache.RUndefinedDefCacheInstance.DeleteItems(RUndefinedDefCacheRequest.RUndefinedDefInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_UndefinedDefInfo>> GetAllRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheGetAllRequest RUndefinedDefCacheRequest)
        {
            var _RUndefinedDefCache = RUndefinedDefCache.RUndefinedDefCacheInstance.Query();
            var RUndefinedDefCacheResponse = new BasicResponse<List<R_UndefinedDefInfo>>();
            RUndefinedDefCacheResponse.Data = _RUndefinedDefCache;
            return RUndefinedDefCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_UndefinedDefInfo> GetByKeyRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheGetByKeyRequest RUndefinedDefCacheRequest)
        {
            var _RUndefinedDefCache = RUndefinedDefCache.RUndefinedDefCacheInstance.Query(o => o.Id == RUndefinedDefCacheRequest.Id).FirstOrDefault();
            var RUndefinedDefCacheResponse = new BasicResponse<R_UndefinedDefInfo>();
            RUndefinedDefCacheResponse.Data = _RUndefinedDefCache;
            return RUndefinedDefCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_UndefinedDefInfo>> GetRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheGetByConditionRequest RUndefinedDefCacheRequest)
        {
            var _RUndefinedDefCache = RUndefinedDefCache.RUndefinedDefCacheInstance.Query(RUndefinedDefCacheRequest.Predicate);
            var RUndefinedDefCacheResponse = new BasicResponse<List<R_UndefinedDefInfo>>();
            RUndefinedDefCacheResponse.Data = _RUndefinedDefCache;
            return RUndefinedDefCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<bool> IsExistsRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheIsExistsRequest RUndefinedDefCacheRequest)
        {
            var _RUndefinedDefCache = RUndefinedDefCache.RUndefinedDefCacheInstance.Query(call => call.Id == RUndefinedDefCacheRequest.Id).FirstOrDefault();
            var RUndefinedDefCacheResponse = new BasicResponse<bool>();
            RUndefinedDefCacheResponse.Data = _RUndefinedDefCache != null;
            return RUndefinedDefCacheResponse;
        }
    }
}

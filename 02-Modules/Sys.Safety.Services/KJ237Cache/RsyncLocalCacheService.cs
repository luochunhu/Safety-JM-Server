using Basic.Framework.Web;
using Sys.Safety.Cache.Person;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.ServiceContract.KJ237Cache;
using System.Collections.Generic;

namespace Sys.Safety.Services.KJ237Cache
{
    public class RsyncLocalCacheService : IRsyncLocalCacheService
    {
        public Basic.Framework.Web.BasicResponse Insert(R_SyncLocalCacheInsertRequest syncLocalCacheRequest)
        {
            KJ237CacheHelper.Cache.Insert<R_SyncLocalInfo>(syncLocalCacheRequest.SyncLocal);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchInsert(Sys.Safety.Request.PersonCache.R_SyncLocalCacheBatchInsertRequest syncLocalCacheRequest)
        {
            KJ237CacheHelper.Cache.BatchInsert<R_SyncLocalInfo>(syncLocalCacheRequest.SyncLocals);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse Delete(Sys.Safety.Request.PersonCache.R_SyncLocalCacheDeleteRequest syncLocalCacheRequest)
        {
            KJ237CacheHelper.Cache.Delete<R_SyncLocalInfo>(syncLocalCacheRequest.SyncLocal);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDelete(Sys.Safety.Request.PersonCache.R_SyncLocalCacheBatchDeleteRequest syncLocalCacheRequest)
        {
            KJ237CacheHelper.Cache.BatchDelete<R_SyncLocalInfo>(syncLocalCacheRequest.SyncLocals);
            return new BasicResponse();
        }

        public BasicResponse<R_SyncLocalInfo> GetById(R_SyncLocalCacheGetByIdRequest syncLocalCacheRequest)
        {
            BasicResponse<R_SyncLocalInfo> response = new BasicResponse<R_SyncLocalInfo>();
            var data = KJ237CacheHelper.Cache.FindById<R_SyncLocalInfo>(syncLocalCacheRequest.Id);
            response.Data = data;
            return response;
        }

        public BasicResponse<System.Collections.Generic.List<R_SyncLocalInfo>> GetAll(R_SyncLocalCacheGetAllRequest syncLocalCacheRequest)
        {
            BasicResponse<List<R_SyncLocalInfo>> response = new BasicResponse<List<R_SyncLocalInfo>>();
            var data = KJ237CacheHelper.Cache.FindAll<R_SyncLocalInfo>();
            response.Data = data;
            return response;
        }

        public BasicResponse<System.Collections.Generic.List<R_SyncLocalInfo>> Get(R_SyncLocalCacheGetByConditionRequest syncLocalCacheRequest)
        {
            BasicResponse<List<R_SyncLocalInfo>> response = new BasicResponse<List<R_SyncLocalInfo>>();
            var data = KJ237CacheHelper.Cache.Find(syncLocalCacheRequest.predicate);
            response.Data = data;
            return response;
        }
    }
}

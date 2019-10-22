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
    public class RCallCacheService : IRCallCacheService
    {
        public Basic.Framework.Web.BasicResponse LoadRCallCache(Sys.Safety.Request.PersonCache.RCallCacheLoadRequest RCallCacheRequest)
        {
            RCallCache.RCallCahceInstance.Load();
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddRCallCache(Sys.Safety.Request.PersonCache.RCallCacheAddRequest RCallCacheRequest)
        {
            RCallCache.RCallCahceInstance.AddItem(RCallCacheRequest.RCallInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchAddRCallCache(Sys.Safety.Request.PersonCache.RCallCacheBatchAddRequest RCallCacheRequest)
        {
            RCallCache.RCallCahceInstance.AddItems(RCallCacheRequest.RCallInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateRCallCache(Sys.Safety.Request.PersonCache.RCallCacheUpdateRequest RCallCacheRequest)
        {
            RCallCache.RCallCahceInstance.UpdateItem(RCallCacheRequest.RCallInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateRCallCache(Sys.Safety.Request.PersonCache.RCallCacheBatchUpdateRequest RCallCacheRequest)
        {
            RCallCache.RCallCahceInstance.UpdateItems(RCallCacheRequest.RCallInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteRCallCache(Sys.Safety.Request.PersonCache.RCallCacheDeleteRequest RCallCacheRequest)
        {
            RCallCache.RCallCahceInstance.DeleteItem(RCallCacheRequest.RCallInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDeleteRCallCache(Sys.Safety.Request.PersonCache.RCallCacheBatchDeleteRequest RCallCacheRequest)
        {
            RCallCache.RCallCahceInstance.DeleteItems(RCallCacheRequest.RCallInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_CallInfo>> GetAllRCallCache(Sys.Safety.Request.PersonCache.RCallCacheGetAllRequest RCallCacheRequest)
        {
            var rcallCache = RCallCache.RCallCahceInstance.Query();
            var rcallCacheResponse = new BasicResponse<List<R_CallInfo>>();
            rcallCacheResponse.Data = rcallCache;
            return rcallCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_CallInfo> GetByKeyRCallCache(Sys.Safety.Request.PersonCache.RCallCacheGetByKeyRequest RCallCacheRequest)
        {
            var rcallCache = RCallCache.RCallCahceInstance.Query(o=>o.Id==RCallCacheRequest.Id).FirstOrDefault();
            var rcallCacheResponse = new BasicResponse<R_CallInfo>();
            rcallCacheResponse.Data = rcallCache;
            return rcallCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_CallInfo>> GetRCallCache(Sys.Safety.Request.PersonCache.RCallCacheGetByConditionRequest RCallCacheRequest)
        {
            var rcallCache = RCallCache.RCallCahceInstance.Query(RCallCacheRequest.Predicate);
            var rcallCacheResponse = new BasicResponse<List<R_CallInfo>>();
            rcallCacheResponse.Data = rcallCache;
            return rcallCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<bool> IsExistsRCallCache(Sys.Safety.Request.PersonCache.RCallCacheIsExistsRequest RCallCacheRequest)
        {
            var rcallCache = RCallCache.RCallCahceInstance.Query(call => call.Id == RCallCacheRequest.Id).FirstOrDefault();
            var rcallCacheResponse = new BasicResponse<bool>();
            rcallCacheResponse.Data = rcallCache != null;
            return rcallCacheResponse;
        }
    }
}

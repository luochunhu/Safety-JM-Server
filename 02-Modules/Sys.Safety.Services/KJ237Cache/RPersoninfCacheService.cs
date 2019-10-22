using Basic.Framework.Web;
using Sys.Safety.Cache.Person;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract.KJ237Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.KJ237Cache
{
    public class RPersoninfCacheService : IRPersoninfCacheService
    {
        public Basic.Framework.Web.BasicResponse LoadRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheLoadRequest RPersoninfCacheRequest)
        {
            RPersoninfCache.RPersoninfInstance.Load();
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheAddRequest RPersoninfCacheRequest)
        {
            RPersoninfCache.RPersoninfInstance.AddItem(RPersoninfCacheRequest.RPersoninfInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchAddRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheBatchAddRequest RPersoninfCacheRequest)
        {
            RPersoninfCache.RPersoninfInstance.AddItems(RPersoninfCacheRequest.RPersoninfInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheUpdateRequest RPersoninfCacheRequest)
        {
            RPersoninfCache.RPersoninfInstance.UpdateItem(RPersoninfCacheRequest.RPersoninfInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheBatchUpdateRequest RPersoninfCacheRequest)
        {
            RPersoninfCache.RPersoninfInstance.UpdateItems(RPersoninfCacheRequest.RPersoninfInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheDeleteRequest RPersoninfCacheRequest)
        {
            RPersoninfCache.RPersoninfInstance.DeleteItem(RPersoninfCacheRequest.RPersoninfInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDeleteRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheBatchDeleteRequest RPersoninfCacheRequest)
        {
            RPersoninfCache.RPersoninfInstance.DeleteItems(RPersoninfCacheRequest.RPersoninfInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_PersoninfInfo>> GetAllRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheGetAllRequest RPersoninfCacheRequest)
        {
            var rPersoninfResponse = new BasicResponse<List<R_PersoninfInfo>>();
            var rPersoninfCache = RPersoninfCache.RPersoninfInstance.Query();
            rPersoninfResponse.Data = rPersoninfCache;
            return rPersoninfResponse;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_PersoninfInfo> GetByKeyRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheGetByKeyRequest RPersoninfCacheRequest)
        {
            var rPersoninfResponse = new BasicResponse<R_PersoninfInfo>();
            var rPersoninfCache = RPersoninfCache.RPersoninfInstance.Query(o=>o.Id==RPersoninfCacheRequest.Id).FirstOrDefault();
            rPersoninfResponse.Data = rPersoninfCache;
            return rPersoninfResponse;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_PersoninfInfo>> GetRPersoninfCache(Sys.Safety.Request.PersonCache.RPersoninfCacheGetByConditionRequest RPersoninfCacheRequest)
        {
            var rPersoninfResponse = new BasicResponse<List<R_PersoninfInfo>>();
            var rPersoninfCache = RPersoninfCache.RPersoninfInstance.Query(RPersoninfCacheRequest.Predicate);
            rPersoninfResponse.Data = rPersoninfCache;
            return rPersoninfResponse;
        }
    }
}

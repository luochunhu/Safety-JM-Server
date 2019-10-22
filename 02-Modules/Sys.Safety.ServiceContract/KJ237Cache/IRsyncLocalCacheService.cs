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
    public interface IRsyncLocalCacheService
    {
        BasicResponse Insert(R_SyncLocalCacheInsertRequest syncLocalCacheRequest);

        BasicResponse BatchInsert(R_SyncLocalCacheBatchInsertRequest syncLocalCacheRequest);

        BasicResponse Delete(R_SyncLocalCacheDeleteRequest syncLocalCacheRequest);

        BasicResponse BatchDelete(R_SyncLocalCacheBatchDeleteRequest syncLocalCacheRequest);

        BasicResponse<R_SyncLocalInfo> GetById(R_SyncLocalCacheGetByIdRequest syncLocalCacheRequest);

        BasicResponse<List<R_SyncLocalInfo>> GetAll(R_SyncLocalCacheGetAllRequest syncLocalCacheRequest);

        BasicResponse<List<R_SyncLocalInfo>> Get(R_SyncLocalCacheGetByConditionRequest syncLocalCacheRequest);
    }
}

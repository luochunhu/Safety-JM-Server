using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    public interface IV_DefCacheService
    {
        BasicResponse Load();

        BasicResponse Insert(V_DefCacheInsertRequest vDefCacheRequest);

        BasicResponse Update(V_DefCacheInsertRequest vDefCacheRequest);

        BasicResponse BatchUpdate(V_DefCacheBatchInsertRequest vDefCacheRequest);

        BasicResponse BatchInsert(V_DefCacheBatchInsertRequest vDefCacheRequest);

        BasicResponse Delete(V_DefCacheDeleteRequest vDefCacheRequest);

        BasicResponse BatchDelete(V_DefCacheBatchDeleteRequest vDefCacheRequest);

        BasicResponse<V_DefInfo> GetById(V_DefCacheGetByIdRequest vDefCacheRequest);

        BasicResponse<List<V_DefInfo>> GetAll(V_DefCacheGetAllRequest vDefCacheRequest);

        BasicResponse<List<V_DefInfo>> Get(V_DefCacheGetByConditionRequest vDefCacheRequest);
    }
}

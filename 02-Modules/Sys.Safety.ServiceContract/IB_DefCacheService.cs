using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.B_Def;
using Sys.Safety.Request.R_Call;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.Cache
{
    public interface IB_DefCacheService
    {
        BasicResponse LoadCache(B_DefCacheLoadRequest bDefCacheRequest);
        BasicResponse Insert(B_DefCacheInsertRequest bDefCacheRequest);
        BasicResponse BatchInsert(B_DefCacheBatchInsertRequest bDefCacheRequest);
        BasicResponse Update(B_DefCacheUpdateRequest bDefCacheRequest);
        BasicResponse BatchUpdate(B_DefCacheBatchUpdateRequest bDefCacheRequest);
        BasicResponse Delete(B_DefCacheDeleteRequest bDefCacheRequest);

        BasicResponse BatchDelete(B_DefCacheBatchDeleteRequest bDefCacheRequest);

        BasicResponse<Jc_DefInfo> GetById(B_DefCacheGetByIdRequest bDefCacheRequest);

        BasicResponse<List<Jc_DefInfo>> GetAll(B_DefCacheGetAllRequest bDefCacheRequest);

        BasicResponse<List<Jc_DefInfo>> Get(B_DefCacheGetByConditionRequest bDefCacheRequest);
        BasicResponse UpdateInfo(UpdatePropertiesRequest bDefCacheRequest);
        BasicResponse BatchUpdateInfo(BatchUpdatePropertiesRequest bDefCacheRequest);
    }
}

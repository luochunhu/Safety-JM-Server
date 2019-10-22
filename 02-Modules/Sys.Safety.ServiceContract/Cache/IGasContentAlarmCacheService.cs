using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract.Custom;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract.Cache
{
    public interface IGasContentAlarmCacheService
    {
        BasicResponse AddCache(AddCacheRequest request);

        BasicResponse DeleteCache(DeleteCacheRequest request);

        BasicResponse DeleteCaches(DeleteCachesRequest request);

        BasicResponse UpdateCache(UpdateCacheRequest request);

        BasicResponse<List<GasContentAlarmInfo>> GetAllCache();

        BasicResponse<List<GasContentAlarmInfo>> GetCacheByCondition(GetCacheByConditionRequest request);
    }
}

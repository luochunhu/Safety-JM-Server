using Basic.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Gascontentanalyzeconfig;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.ServiceContract.Cache
{
    public interface IGasContentAnalyzeConfigCacheService
    {
        void LoadCache();

        BasicResponse AddCache(GasContentAnalyzeConfigAddCacheRequest request);

        BasicResponse DeleteCache(GasContentAnalyzeConfigDeleteCacheRequest request);

        BasicResponse DeleteCaches(GasContentAnalyzeConfigDeleteCachesRequest request);

        BasicResponse UpdateCache(GasContentAnalyzeConfigUpdateCacheRequest request);

        BasicResponse UpdateRealTimeValue(UpdateRealTimeValueRequest updateData);

        BasicResponse<List<GascontentanalyzeconfigInfo>> GetAllCache();

        BasicResponse<List<GascontentanalyzeconfigInfo>> GetCacheByCondition(
            GasContentAnalyzeConfigGetCacheByConditionRequest request);
    }
}

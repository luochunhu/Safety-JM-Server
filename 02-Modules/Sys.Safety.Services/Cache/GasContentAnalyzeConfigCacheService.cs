using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Gascontentanalyzeconfig;
using Sys.Safety.Cache.Safety;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Services.Cache
{
    public class GasContentAnalyzeConfigCacheService:IGasContentAnalyzeConfigCacheService
    {
        public void LoadCache()
        {
            GasContentAnalyzeConfigCache.CacheInstance.Load();
        }

        public BasicResponse AddCache(GasContentAnalyzeConfigAddCacheRequest request)
        {
            GasContentAnalyzeConfigCache.CacheInstance.AddItem(request.Info);
            return new BasicResponse();
        }

        public BasicResponse DeleteCache(GasContentAnalyzeConfigDeleteCacheRequest request)
        {
            GasContentAnalyzeConfigCache.CacheInstance.DeleteItem(request.Info);
            return new BasicResponse();
        }

        public BasicResponse DeleteCaches(GasContentAnalyzeConfigDeleteCachesRequest request)
        {
            foreach (var item in request.Infos)
            {
                GasContentAnalyzeConfigCache.CacheInstance.DeleteItem(item);
            }
            return new BasicResponse();
        }

        public BasicResponse UpdateCache(GasContentAnalyzeConfigUpdateCacheRequest request)
        {
            GasContentAnalyzeConfigCache.CacheInstance.UpdateItem(request.Info);
            return new BasicResponse();
        }

        public BasicResponse UpdateRealTimeValue(UpdateRealTimeValueRequest updateData)
        {
            GasContentAnalyzeConfigCache.CacheInstance.UpdateRealTimeValue(updateData);
            return new BasicResponse();
        }

        public BasicResponse<List<GascontentanalyzeconfigInfo>> GetAllCache()
        {
            var info = GasContentAnalyzeConfigCache.CacheInstance.Query();
            var ret = new BasicResponse<List<GascontentanalyzeconfigInfo>>
            {
                Data = info
            };
            return ret;
        }

        public BasicResponse<List<GascontentanalyzeconfigInfo>> GetCacheByCondition(GasContentAnalyzeConfigGetCacheByConditionRequest request)
        {
            var infos = GasContentAnalyzeConfigCache.CacheInstance.Query().Where(request.Condition).ToList();
            var ret = new BasicResponse<List<GascontentanalyzeconfigInfo>>
            {
                Data = infos
            };
            return ret;
        }
    }
}

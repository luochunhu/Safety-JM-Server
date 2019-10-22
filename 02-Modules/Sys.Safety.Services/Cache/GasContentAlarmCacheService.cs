using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract.Custom;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Services.Cache
{
    public class GasContentAlarmCacheService : IGasContentAlarmCacheService
    {
        public BasicResponse AddCache(AddCacheRequest request)
        {
            GasContentAlarmCache.AlarmCacheInstance.AddItem(request.Info);
            return new BasicResponse();
        }

        public BasicResponse DeleteCache(DeleteCacheRequest request)
        {
            GasContentAlarmCache.AlarmCacheInstance.DeleteItem(request.Info);
            return new BasicResponse();
        }

        public BasicResponse DeleteCaches(DeleteCachesRequest request)
        {
            foreach (var item in request.Infos)
            {
                GasContentAlarmCache.AlarmCacheInstance.DeleteItem(item);
            }
            return new BasicResponse();
        }

        public BasicResponse UpdateCache(UpdateCacheRequest request)
        {
            GasContentAlarmCache.AlarmCacheInstance.UpdateItem(request.Info);
            return new BasicResponse();
        }

        public BasicResponse<List<GasContentAlarmInfo>> GetAllCache()
        {
            var info = GasContentAlarmCache.AlarmCacheInstance.Query();
            var ret = new BasicResponse<List<GasContentAlarmInfo>>
            {
                Data = info
            };
            return ret;
        }

        public BasicResponse<List<GasContentAlarmInfo>> GetCacheByCondition(GetCacheByConditionRequest request)
        {
            var infos = GasContentAlarmCache.AlarmCacheInstance.Query().Where(request.Condition).ToList();
            var ret = new BasicResponse<List<GasContentAlarmInfo>>
            {
                Data = infos
            };
            return ret;
        }
    }
}

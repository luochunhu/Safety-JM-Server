using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract.Custom;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Services
{
    public class GasContentService : IGasContentService
    {
        private static IGasContentAlarmCacheService _gasContentAlarmCacheService;

        public GasContentService(IGasContentAlarmCacheService gasContentAlarmCacheService)
        {
            _gasContentAlarmCacheService = gasContentAlarmCacheService;
        }

        public BasicResponse<List<GasContentAlarmInfo>> GetAllGasContentAlarmCache()
        {
            var res = _gasContentAlarmCacheService.GetAllCache();
            var ret = new BasicResponse<List<GasContentAlarmInfo>>
            {
                Data = res.Data
            };
            return ret;
        }
    }
}

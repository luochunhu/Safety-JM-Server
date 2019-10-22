using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Client.Linkage.Handlers
{
    public class BigDataHandle
    {
        private static readonly ILargeDataAnalysisConfigCacheService LargeDataAnalysisConfigCacheService = ServiceFactory.Create<ILargeDataAnalysisConfigCacheService>();

        public static List<JC_LargedataAnalysisConfigInfo> GetAllLargedataAnalysisConfig()
        {
            var req = new LargeDataAnalysisConfigCacheGetAllRequest();
            var res = LargeDataAnalysisConfigCacheService.GetAllLargeDataAnalysisConfigCache(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            return res.Data;
        }
    }
}

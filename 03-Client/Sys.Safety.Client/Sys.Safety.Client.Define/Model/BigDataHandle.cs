using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Client.Define.Model
{
    public class BigDataHandle
    {
        private static ILargeDataAnalysisConfigCacheService _largeDataAnalysisConfigCacheService = ServiceFactory.Create<ILargeDataAnalysisConfigCacheService>();

        public static List<JC_LargedataAnalysisConfigInfo> GetAllLargedataAnalysisConfig()
        {
            var req = new LargeDataAnalysisConfigCacheGetAllRequest();
            var res = _largeDataAnalysisConfigCacheService.GetAllLargeDataAnalysisConfigCache(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            return res.Data;
        }
    }
}

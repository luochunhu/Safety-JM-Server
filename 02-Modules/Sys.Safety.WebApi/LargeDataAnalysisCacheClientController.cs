using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.WebApi
{
    public class LargeDataAnalysisCacheClientController : Basic.Framework.Web.WebApi.BasicApiController, ILargeDataAnalysisCacheClientService
    {

        private ILargeDataAnalysisCacheClientService largeDataAnalysisCacheClientService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();

        [HttpPost]
        [Route("v1/LargeDataAnalysisCache/GetAllDeviceClassCache")]
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache(DeviceClassCacheGetAllRequest deviceClassCacheRequest)
        {
            return largeDataAnalysisCacheClientService.GetAllDeviceClassCache(deviceClassCacheRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisCache/GetAllLargeDataAnalysisConfigCache")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigCache(LargeDataAnalysisCacheClientGetAllRequest getAllRequest)
        {
            return largeDataAnalysisCacheClientService.GetAllLargeDataAnalysisConfigCache(getAllRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisCache/GetLargeDataAnalysisConfigCacheWithEmergencyLinkage")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigCacheWithEmergencyLinkage(LargeDataAnalysisCacheClientGetAllRequest getAllRequest)
        {
            return largeDataAnalysisCacheClientService.GetLargeDataAnalysisConfigCacheWithEmergencyLinkage(getAllRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisCache/GetPointDefineCacheByDevClassID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(PointDefineGetByDevClassIDRequest PointDefineRequest)
        {
            return largeDataAnalysisCacheClientService.GetPointDefineCacheByDevClassID(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisCache/PointDefineCacheByPointIdRequeest")]
        public BasicResponse<Jc_DefInfo> PointDefineCacheByPointIdRequeest(PointDefineCacheByPointIdRequeest pointDefineCacheRequest)
        {
            return largeDataAnalysisCacheClientService.PointDefineCacheByPointIdRequeest(pointDefineCacheRequest);
        }

        [HttpPost]
        [Route("v1/LargeDataAnalysisCache/UpdateLargeDataAnalysisConfigCahce")]
        public BasicResponse UpdateLargeDataAnalysisConfigCahce(LargeDataAnalysisConfigCacheUpdateRequest largeDataAnalysisConfigCache)
        {
            return largeDataAnalysisCacheClientService.UpdateLargeDataAnalysisConfigCahce(largeDataAnalysisConfigCache);
        }
    }
}

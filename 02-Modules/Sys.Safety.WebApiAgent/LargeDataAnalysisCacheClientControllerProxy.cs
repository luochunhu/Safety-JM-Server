using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.WebApiAgent
{
    public class LargeDataAnalysisCacheClientControllerProxy : BaseProxy, ILargeDataAnalysisCacheClientService
    {
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache(DeviceClassCacheGetAllRequest deviceClassCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/GetAllDeviceClassCache?token=" + Token, JSONHelper.ToJSONString(deviceClassCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responsestr);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigCache(LargeDataAnalysisCacheClientGetAllRequest getAllRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/GetAllLargeDataAnalysisConfigCache?token=" + Token, JSONHelper.ToJSONString(getAllRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responsestr);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigCacheWithEmergencyLinkage(LargeDataAnalysisCacheClientGetAllRequest getAllRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/GetLargeDataAnalysisConfigCacheWithEmergencyLinkage?token=" + Token, JSONHelper.ToJSONString(getAllRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responsestr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(PointDefineGetByDevClassIDRequest PointDefineRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/GetPointDefineCacheByDevClassID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responsestr);
        }

        public BasicResponse<Jc_DefInfo> PointDefineCacheByPointIdRequeest(PointDefineCacheByPointIdRequeest pointDefineCacheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/PointDefineCacheByPointIdRequeest?token=" + Token, JSONHelper.ToJSONString(pointDefineCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responsestr);
        }

        public BasicResponse UpdateLargeDataAnalysisConfigCahce(LargeDataAnalysisConfigCacheUpdateRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/UpdateLargeDataAnalysisConfigCahce?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.WebApiAgent
{
    public class LargeDataAnalysisConfigCacheControllerProxy : BaseProxy, ILargeDataAnalysisConfigCacheService
    {
        public Basic.Framework.Web.BasicResponse LoadLargeDataAnalysisConfigCache(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheLoadRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/LoadLargeDataAnalysisConfigCache?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse AddLargeDataAnalysisConfigCache(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheAddRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/AddLargeDataAnalysisConfigCache?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse BacthAddLargeDataAnalysisConfigCache(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheBatchAddRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/BacthAddLargeDataAnalysisConfigCache?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse UpdateLargeDataAnalysisConfigCahce(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheUpdateRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/UpdateLargeDataAnalysisConfigCahce?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateLargeDataAnalysisConfigCache(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheBatchUpdateRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/BatchUpdateLargeDataAnalysisConfigCache?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse DeleteLargeDataAnalysisConfigCache(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheDeleteRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/DeleteLargeDataAnalysisConfigCache?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigCache(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheGetAllRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/GetAllLargeDataAnalysisConfigCache?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_LargedataAnalysisConfigInfo>>>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigCacheByKey(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheGetByKeyRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/GetLargeDataAnalysisConfigCacheByKey?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse<JC_LargedataAnalysisConfigInfo>>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigCache(Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheGetByConditonRequest largeDataAnalysisConfigCache)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/LargeDataAnalysisCache/GetLargeDataAnalysisConfigCache?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigCache));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_LargedataAnalysisConfigInfo>>>(responsestr);
        }
    }
}

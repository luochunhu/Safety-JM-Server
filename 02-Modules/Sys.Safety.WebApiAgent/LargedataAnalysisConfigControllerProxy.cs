using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Request.JC_Largedataanalysisconfig;

namespace Sys.Safety.WebApiAgent
{
    public class LargedataAnalysisConfigControllerProxy : BaseProxy, ILargedataAnalysisConfigService
    {

        public BasicResponse<JC_LargedataAnalysisConfigInfo> AddLargeDataAnalysisConfig(Sys.Safety.Request.JC_Largedataanalysisconfig.LargedataAnalysisConfigAddRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/AddLargeDataAnalysisConfig?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_LargedataAnalysisConfigInfo>>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<DataContract.JC_LargedataAnalysisConfigInfo> UpdateLargeDataAnalysisConfig(Sys.Safety.Request.JC_Largedataanalysisconfig.LargedataAnalysisConfigUpdateRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/UpdateLargeDataAnalysisConfig?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_LargedataAnalysisConfigInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteLargeDataAnalysisConfig(Sys.Safety.Request.JC_Largedataanalysisconfig.LargedataAnalysisConfigDeleteRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/DeleteLargeDataAnalysisConfig?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigList(Sys.Safety.Request.JC_Largedataanalysisconfig.LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigList?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<DataContract.JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigById(Sys.Safety.Request.JC_Largedataanalysisconfig.LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigById?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_LargedataAnalysisConfigInfo>>(responseStr);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetAllLargeDataAnalysisConfigList?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllEnabledLargeDataAnalysisConfigWithDetail(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetAllEnabledLargeDataAnalysisConfigWithDetail?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }


        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListByTempleteId(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigListByTempleteId?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }


        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargedataAnalysisConfigDetailById(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRsequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargedataAnalysisConfigDetailById?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRsequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }


        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListByName(LargedataAnalysisConfigGetListByNameRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigListByName?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithoutAlarmConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigWithoutAlarmConfigList?token=" + Token, JSONHelper.ToJSONString(jc_LargedataAnalysisConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithRegionOutage(LargedataAnalysisConfigGetListWithRegionOutageRequest largedataAnalysisConfigGetListWithRegionOutageRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigWithRegionOutage?token=" + Token, JSONHelper.ToJSONString(largedataAnalysisConfigGetListWithRegionOutageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithoutRegionOutage(LargedataAnalysisConfigGetListRequest largedataAnalysisConfigGetListRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigWithoutRegionOutage?token=" + Token, JSONHelper.ToJSONString(largedataAnalysisConfigGetListRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListForCurve(LargeDataAnalysisConfigListForCurveRequest largeDataAnalysisConfigListForCurveRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigListForCurve?token=" + Token, JSONHelper.ToJSONString(largeDataAnalysisConfigListForCurveRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }


        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithRegionOutagePage(LargedataAnalysisConfigGetListWithRegionOutageRequest largedataAnalysisConfigGetListWithRegionOutageRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigWithRegionOutagePage?token=" + Token, JSONHelper.ToJSONString(largedataAnalysisConfigGetListWithRegionOutageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_LargedataAnalysisConfigInfo>>>(responseStr);
        }
    }
}

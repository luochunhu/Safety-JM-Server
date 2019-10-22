using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.RegionOutageConfig;
using Basic.Framework.Common;
using Basic.Framework.Web.WebApi.Proxy;

namespace Sys.Safety.WebApiAgent
{
    public class RegionOutageConfigControllerProxy : BaseProxy, IRegionOutageConfigService
    {

        public BasicResponse<JC_RegionOutageConfigInfo> AddJC_Regionoutageconfig(RegionOutageConfigAddRequest jC_Regionoutageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/AddJC_Regionoutageconfig?token=" + Token, JSONHelper.ToJSONString(jC_Regionoutageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_RegionOutageConfigInfo>>(responseStr);
        }

        public BasicResponse<List<JC_RegionOutageConfigInfo>> AddJC_RegionOutageConfigList(RegionOutageConfigListAddRequest jC_RegionOutageConfigListAddRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/AddJC_RegionOutageConfigList?token=" + Token, JSONHelper.ToJSONString(jC_RegionOutageConfigListAddRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_RegionOutageConfigInfo>>>(responseStr);
        }

        public BasicResponse DeleteJC_Regionoutageconfig(RegionoutageconfigDeleteRequest jC_Regionoutageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/DeleteJC_Regionoutageconfig?token=" + Token, JSONHelper.ToJSONString(jC_Regionoutageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse DeleteJC_RegionoutageconfigByAnalysisModelId(RegionoutageconfigDeleteByAnalysisModelIdRequest jC_Regionoutageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/DeleteJC_RegionoutageconfigByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(jC_Regionoutageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<JC_RegionOutageConfigInfo> GetJC_RegionoutageconfigById(RegionOutageConfigGetRequest jC_Regionoutageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/GetJC_RegionoutageconfigById?token=" + Token, JSONHelper.ToJSONString(jC_Regionoutageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_RegionOutageConfigInfo>>(responseStr);
        }

        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetJC_RegionoutageconfigList(RegionOutageConfigGetListRequest jC_Regionoutageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/GetJC_RegionoutageconfigList?token=" + Token, JSONHelper.ToJSONString(jC_Regionoutageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_RegionOutageConfigInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetRegionOutageConfigAllList(GetAllRegionOutageConfigRequest getAllRegionOutageConfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/GetRegionOutageConfigAllList?token=" + Token, JSONHelper.ToJSONString(getAllRegionOutageConfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_RegionOutageConfigInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetRegionOutageConfigListByAnalysisModelId(RegionOutageConfigGetListRequest regionOutageConfigGetListRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/GetRegionOutageConfigListByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(regionOutageConfigGetListRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_RegionOutageConfigInfo>>>(responseStr);
        }

        public BasicResponse<bool> HasRegionOutageForAnalysisModel(GetByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/HasRegionOutageForAnalysisModel?token=" + Token, JSONHelper.ToJSONString(getByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public BasicResponse NoReleaseControlForAnalysysModelAndPoint(ReleaseControlCheckRequest releaseControlCheckRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/NoReleaseControlForAnalysysModelAndPoint?token=" + Token, JSONHelper.ToJSONString(releaseControlCheckRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<JC_RegionOutageConfigInfo> UpdateJC_Regionoutageconfig(RegionOutageConfigUpdateRequest jC_Regionoutageconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/RegionOutageConfig/UpdateJC_Regionoutageconfig?token=" + Token, JSONHelper.ToJSONString(jC_Regionoutageconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_RegionOutageConfigInfo>>(responseStr);
        }

    }
}

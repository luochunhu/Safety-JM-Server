using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AnalysisTemplateConfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class AnalysisTemplateConfigControllerProxy : BaseProxy, IAnalysisTemplateConfigService
    {

        public BasicResponse<JC_AnalysisTemplateConfigInfo> AddJC_Analysistemplateconfig(AnalysisTemplateConfigAddRequest jC_Analysistemplateconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateConfig/AddJC_Analysistemplateconfig?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplateconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalysisTemplateConfigInfo>>(responseStr);
        }

        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> AddAnalysistemplateconfigList(AnalysisTemplateConfigListAddRequest jC_Analysistemplateconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateConfig/AddAnalysistemplateconfigList?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplateconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AnalysisTemplateConfigInfo>>>(responseStr);
        }

        public BasicResponse<JC_AnalysisTemplateConfigInfo> UpdateJC_Analysistemplateconfig(AnalysisTemplateConfigUpdateRequest jC_Analysistemplateconfigrequest)
        {

            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateConfig/UpdateJC_Analysistemplateconfig?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplateconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalysisTemplateConfigInfo>>(responseStr);
        }

        public BasicResponse DeleteJC_Analysistemplateconfig(AnalysisTemplateConfigDeleteRequest jC_Analysistemplateconfigrequest)
        {

            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateConfig/DeleteJC_Analysistemplateconfig?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplateconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> GetJC_AnalysistemplateconfigList(AnalysisTemplateConfigGetListRequest jC_Analysistemplateconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateConfig/GetJC_AnalysistemplateconfigList?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplateconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AnalysisTemplateConfigInfo>>>(responseStr);
        }

        public BasicResponse<JC_AnalysisTemplateConfigInfo> GetJC_AnalysistemplateconfigById(AnalysisTemplateConfigGetRequest jC_Analysistemplateconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateConfig/GetJC_AnalysistemplateconfigById?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplateconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalysisTemplateConfigInfo>>(responseStr);
        }

        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> GetJC_AnalysistemplateconfigByTempleteId(AnalysisTemplateConfigGetByTempleteIdRequest jC_AnalysisTemplateConfigGetByTempleteIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateConfig/GetJC_AnalysistemplateconfigByTempleteId?token=" + Token, JSONHelper.ToJSONString(jC_AnalysisTemplateConfigGetByTempleteIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AnalysisTemplateConfigInfo>>>(responseStr);
        }


        public BasicResponse DeleteJC_AnalysistemplateconfigByTempleteId(AnalysisTemplateConfigGetByTempleteIdRequest jC_AnalysisTemplateConfigGetByTempleteIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateConfig/DeleteJC_AnalysistemplateconfigByTempleteId?token=" + Token, JSONHelper.ToJSONString(jC_AnalysisTemplateConfigGetByTempleteIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}

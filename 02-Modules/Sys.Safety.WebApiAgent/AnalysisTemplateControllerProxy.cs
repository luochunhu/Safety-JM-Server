using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AnalysisTemplate;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class AnalysisTemplateControllerProxy : BaseProxy, IAnalysisTemplateService
    {


        public BasicResponse<JC_AnalysisTemplateInfo> AddJC_Analysistemplate(AnalysisTemplateAddRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplate/AddJC_Analysistemplate?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalysisTemplateInfo>>(responseStr);
        }

        public BasicResponse<JC_AnalysisTemplateInfo> UpdateJC_Analysistemplate(AnalysisTemplateUpdateRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplate/UpdateJC_Analysistemplate?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalysisTemplateInfo>>(responseStr);
        }

        public BasicResponse DeleteJC_Analysistemplate(AnalysisTemplateDeleteRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplate/DeleteJC_Analysistemplate?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetJC_AnalysistemplateList(AnalysisTemplateGetListRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplate/GetJC_AnalysistemplateList?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AnalysisTemplateInfo>>>(responseStr);
        }

        public BasicResponse<JC_AnalysisTemplateInfo> GetJC_AnalysistemplateById(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplate/GetJC_AnalysistemplateById?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalysisTemplateInfo>>(responseStr);
        }


        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetAnalysisTemplateListDetail(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplate/GetAnalysisTemplateListDetail?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AnalysisTemplateInfo>>>(responseStr);
        }


        public BasicResponse<JC_AnalysisTemplateInfo> GetJC_AnalysistemplateByTempleteId(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplate/GetJC_AnalysistemplateByTempleteId?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalysisTemplateInfo>>(responseStr);
        }


        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetJC_AnalysistemplateListByName(AnalysisTemplateGetListByNameRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplate/GetJC_AnalysistemplateListByName?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AnalysisTemplateInfo>>>(responseStr);
        }
    }
}

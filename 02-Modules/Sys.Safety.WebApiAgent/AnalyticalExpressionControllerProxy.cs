using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Analyticalexpression;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class AnalyticalExpressionControllerProxy : BaseProxy, IAnalyticalExpressionService
    {
        

        public Basic.Framework.Web.BasicResponse<DataContract.JC_AnalyticalExpressionInfo> AddJC_Analyticalexpression(Sys.Safety.Request.JC_Analyticalexpression.AnalyticalExpressionAddRequest jC_Analyticalexpressionrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalyticalExpression/AddJC_Analyticalexpression?token=" + Token, JSONHelper.ToJSONString(jC_Analyticalexpressionrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalyticalExpressionInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<JC_AnalyticalExpressionInfo>> AddAnalyticalExpressionList(AnalyticalExpressionListAddRequest jC_AnalyticalExpressionrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalyticalExpression/AddAnalyticalExpressionList?token=" + Token, JSONHelper.ToJSONString(jC_AnalyticalExpressionrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AnalyticalExpressionInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_AnalyticalExpressionInfo> UpdateJC_Analyticalexpression(Sys.Safety.Request.JC_Analyticalexpression.AnalyticalExpressionUpdateRequest jC_Analyticalexpressionrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalyticalExpression/UpdateJC_Analyticalexpression?token=" + Token, JSONHelper.ToJSONString(jC_Analyticalexpressionrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalyticalExpressionInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteJC_Analyticalexpression(Sys.Safety.Request.JC_Analyticalexpression.AnalyticalExpressionDeleteRequest jC_Analyticalexpressionrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalyticalExpression/DeleteJC_Analyticalexpression?token=" + Token, JSONHelper.ToJSONString(jC_Analyticalexpressionrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_AnalyticalExpressionInfo>> GetJC_AnalyticalexpressionList(Sys.Safety.Request.JC_Analyticalexpression.AnalyticalExpressionGetListRequest jC_Analyticalexpressionrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalyticalExpression/GetJC_AnalyticalexpressionList?token=" + Token, JSONHelper.ToJSONString(jC_Analyticalexpressionrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_AnalyticalExpressionInfo>>>(responseStr);
       
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_AnalyticalExpressionInfo> GetJC_AnalyticalexpressionById(Sys.Safety.Request.JC_Analyticalexpression.AnalyticalExpressionGetRequest jC_Analyticalexpressionrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalyticalExpression/GetJC_AnalyticalexpressionById?token=" + Token, JSONHelper.ToJSONString(jC_Analyticalexpressionrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AnalyticalExpressionInfo>>(responseStr);

        }


        public BasicResponse<List<JC_AnalyticalExpressionInfo>> GetAnalysisTemplateListByTempleteId(AnalyticalExpressionGetListRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalyticalExpression/GetAnalysisTemplateListByTempleteId?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AnalyticalExpressionInfo>>>(responseStr);
        }


        public BasicResponse DeleteJC_AnalyticalexpressionByTempleteId(AnalyticalExpressionGetListRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalyticalExpression/DeleteJC_AnalyticalexpressionByTempleteId?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

    }
}

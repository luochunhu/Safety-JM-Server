using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Expressionconfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class ExpressionConfigControllerProxy : BaseProxy, IExpressionConfigService
    {


        public Basic.Framework.Web.BasicResponse<DataContract.JC_ExpressionConfigInfo> AddJC_Expressionconfig(Sys.Safety.Request.JC_Expressionconfig.ExpressionConfigAddRequest jC_Expressionconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/AddJC_Expressionconfig?token=" + Token, JSONHelper.ToJSONString(jC_Expressionconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_ExpressionConfigInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_ExpressionConfigInfo>> AddExpressionConfigList(Sys.Safety.Request.JC_Expressionconfig.ExpressionConfigListAddRequest jC_ExpressionConfigListAddRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/AddExpressionConfigList?token=" + Token, JSONHelper.ToJSONString(jC_ExpressionConfigListAddRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_ExpressionConfigInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_ExpressionConfigInfo> UpdateJC_Expressionconfig(Sys.Safety.Request.JC_Expressionconfig.ExpressionConfigUpdateRequest jC_Expressionconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/UpdateJC_Expressionconfig?token=" + Token, JSONHelper.ToJSONString(jC_Expressionconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_ExpressionConfigInfo>>(responseStr);
        }
        public Basic.Framework.Web.BasicResponse DeleteJC_Expressionconfig(Sys.Safety.Request.JC_Expressionconfig.ExpressionconfigDeleteRequest jC_Expressionconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/DeleteJC_Expressionconfig?token=" + Token, JSONHelper.ToJSONString(jC_Expressionconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_ExpressionConfigInfo>> GetJC_ExpressionconfigList(Sys.Safety.Request.JC_Expressionconfig.ExpressionConfigGetListRequest jC_Expressionconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/GetJC_ExpressionconfigList?token=" + Token, JSONHelper.ToJSONString(jC_Expressionconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_ExpressionConfigInfo>>>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<DataContract.JC_ExpressionConfigInfo> GetJC_ExpressionconfigById(Sys.Safety.Request.JC_Expressionconfig.ExpressionConfigGetRequest jC_Expressionconfigrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/GetJC_ExpressionconfigById?token=" + Token, JSONHelper.ToJSONString(jC_Expressionconfigrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_ExpressionConfigInfo>>(responseStr);
        }


        public BasicResponse<List<JC_ExpressionConfigInfo>> GetJC_ExpressionconfigListByExpressionId(Sys.Safety.Request.JC_Expressionconfig.ExpressionConfigGetByExpressionIdRequest expressionId)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/GetJC_ExpressionconfigListByExpressionId?token=" + Token, JSONHelper.ToJSONString(expressionId));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_ExpressionConfigInfo>>>(responseStr);
        }
        public BasicResponse<List<JC_ExpressionConfigInfo>> GetExpressionConfigListByTempleteId(Sys.Safety.Request.JC_Expressionconfig.ExpressionConfigGetListRequest expressionId)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/GetExpressionConfigListByTempleteId?token=" + Token, JSONHelper.ToJSONString(expressionId));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_ExpressionConfigInfo>>>(responseStr);
        }


        public BasicResponse DeleteJC_ExpressionconfigByTempleteId(ExpressionConfigGetListRequest jC_Analysistemplaterequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ExpressionConfig/DeleteJC_ExpressionconfigByTempleteId?token=" + Token, JSONHelper.ToJSONString(jC_Analysistemplaterequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}

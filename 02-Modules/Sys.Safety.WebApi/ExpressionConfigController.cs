using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Analyticalexpression;
using Sys.Safety.Request.JC_Expressionconfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class ExpressionConfigController: Basic.Framework.Web.WebApi.BasicApiController, IExpressionConfigService
   {
        static ExpressionConfigController()
        {

        }
        IExpressionConfigService _expressionConfigService = ServiceFactory.Create<IExpressionConfigService>();
      
      
        [HttpPost]
        [Route("v1/ExpressionConfig/AddJC_Expressionconfig")]
        public BasicResponse<JC_ExpressionConfigInfo> AddJC_Expressionconfig(ExpressionConfigAddRequest jC_Expressionconfigrequest)
        {
            return _expressionConfigService.AddJC_Expressionconfig(jC_Expressionconfigrequest);
        }
        [HttpPost]
        [Route("v1/ExpressionConfig/AddExpressionConfigList")]
        public BasicResponse<List<JC_ExpressionConfigInfo>> AddExpressionConfigList(ExpressionConfigListAddRequest jC_ExpressionConfigListAddRequest)
        {
            return _expressionConfigService.AddExpressionConfigList(jC_ExpressionConfigListAddRequest);
        }
        [HttpPost]
        [Route("v1/ExpressionConfig/UpdateJC_Expressionconfig")]
        public BasicResponse<JC_ExpressionConfigInfo> UpdateJC_Expressionconfig(ExpressionConfigUpdateRequest jC_Expressionconfigrequest)
        {
            return _expressionConfigService.UpdateJC_Expressionconfig(jC_Expressionconfigrequest);
        }
        [HttpPost]
        [Route("v1/ExpressionConfig/DeleteJC_Expressionconfig")]
        public BasicResponse DeleteJC_Expressionconfig(ExpressionconfigDeleteRequest jC_Expressionconfigrequest)
        {
            return _expressionConfigService.DeleteJC_Expressionconfig(jC_Expressionconfigrequest);
        }
        [HttpPost]
        [Route("v1/ExpressionConfig/GetJC_ExpressionconfigList")]
        public BasicResponse<List<JC_ExpressionConfigInfo>> GetJC_ExpressionconfigList(ExpressionConfigGetListRequest jC_Expressionconfigrequest)
        {
            return _expressionConfigService.GetJC_ExpressionconfigList(jC_Expressionconfigrequest);
        }
        [HttpPost]
        [Route("v1/ExpressionConfig/GetJC_ExpressionconfigById")]
        public BasicResponse<JC_ExpressionConfigInfo> GetJC_ExpressionconfigById(ExpressionConfigGetRequest jC_Expressionconfigrequest)
        {
            return _expressionConfigService.GetJC_ExpressionconfigById(jC_Expressionconfigrequest);
        }

        [HttpPost]
        [Route("v1/ExpressionConfig/GetJC_ExpressionconfigListByExpressionId")]
        public BasicResponse<List<JC_ExpressionConfigInfo>> GetJC_ExpressionconfigListByExpressionId(ExpressionConfigGetByExpressionIdRequest expressionId)
        {
            return _expressionConfigService.GetJC_ExpressionconfigListByExpressionId(expressionId);
        }

        [HttpPost]
        [Route("v1/ExpressionConfig/GetExpressionConfigListByTempleteId")]
        public BasicResponse<List<JC_ExpressionConfigInfo>> GetExpressionConfigListByTempleteId(ExpressionConfigGetListRequest jC_Analysistemplaterequest)
        {
            return _expressionConfigService.GetExpressionConfigListByTempleteId(jC_Analysistemplaterequest);
        }

        [HttpPost]
        [Route("v1/ExpressionConfig/DeleteJC_ExpressionconfigByTempleteId")]
        public BasicResponse DeleteJC_ExpressionconfigByTempleteId(ExpressionConfigGetListRequest jC_Analysistemplaterequest)
        {
            return _expressionConfigService.DeleteJC_ExpressionconfigByTempleteId(jC_Analysistemplaterequest);
        }
   }
}

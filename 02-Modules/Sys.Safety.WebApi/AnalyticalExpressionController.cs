using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Analyticalexpression;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AnalyticalExpressionController: Basic.Framework.Web.WebApi.BasicApiController, IAnalyticalExpressionService
   {
        public AnalyticalExpressionController() {
            _analysisExpressionService = ServiceFactory.Create<IAnalyticalExpressionService>();
        }
        static AnalyticalExpressionController()
        {

        }
        IAnalyticalExpressionService _analysisExpressionService = null;
      
      
        [HttpPost]
        [Route("v1/AnalyticalExpression/AddJC_Analyticalexpression")]
        public BasicResponse<JC_AnalyticalExpressionInfo> AddJC_Analyticalexpression(AnalyticalExpressionAddRequest jC_Analyticalexpressionrequest)
        {
            return _analysisExpressionService.AddJC_Analyticalexpression(jC_Analyticalexpressionrequest);
        }
        [HttpPost]
        [Route("v1/AnalyticalExpression/AddAnalyticalExpressionList")]
        public BasicResponse<List<JC_AnalyticalExpressionInfo>> AddAnalyticalExpressionList(AnalyticalExpressionListAddRequest jC_AnalyticalExpressionrequest)
        {
            return _analysisExpressionService.AddAnalyticalExpressionList(jC_AnalyticalExpressionrequest);
        }
        [HttpPost]
        [Route("v1/AnalyticalExpression/UpdateJC_Analyticalexpression")]
        public BasicResponse<JC_AnalyticalExpressionInfo> UpdateJC_Analyticalexpression(AnalyticalExpressionUpdateRequest jC_Analyticalexpressionrequest)
        {
            return _analysisExpressionService.UpdateJC_Analyticalexpression(jC_Analyticalexpressionrequest);
        }
        [HttpPost]
        [Route("v1/AnalyticalExpression/DeleteJC_Analyticalexpression")]
        public BasicResponse DeleteJC_Analyticalexpression(AnalyticalExpressionDeleteRequest jC_Analyticalexpressionrequest)
        {
            return _analysisExpressionService.DeleteJC_Analyticalexpression(jC_Analyticalexpressionrequest);
        }
        [HttpPost]
        [Route("v1/AnalyticalExpression/GetJC_AnalyticalexpressionList")]
        public BasicResponse<List<JC_AnalyticalExpressionInfo>> GetJC_AnalyticalexpressionList(AnalyticalExpressionGetListRequest jC_Analyticalexpressionrequest)
        {
            return _analysisExpressionService.GetJC_AnalyticalexpressionList(jC_Analyticalexpressionrequest);
        }
        [HttpPost]
        [Route("v1/AnalyticalExpression/GetJC_AnalyticalexpressionById")]
        public BasicResponse<JC_AnalyticalExpressionInfo> GetJC_AnalyticalexpressionById(AnalyticalExpressionGetRequest jC_Analyticalexpressionrequest)
        {
            return _analysisExpressionService.GetJC_AnalyticalexpressionById(jC_Analyticalexpressionrequest);
        }

        [HttpPost]
        [Route("v1/AnalyticalExpression/GetAnalysisTemplateListByTempleteId")]
        public BasicResponse<List<JC_AnalyticalExpressionInfo>> GetAnalysisTemplateListByTempleteId(AnalyticalExpressionGetListRequest jC_Analysistemplaterequest)
        {
            return _analysisExpressionService.GetAnalysisTemplateListByTempleteId(jC_Analysistemplaterequest);
        }
        [HttpPost]
        [Route("v1/AnalyticalExpression/DeleteJC_AnalyticalexpressionByTempleteId")]

        public BasicResponse DeleteJC_AnalyticalexpressionByTempleteId(AnalyticalExpressionGetListRequest jC_Analysistemplaterequest)
        {
            return _analysisExpressionService.DeleteJC_AnalyticalexpressionByTempleteId(jC_Analysistemplaterequest);
        }
   }
}

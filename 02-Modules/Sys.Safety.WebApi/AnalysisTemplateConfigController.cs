using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AnalysisTemplateConfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AnalysisTemplateConfigController : Basic.Framework.Web.WebApi.BasicApiController, IAnalysisTemplateConfigService
    {
        static AnalysisTemplateConfigController()
        {

        }
        IAnalysisTemplateConfigService _AnalysisTemplateConfigService = ServiceFactory.Create<IAnalysisTemplateConfigService>();

        [HttpPost]
        [Route("v1/AnalysisTemplateConfig/AddJC_Analysistemplateconfig")]
        public BasicResponse<JC_AnalysisTemplateConfigInfo> AddJC_Analysistemplateconfig(AnalysisTemplateConfigAddRequest jC_Analysistemplateconfigrequest)
        {
            return _AnalysisTemplateConfigService.AddJC_Analysistemplateconfig(jC_Analysistemplateconfigrequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplateConfig/AddAnalysistemplateconfigList")]
        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> AddAnalysistemplateconfigList(AnalysisTemplateConfigListAddRequest jC_Analysistemplateconfigrequest)
        {
            return _AnalysisTemplateConfigService.AddAnalysistemplateconfigList(jC_Analysistemplateconfigrequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplateConfig/UpdateJC_Analysistemplateconfig")]
        public BasicResponse<JC_AnalysisTemplateConfigInfo> UpdateJC_Analysistemplateconfig(AnalysisTemplateConfigUpdateRequest jC_Analysistemplateconfigrequest)
        {
            return _AnalysisTemplateConfigService.UpdateJC_Analysistemplateconfig(jC_Analysistemplateconfigrequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplateConfig/DeleteJC_Analysistemplateconfig")]
        public BasicResponse DeleteJC_Analysistemplateconfig(AnalysisTemplateConfigDeleteRequest jC_Analysistemplateconfigrequest)
        {
            return _AnalysisTemplateConfigService.DeleteJC_Analysistemplateconfig(jC_Analysistemplateconfigrequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplateConfig/GetJC_AnalysistemplateconfigList")]
        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> GetJC_AnalysistemplateconfigList(AnalysisTemplateConfigGetListRequest jC_Analysistemplateconfigrequest)
        {
            return _AnalysisTemplateConfigService.GetJC_AnalysistemplateconfigList(jC_Analysistemplateconfigrequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplateConfig/GetJC_AnalysistemplateconfigById")]
        public BasicResponse<JC_AnalysisTemplateConfigInfo> GetJC_AnalysistemplateconfigById(AnalysisTemplateConfigGetRequest jC_Analysistemplateconfigrequest)
        {
            return _AnalysisTemplateConfigService.GetJC_AnalysistemplateconfigById(jC_Analysistemplateconfigrequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplateConfig/GetJC_AnalysistemplateconfigByTempleteId")]
        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> GetJC_AnalysistemplateconfigByTempleteId(AnalysisTemplateConfigGetByTempleteIdRequest jC_AnalysisTemplateConfigGetByTempleteIdRequest)
        {
            return _AnalysisTemplateConfigService.GetJC_AnalysistemplateconfigByTempleteId(jC_AnalysisTemplateConfigGetByTempleteIdRequest);
        }


        [HttpPost]
        [Route("v1/AnalysisTemplateConfig/DeleteJC_AnalysistemplateconfigByTempleteId")]

        public BasicResponse DeleteJC_AnalysistemplateconfigByTempleteId(AnalysisTemplateConfigGetByTempleteIdRequest jC_AnalysisTemplateConfigGetByTempleteIdRequest)
        {
            return _AnalysisTemplateConfigService.DeleteJC_AnalysistemplateconfigByTempleteId(jC_AnalysisTemplateConfigGetByTempleteIdRequest);
        }
    }
}

using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AnalysisTemplate;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AnalysisTemplateController : Basic.Framework.Web.WebApi.BasicApiController, IAnalysisTemplateService
   {
        static AnalysisTemplateController()
        {

        }
        IAnalysisTemplateService _analysisTemplateService = ServiceFactory.Create<IAnalysisTemplateService>();
      
        [HttpPost]
        [Route("v1/AnalysisTemplate/AddJC_Analysistemplate")]
        public BasicResponse<JC_AnalysisTemplateInfo> AddJC_Analysistemplate(AnalysisTemplateAddRequest jC_Analysistemplaterequest)
        {
            return _analysisTemplateService.AddJC_Analysistemplate(jC_Analysistemplaterequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplate/UpdateJC_Analysistemplate")]
        public BasicResponse<JC_AnalysisTemplateInfo> UpdateJC_Analysistemplate(AnalysisTemplateUpdateRequest jC_Analysistemplaterequest)
        {
            return _analysisTemplateService.UpdateJC_Analysistemplate(jC_Analysistemplaterequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplate/DeleteJC_Analysistemplate")]
        public BasicResponse DeleteJC_Analysistemplate(AnalysisTemplateDeleteRequest jC_Analysistemplaterequest)
        {
            return _analysisTemplateService.DeleteJC_Analysistemplate(jC_Analysistemplaterequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplate/GetJC_AnalysistemplateList")]
        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetJC_AnalysistemplateList(AnalysisTemplateGetListRequest jC_Analysistemplaterequest)
        {
            return _analysisTemplateService.GetJC_AnalysistemplateList(jC_Analysistemplaterequest);
        }
        [HttpPost]
        [Route("v1/AnalysisTemplate/GetJC_AnalysistemplateById")]
        public BasicResponse<JC_AnalysisTemplateInfo> GetJC_AnalysistemplateById(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            return _analysisTemplateService.GetJC_AnalysistemplateById(jC_Analysistemplaterequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplate/GetAnalysisTemplateListDetail")]
        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetAnalysisTemplateListDetail(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            return _analysisTemplateService.GetAnalysisTemplateListDetail(jC_Analysistemplaterequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplate/GetJC_AnalysistemplateByTempleteId")]
        public BasicResponse<JC_AnalysisTemplateInfo> GetJC_AnalysistemplateByTempleteId(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            return _analysisTemplateService.GetJC_AnalysistemplateByTempleteId(jC_Analysistemplaterequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplate/GetJC_AnalysistemplateListByName")]

        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetJC_AnalysistemplateListByName(AnalysisTemplateGetListByNameRequest jC_Analysistemplaterequest)
        {
            return _analysisTemplateService.GetJC_AnalysistemplateListByName(jC_Analysistemplaterequest);
        }
   }
}

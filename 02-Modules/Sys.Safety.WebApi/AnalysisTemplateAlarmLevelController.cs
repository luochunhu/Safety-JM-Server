using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AnalysisTemplateAlarmLevelController : Basic.Framework.Web.WebApi.BasicApiController, IJc_AnalysistemplatealarmlevelService
    {
        IJc_AnalysistemplatealarmlevelService _AnalysistemplatealarmlevelService = ServiceFactory.Create<IJc_AnalysistemplatealarmlevelService>();

        [HttpPost]
        [Route("v1/AnalysisTemplateAlarmLevel/AddAnalysistemplatealarmlevel")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_AnalysistemplatealarmlevelInfo> AddAnalysistemplatealarmlevel(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelAddRequest analysistemplatealarmlevelRequest)
        {
            return _AnalysistemplatealarmlevelService.AddAnalysistemplatealarmlevel(analysistemplatealarmlevelRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplateAlarmLevel/UpdateAnalysistemplatealarmlevel")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_AnalysistemplatealarmlevelInfo> UpdateAnalysistemplatealarmlevel(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelUpdateRequest analysistemplatealarmlevelRequest)
        {
            return _AnalysistemplatealarmlevelService.UpdateAnalysistemplatealarmlevel(analysistemplatealarmlevelRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplateAlarmLevel/DeleteAnalysistemplatealarmlevel")]
        public Basic.Framework.Web.BasicResponse DeleteAnalysistemplatealarmlevel(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelDeleteRequest analysistemplatealarmlevelRequest)
        {
            return _AnalysistemplatealarmlevelService.DeleteAnalysistemplatealarmlevel(analysistemplatealarmlevelRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplateAlarmLevel/GetAnalysistemplatealarmlevelList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_AnalysistemplatealarmlevelInfo>> GetAnalysistemplatealarmlevelList(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelGetListRequest analysistemplatealarmlevelRequest)
        {
            return _AnalysistemplatealarmlevelService.GetAnalysistemplatealarmlevelList(analysistemplatealarmlevelRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplateAlarmLevel/GetAnalysistemplatealarmlevelById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_AnalysistemplatealarmlevelInfo> GetAnalysistemplatealarmlevelById(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelGetRequest analysistemplatealarmlevelRequest)
        {
            return _AnalysistemplatealarmlevelService.GetAnalysistemplatealarmlevelById(analysistemplatealarmlevelRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplateAlarmLevel/GetAnalysistemplatealarmlevelByAnalysistemplateId")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_AnalysistemplatealarmlevelInfo> GetAnalysistemplatealarmlevelByAnalysistemplateId(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest analysistemplatealarmlevelRequest)
        {
            return _AnalysistemplatealarmlevelService.GetAnalysistemplatealarmlevelByAnalysistemplateId(analysistemplatealarmlevelRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisTemplateAlarmLevel/GetAllAnalysistemplateAlarmLevelInfos")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_AnalysistemplatealarmlevelInfo>> GetAllAnalysistemplateAlarmLevelInfos()
        {
            return _AnalysistemplatealarmlevelService.GetAllAnalysistemplateAlarmLevelInfos();
        }
    }
}

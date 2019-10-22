using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class R_PersoninfController : BasicApiController, IR_PersoninfService
    {
        IR_PersoninfService _presonInfoService = ServiceFactory.Create<IR_PersoninfService>();

        [HttpPost]
        [Route("v1/R_Personinf/AddPersoninf")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PersoninfInfo> AddPersoninf(Sys.Safety.Request.R_Personinf.R_PersoninfAddRequest personinfRequest)
        {
            return _presonInfoService.AddPersoninf(personinfRequest);
        }

        [HttpPost]
        [Route("v1/R_Personinf/UpdatePersoninf")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PersoninfInfo> UpdatePersoninf(Sys.Safety.Request.R_Personinf.R_PersoninfUpdateRequest personinfRequest)
        {
            return _presonInfoService.UpdatePersoninf(personinfRequest);
        }

        [HttpPost]
        [Route("v1/R_Personinf/DeletePersoninf")]
        public Basic.Framework.Web.BasicResponse DeletePersoninf(Sys.Safety.Request.R_Personinf.R_PersoninfDeleteRequest personinfRequest)
        {
            return _presonInfoService.DeletePersoninf(personinfRequest);
        }

        [HttpPost]
        [Route("v1/R_Personinf/GetPersoninfList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PersoninfInfo>> GetPersoninfList(Sys.Safety.Request.R_Personinf.R_PersoninfGetListRequest personinfRequest)
        {
            return _presonInfoService.GetPersoninfList(personinfRequest);
        }

        [HttpPost]
        [Route("v1/R_Personinf/GetPersoninfById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PersoninfInfo> GetPersoninfById(Sys.Safety.Request.R_Personinf.R_PersoninfGetRequest personinfRequest)
        {
            return _presonInfoService.GetPersoninfById(personinfRequest);
        }

        [HttpPost]
        [Route("v1/R_Personinf/GetAllPersonInfo")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PersoninfInfo>> GetAllPersonInfo(Basic.Framework.Web.BasicRequest personinfRequest)
        {
            return _presonInfoService.GetAllPersonInfo(personinfRequest);
        }

        [HttpPost]
        [Route("v1/R_Personinf/GetAllPersonInfoCache")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PersoninfInfo>> GetAllPersonInfoCache(Basic.Framework.Web.BasicRequest personinfRequest)
        {
            return _presonInfoService.GetAllPersonInfoCache(personinfRequest);
        }
        [HttpPost]
        [Route("v1/R_Personinf/GetAllDefinedPersonInfoCache")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PersoninfInfo>> GetAllDefinedPersonInfoCache(Basic.Framework.Web.BasicRequest personinfRequest)
        {
            return _presonInfoService.GetAllDefinedPersonInfoCache(personinfRequest);
        }
        [HttpPost]
        [Route("v1/R_Personinf/GetPersoninfCache")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PersoninfInfo> GetPersoninfCache(Sys.Safety.Request.R_Personinf.R_PersoninfGetRequest personinfRequest)
        {
            return _presonInfoService.GetPersoninfCache(personinfRequest);
        }

        [HttpPost]
        [Route("v1/R_Personinf/GetPersoninfCacheByBh")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PersoninfInfo>> GetPersoninfCacheByBh(Sys.Safety.Request.R_Personinf.R_PersoninfGetByBhRequest personinfRequest)
        {
            return _presonInfoService.GetPersoninfCacheByBh(personinfRequest);
        }
    }
}

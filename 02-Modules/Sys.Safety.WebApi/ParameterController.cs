using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Parameter;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class ParameterController : Basic.Framework.Web.WebApi.BasicApiController, IParameterService
    {
        static ParameterController()
        {

        }
        IParameterService _parameterService = ServiceFactory.Create<IParameterService>();

        [HttpPost]
        [Route("v1/Parameter/AddJC_Parameter")]
        public BasicResponse<JC_ParameterInfo> AddJC_Parameter(ParameterAddRequest jC_Parameterrequest)
        {
            return _parameterService.AddJC_Parameter(jC_Parameterrequest);
        }

        [HttpPost]
        [Route("v1/Parameter/UpdateJC_Parameter")]
        public BasicResponse<JC_ParameterInfo> UpdateJC_Parameter(ParameterUpdateRequest jC_Parameterrequest)
        {
            return _parameterService.UpdateJC_Parameter(jC_Parameterrequest);
        }
        [HttpPost]
        [Route("v1/Parameter/DeleteJC_Parameter")]

        public BasicResponse DeleteJC_Parameter(ParameterDeleteRequest jC_Parameterrequest)
        {
            return _parameterService.DeleteJC_Parameter(jC_Parameterrequest);
        }
        [HttpPost]
        [Route("v1/Parameter/GetJC_ParameterList")]
        public BasicResponse<List<JC_ParameterInfo>> GetJC_ParameterList(ParameterGetListRequest jC_Parameterrequest)
        {
            return _parameterService.GetJC_ParameterList(jC_Parameterrequest);
        }
        [HttpPost]
        [Route("v1/Parameter/GetJC_ParameterById")]
        public BasicResponse<JC_ParameterInfo> GetJC_ParameterById(ParameterGetRequest jC_Parameterrequest)
        {
            return _parameterService.GetJC_ParameterById(jC_Parameterrequest);
        }
    }
}

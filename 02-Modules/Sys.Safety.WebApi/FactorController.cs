using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Factor;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class FactorController: Basic.Framework.Web.WebApi.BasicApiController, IFactorService
   {
        static FactorController()
        {

        }
        IFactorService _factorService = ServiceFactory.Create<IFactorService>();
      
      
        [HttpPost]
        [Route("v1/Factor/AddJC_Factor")]
        public BasicResponse<JC_FactorInfo> AddJC_Factor(FactorAddRequest jC_Factorrequest)
        {
            return _factorService.AddJC_Factor(jC_Factorrequest);
        }
        [HttpPost]
        [Route("v1/Factor/UpdateJC_Factor")]
        public BasicResponse<JC_FactorInfo> UpdateJC_Factor(FactorUpdateRequest jC_Factorrequest)
        {
            return _factorService.UpdateJC_Factor(jC_Factorrequest);
        }
        [HttpPost]
        [Route("v1/Factor/DeleteJC_Factor")]
        public BasicResponse DeleteJC_Factor(FactorDeleteRequest jC_Factorrequest)
        {
            return _factorService.DeleteJC_Factor(jC_Factorrequest);
        }
        [HttpPost]
        [Route("v1/Factor/GetJC_FactorList")]
        public BasicResponse<List<JC_FactorInfo>> GetJC_FactorList(FactorGetListRequest jC_Factorrequest)
        {
            return _factorService.GetJC_FactorList(jC_Factorrequest);
        }
        [HttpPost]
        [Route("v1/Factor/GetJC_FactorById")]
        public BasicResponse<JC_FactorInfo> GetJC_FactorById(FactorGetRequest jC_Factorrequest)
        {
            return _factorService.GetJC_FactorById(jC_Factorrequest);
        }
   }
}

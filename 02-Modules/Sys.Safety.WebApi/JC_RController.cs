using Basic.Framework.Service;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi;
using Sys.Safety.Request.Jc_R;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class JC_RController : BasicApiController, IJc_RService
    {
        IJc_RService jc_rService = ServiceFactory.Create<IJc_RService>();


        [HttpPost]
        [Route("v1/JC_R/AddJc_R")]
        public BasicResponse<DataContract.Jc_RInfo> AddJc_R(Jc_RAddRequest jc_Rrequest)
        {
            return jc_rService.AddJc_R(jc_Rrequest);
        }

        [HttpPost]
        [Route("v1/JC_R/UpdateJc_R")]
        public BasicResponse<DataContract.Jc_RInfo> UpdateJc_R(Jc_RUpdateRequest jc_Rrequest)
        {
            return jc_rService.UpdateJc_R(jc_Rrequest);
        }

        [HttpPost]
        [Route("v1/JC_R/DeleteJc_R")]
        public BasicResponse DeleteJc_R(Jc_RDeleteRequest jc_Rrequest)
        {
            return jc_rService.DeleteJc_R(jc_Rrequest);
        }

        [HttpPost]
        [Route("v1/JC_R/GetJc_RList")]
        public BasicResponse<List<DataContract.Jc_RInfo>> GetJc_RList(Jc_RGetListRequest jc_Rrequest)
        {
            return jc_rService.GetJc_RList(jc_Rrequest);
        }

        [HttpPost]
        [Route("v1/JC_R/GetJc_RById")]
        public BasicResponse<DataContract.Jc_RInfo> GetJc_RById(Jc_RGetRequest jc_Rrequest)
        {
            return jc_rService.GetJc_RById(jc_Rrequest);
        }

        [HttpPost]
        [Route("v1/JC_R/GetJc_RByDataAndId")]
        public BasicResponse<DataContract.Jc_RInfo> GetJc_RByDataAndId(Jc_RGetByDateAndIdRequest jc_Rrequest)
        {
            return jc_rService.GetJc_RByDataAndId(jc_Rrequest);
        }
    }
}

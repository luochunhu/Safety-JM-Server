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
    public class B_MusicfilesController : Basic.Framework.Web.WebApi.BasicApiController, IB_MusicfilesService
    {
        private IB_MusicfilesService musicfilesService = ServiceFactory.Create<IB_MusicfilesService>();

        [HttpPost]
        [Route("v1/B_Musicfiles/AddB_Musicfiles")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.B_MusicfilesInfo> AddB_Musicfiles(Sys.Safety.Request.B_Musicfiles.B_MusicfilesAddRequest b_MusicfilesRequest)
        {
            return musicfilesService.AddB_Musicfiles(b_MusicfilesRequest);
        }


        [HttpPost]
        [Route("v1/B_Musicfiles/UpdateB_Musicfiles")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.B_MusicfilesInfo> UpdateB_Musicfiles(Sys.Safety.Request.B_Musicfiles.B_MusicfilesUpdateRequest b_MusicfilesRequest)
        {
            return musicfilesService.UpdateB_Musicfiles(b_MusicfilesRequest);
        }


        [HttpPost]
        [Route("v1/B_Musicfiles/DeleteB_Musicfiles")]
        public Basic.Framework.Web.BasicResponse DeleteB_Musicfiles(Sys.Safety.Request.B_Musicfiles.B_MusicfilesDeleteRequest b_MusicfilesRequest)
        {
            return musicfilesService.DeleteB_Musicfiles(b_MusicfilesRequest);
        }


        [HttpPost]
        [Route("v1/B_Musicfiles/GetB_MusicfilesList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.B_MusicfilesInfo>> GetB_MusicfilesList(Sys.Safety.Request.B_Musicfiles.B_MusicfilesGetListRequest b_MusicfilesRequest)
        {
            return musicfilesService.GetB_MusicfilesList(b_MusicfilesRequest);
        }

        [HttpPost]
        [Route("v1/B_Musicfiles/GetB_MusicfilesById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.B_MusicfilesInfo> GetB_MusicfilesById(Sys.Safety.Request.B_Musicfiles.B_MusicfilesGetRequest b_MusicfilesRequest)
        {
            return musicfilesService.GetB_MusicfilesById(b_MusicfilesRequest);
        }
    }
}

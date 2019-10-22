using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi
{
    public class ListdisplayexController : Basic.Framework.Web.WebApi.BasicApiController, IListdisplayexService
    {
        IListdisplayexService _listdisplayexService = ServiceFactory.Create<IListdisplayexService>();

        [HttpPost]
        [Route("v1/Listdisplayex/AddListdisplayex")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdisplayexInfo> AddListdisplayex(Sys.Safety.Request.Listdisplayex.ListdisplayexAddRequest listdisplayexrequest)
        {
            return _listdisplayexService.AddListdisplayex(listdisplayexrequest);
        }

        [HttpPost]
        [Route("v1/Listdisplayex/UpdateListdisplayex")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdisplayexInfo> UpdateListdisplayex(Sys.Safety.Request.Listdisplayex.ListdisplayexUpdateRequest listdisplayexrequest)
        {
            return _listdisplayexService.UpdateListdisplayex(listdisplayexrequest);
        }

        [HttpPost]
        [Route("v1/Listdisplayex/DeleteListdisplayex")]
        public Basic.Framework.Web.BasicResponse DeleteListdisplayex(Sys.Safety.Request.Listdisplayex.ListdisplayexDeleteRequest listdisplayexrequest)
        {
            return _listdisplayexService.DeleteListdisplayex(listdisplayexrequest);
        }

        [HttpPost]
        [Route("v1/Listdisplayex/GetListdisplayexList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ListdisplayexInfo>> GetListdisplayexList(Sys.Safety.Request.Listdisplayex.ListdisplayexGetListRequest listdisplayexrequest)
        {
            return _listdisplayexService.GetListdisplayexList(listdisplayexrequest);
        }

        [HttpPost]
        [Route("v1/Listdisplayex/GetListdisplayexById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdisplayexInfo> GetListdisplayexById(Sys.Safety.Request.Listdisplayex.ListdisplayexGetRequest listdisplayexrequest)
        {
            return _listdisplayexService.GetListdisplayexById(listdisplayexrequest);
        }

        [HttpPost]
        [Route("v1/Listdisplayex/SaveListDisplayExInfo")]
        public Basic.Framework.Web.BasicResponse SaveListDisplayExInfo(Sys.Safety.Request.Listdisplayex.SaveListDisplayExInfoRequest request)
        {
            return _listdisplayexService.SaveListDisplayExInfo(request);
        }
    }
}

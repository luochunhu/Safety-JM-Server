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
    public class ListdataexController : Basic.Framework.Web.WebApi.BasicApiController, IListdataexService
    {
        IListdataexService _listdataexService = ServiceFactory.Create<IListdataexService>();

        [HttpPost]
        [Route("v1/Listdataex/AddListdataex")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataexInfo> AddListdataex(Sys.Safety.Request.Listdataex.ListdataexAddRequest listdataexrequest)
        {
            return _listdataexService.AddListdataex(listdataexrequest);
        }

        [HttpPost]
        [Route("v1/Listdataex/UpdateListdataex")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataexInfo> UpdateListdataex(Sys.Safety.Request.Listdataex.ListdataexUpdateRequest listdataexrequest)
        {
            return _listdataexService.UpdateListdataex(listdataexrequest);
        }

        [HttpPost]
        [Route("v1/Listdataex/DeleteListdataex")]
        public Basic.Framework.Web.BasicResponse DeleteListdataex(Sys.Safety.Request.Listdataex.ListdataexDeleteRequest listdataexrequest)
        {
            return _listdataexService.DeleteListdataex(listdataexrequest);
        }

        [HttpPost]
        [Route("v1/Listdataex/GetListdataexList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ListdataexInfo>> GetListdataexList(Sys.Safety.Request.Listdataex.ListdataexGetListRequest listdataexrequest)
        {
            return _listdataexService.GetListdataexList(listdataexrequest);
        }

        [HttpPost]
        [Route("v1/Listdataex/GetListdataexById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataexInfo> GetListdataexById(Sys.Safety.Request.Listdataex.ListdataexGetRequest listdataexrequest)
        {
            return _listdataexService.GetListdataexById(listdataexrequest);
        }

        [HttpPost]
        [Route("v1/Listdataex/GetListDataExEntity")]
        public Basic.Framework.Web.BasicResponse<IList<Sys.Safety.DataContract.ListdataexInfo>> GetListDataExEntity(Sys.Safety.Request.Listdataex.ListdataexGetBySqlRequest strHql)
        {
            return _listdataexService.GetListDataExEntity(strHql);
        }

        [HttpPost]
        [Route("v1/Listdataex/SaveListDataExInfo")]
        public Basic.Framework.Web.BasicResponse SaveListDataExInfo(Sys.Safety.Request.Listdataex.SaveListDataExInfoRequest request)
        {
            return _listdataexService.SaveListDataExInfo(request);
        }
    }
}

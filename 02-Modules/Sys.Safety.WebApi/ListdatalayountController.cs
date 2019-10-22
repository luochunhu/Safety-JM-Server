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
    public class ListdatalayountController : Basic.Framework.Web.WebApi.BasicApiController, IListdatalayountService
    {
        IListdatalayountService _listdatalayountService = ServiceFactory.Create<IListdatalayountService>();

        [HttpPost]
        [Route("v1/Listdatalayount/AddListdatalayount")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdatalayountInfo> AddListdatalayount(Sys.Safety.Request.Listdatalayount.ListdatalayountAddRequest listdatalayountrequest)
        {
            return _listdatalayountService.AddListdatalayount(listdatalayountrequest);
        }

        [HttpPost]
        [Route("v1/Listdatalayount/UpdateListdatalayount")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdatalayountInfo> UpdateListdatalayount(Sys.Safety.Request.Listdatalayount.ListdatalayountUpdateRequest listdatalayountrequest)
        {
            return _listdatalayountService.UpdateListdatalayount(listdatalayountrequest);
        }

        [HttpPost]
        [Route("v1/Listdatalayount/DeleteListdatalayount")]
        public Basic.Framework.Web.BasicResponse DeleteListdatalayount(Sys.Safety.Request.Listdatalayount.ListdatalayountDeleteRequest listdatalayountrequest)
        {
            return _listdatalayountService.DeleteListdatalayount(listdatalayountrequest);
        }

        [HttpPost]
        [Route("v1/Listdatalayount/GetListdatalayountList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ListdatalayountInfo>> GetListdatalayountList(Sys.Safety.Request.Listdatalayount.ListdatalayountGetListRequest listdatalayountrequest)
        {
            return _listdatalayountService.GetListdatalayountList(listdatalayountrequest);
        }

        [HttpPost]
        [Route("v1/Listdatalayount/GetListdatalayountById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdatalayountInfo> GetListdatalayountById(Sys.Safety.Request.Listdatalayount.ListdatalayountGetRequest listdatalayountrequest)
        {
            return _listdatalayountService.GetListdatalayountById(listdatalayountrequest);
        }

        [HttpPost]
        [Route("v1/Listdatalayount/SaveListDataLayountInfo")]
        public Basic.Framework.Web.BasicResponse SaveListDataLayountInfo(Sys.Safety.Request.Listdatalayount.SaveListDataLayountInfoRequest request)
        {
            return _listdatalayountService.SaveListDataLayountInfo(request);
        }

        [HttpPost]
        [Route("v1/Listdatalayount/DeleteListdatalayountByTimeListDataId")]
        public Basic.Framework.Web.BasicResponse DeleteListdatalayountByTimeListDataId(Sys.Safety.Request.Listdatalayount.DeleteListdatalayountByTimeListDataIdRequest request)
        {
            return _listdatalayountService.DeleteListdatalayountByTimeListDataId(request);
        }

        [HttpPost]
        [Route("v1/Listdatalayount/GetListdatalayountByListDataId")]
        public Basic.Framework.Web.BasicResponse<IList<Sys.Safety.DataContract.ListdatalayountInfo>> GetListdatalayountByListDataId(Sys.Safety.Request.Listdatalayount.GetListdatalayountByListDataIdRequest request)
        {
            return _listdatalayountService.GetListdatalayountByListDataId(request);
        }

        [HttpPost]
        [Route("v1/Listdatalayount/GetListdatalayountByListDataIdArrangeTime")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdatalayountInfo> GetListdatalayountByListDataIdArrangeName(Sys.Safety.Request.Listdatalayount.GetListdatalayountByListDataIdArrangeTimeRequest request)
        {
            return _listdatalayountService.GetListdatalayountByListDataIdArrangeName(request);
        }
    }
}

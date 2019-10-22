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
    public class ListtempleController : Basic.Framework.Web.WebApi.BasicApiController,IListtempleService
    {
        IListtempleService _service = ServiceFactory.Create<IListtempleService>();

        [HttpPost]
        [Route("v1/Listtemple/AddListtemple")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListtempleInfo> AddListtemple(Sys.Safety.Request.Listtemple.ListtempleAddRequest listtemplerequest)
        {
            return _service.AddListtemple(listtemplerequest);
        }

        [HttpPost]
        [Route("v1/Listtemple/UpdateListtemple")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListtempleInfo> UpdateListtemple(Sys.Safety.Request.Listtemple.ListtempleUpdateRequest listtemplerequest)
        {
            return _service.UpdateListtemple(listtemplerequest);
        }

        [HttpPost]
        [Route("v1/Listtemple/DeleteListtemple")]
        public Basic.Framework.Web.BasicResponse DeleteListtemple(Sys.Safety.Request.Listtemple.ListtempleDeleteRequest listtemplerequest)
        {
            return _service.DeleteListtemple(listtemplerequest);
        }

        [HttpPost]
        [Route("v1/Listtemple/GetListtempleList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ListtempleInfo>> GetListtempleList(Sys.Safety.Request.Listtemple.ListtempleGetListRequest listtemplerequest)
        {
            return _service.GetListtempleList(listtemplerequest);
        }

        [HttpPost]
        [Route("v1/Listtemple/GetListtempleById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListtempleInfo> GetListtempleById(Sys.Safety.Request.Listtemple.ListtempleGetRequest listtemplerequest)
        {
            return _service.GetListtempleById(listtemplerequest);
        }

        [HttpPost]
        [Route("v1/Listtemple/SaveListTempleInfo")]
        public Basic.Framework.Web.BasicResponse SaveListTempleInfo(Sys.Safety.Request.Listtemple.SaveListTempleInfoRequest request)
        {
            return _service.SaveListTempleInfo(request);
        }

        [HttpPost]
        [Route("v1/Listtemple/GetListtempleByListDataID")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListtempleInfo> GetListtempleByListDataID(Sys.Safety.Request.Listex.IdRequest request)
        {
            return _service.GetListtempleByListDataID(request);
        }

        [HttpPost]
        [Route("v1/Listtemple/GetNameFromListDataExListEx")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetNameFromListDataExListEx(Sys.Safety.Request.Listex.IdRequest request)
        {
            return _service.GetNameFromListDataExListEx(request);
        }
    }
}

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
    public class ListdataremarkController : Basic.Framework.Web.WebApi.BasicApiController, IListdataremarkService
    {
        readonly IListdataremarkService _listdataremarkService = ServiceFactory.Create<IListdataremarkService>();

        [HttpPost]
        [Route("v1/Listdataremark/AddListdataremark")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataremarkInfo> AddListdataremark(Sys.Safety.Request.Listdataremark.ListdataremarkAddRequest listdataremarkRequest)
        {
            return _listdataremarkService.AddListdataremark(listdataremarkRequest);
        }

        [HttpPost]
        [Route("v1/Listdataremark/UpdateListdataremark")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataremarkInfo> UpdateListdataremark(Sys.Safety.Request.Listdataremark.ListdataremarkUpdateRequest listdataremarkRequest)
        {
            return _listdataremarkService.UpdateListdataremark(listdataremarkRequest);
        }

        [HttpPost]
        [Route("v1/Listdataremark/DeleteListdataremark")]
        public Basic.Framework.Web.BasicResponse DeleteListdataremark(Sys.Safety.Request.Listdataremark.ListdataremarkDeleteRequest listdataremarkRequest)
        {
            return _listdataremarkService.DeleteListdataremark(listdataremarkRequest);
        }

        [HttpPost]
        [Route("v1/Listdataremark/GetListdataremarkList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ListdataremarkInfo>> GetListdataremarkList(Sys.Safety.Request.Listdataremark.ListdataremarkGetListRequest listdataremarkRequest)
        {
            return _listdataremarkService.GetListdataremarkList(listdataremarkRequest);
        }

        [HttpPost]
        [Route("v1/Listdataremark/GetListdataremarkById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataremarkInfo> GetListdataremarkById(Sys.Safety.Request.Listdataremark.ListdataremarkGetRequest listdataremarkRequest)
        {
            return _listdataremarkService.GetListdataremarkById(listdataremarkRequest);
        }

        [HttpPost]
        [Route("v1/Listdataremark/GetListdataremarkByTimeListDataId")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataremarkInfo> GetListdataremarkByTimeListDataId(Sys.Safety.Request.Listdataremark.GetListdataremarkByTimeListDataIdRequest getListdataremarkByTimeListDataIdRequest)
        {
            return _listdataremarkService.GetListdataremarkByTimeListDataId(getListdataremarkByTimeListDataIdRequest);
        }

        [HttpPost]
        [Route("v1/Listdataremark/UpdateListdataremarkByTimeListDataId")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataremarkInfo> UpdateListdataremarkByTimeListDataId(Sys.Safety.Request.Listdataremark.ListdataremarkUpdateRequest listdataremarkRequest)
        {
            return _listdataremarkService.UpdateListdataremarkByTimeListDataId(listdataremarkRequest);
        }
    }
}

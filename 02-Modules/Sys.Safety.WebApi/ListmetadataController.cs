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
    public class ListmetadataController : Basic.Framework.Web.WebApi.BasicApiController, IListmetadataService
    {
        IListmetadataService _service = ServiceFactory.Create<IListmetadataService>();

        [HttpPost]
        [Route("v1/Listmetadata/AddListmetadata")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListmetadataInfo> AddListmetadata(Sys.Safety.Request.Listmetadata.ListmetadataAddRequest listmetadatarequest)
        {
            return _service.AddListmetadata(listmetadatarequest);
        }

        [HttpPost]
        [Route("v1/Listmetadata/UpdateListmetadata")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListmetadataInfo> UpdateListmetadata(Sys.Safety.Request.Listmetadata.ListmetadataUpdateRequest listmetadatarequest)
        {
            return _service.UpdateListmetadata(listmetadatarequest);
        }

        [HttpPost]
        [Route("v1/Listmetadata/DeleteListmetadata")]
        public Basic.Framework.Web.BasicResponse DeleteListmetadata(Sys.Safety.Request.Listmetadata.ListmetadataDeleteRequest listmetadatarequest)
        {
            return _service.DeleteListmetadata(listmetadatarequest);
        }

        [HttpPost]
        [Route("v1/Listmetadata/GetListmetadataList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ListmetadataInfo>> GetListmetadataList(Sys.Safety.Request.Listmetadata.ListmetadataGetListRequest listmetadatarequest)
        {
            return _service.GetListmetadataList(listmetadatarequest);
        }

        [HttpPost]
        [Route("v1/Listmetadata/GetListmetadataById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListmetadataInfo> GetListmetadataById(Sys.Safety.Request.Listmetadata.ListmetadataGetRequest listmetadatarequest)
        {
            return _service.GetListmetadataById(listmetadatarequest);
        }

        [HttpPost]
        [Route("v1/Listmetadata/SaveListMetaDataExInfo")]
        public Basic.Framework.Web.BasicResponse SaveListMetaDataExInfo(Sys.Safety.Request.Listmetadata.SaveListMetaDataExInfoRequest request)
        {
            return _service.SaveListMetaDataExInfo(request);
        }
    }
}

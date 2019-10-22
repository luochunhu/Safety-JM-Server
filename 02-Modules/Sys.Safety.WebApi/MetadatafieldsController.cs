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
    public class MetadatafieldsController : Basic.Framework.Web.WebApi.BasicApiController, IMetadataService
    {
        IMetadataService _service = ServiceFactory.Create<IMetadataService>();

        [HttpPost]
        [Route("v1/Metadatafields/AddMetadata")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MetadataInfo> AddMetadata(Sys.Safety.Request.Metadata.MetadataAddRequest metadatarequest)
        {
            return _service.AddMetadata(metadatarequest);
        }

        [HttpPost]
        [Route("v1/Metadatafields/UpdateMetadata")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MetadataInfo> UpdateMetadata(Sys.Safety.Request.Metadata.MetadataUpdateRequest metadatarequest)
        {
            return _service.UpdateMetadata(metadatarequest);
        }

        [HttpPost]
        [Route("v1/Metadatafields/DeleteMetadata")]
        public Basic.Framework.Web.BasicResponse DeleteMetadata(Sys.Safety.Request.Metadata.MetadataDeleteRequest metadatarequest)
        {
            return _service.DeleteMetadata(metadatarequest);
        }

        [HttpPost]
        [Route("v1/Metadatafields/GetMetadataList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.MetadataInfo>> GetMetadataList(Sys.Safety.Request.Metadata.MetadataGetListRequest metadatarequest)
        {
            return _service.GetMetadataList(metadatarequest);
        }

        [HttpPost]
        [Route("v1/Metadatafields/GetMetadataById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MetadataInfo> GetMetadataById(Sys.Safety.Request.Metadata.MetadataGetRequest metadatarequest)
        {
            return _service.GetMetadataById(metadatarequest);
        }

        [HttpPost]
        [Route("v1/Metadatafields/ImportMetadata")]
        public Basic.Framework.Web.BasicResponse ImportMetadata(Sys.Safety.Request.Metadata.ImportMetadataRequest request)
        {
            return _service.ImportMetadata(request);
        }
    }
}

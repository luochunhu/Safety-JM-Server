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
    public class MetadataController : Basic.Framework.Web.WebApi.BasicApiController, IMetadatafieldsService
    {
        IMetadatafieldsService _service = ServiceFactory.Create<IMetadatafieldsService>();

        [HttpPost]
        [Route("v1/Metadata/AddMetadatafields")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MetadatafieldsInfo> AddMetadatafields(Sys.Safety.Request.Metadatafields.MetadatafieldsAddRequest metadatafieldsrequest)
        {
            return _service.AddMetadatafields(metadatafieldsrequest);
        }

        [HttpPost]
        [Route("v1/Metadata/UpdateMetadatafields")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MetadatafieldsInfo> UpdateMetadatafields(Sys.Safety.Request.Metadatafields.MetadatafieldsUpdateRequest metadatafieldsrequest)
        {
            return _service.UpdateMetadatafields(metadatafieldsrequest);
        }

        [HttpPost]
        [Route("v1/Metadata/DeleteMetadatafields")]
        public Basic.Framework.Web.BasicResponse DeleteMetadatafields(Sys.Safety.Request.Metadatafields.MetadatafieldsDeleteRequest metadatafieldsrequest)
        {
            return _service.DeleteMetadatafields(metadatafieldsrequest);
        }

        [HttpPost]
        [Route("v1/Metadata/GetMetadatafieldsList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.MetadatafieldsInfo>> GetMetadatafieldsList(Sys.Safety.Request.Metadatafields.MetadatafieldsGetListRequest metadatafieldsrequest)
        {
            return _service.GetMetadatafieldsList(metadatafieldsrequest);
        }

        [HttpPost]
        [Route("v1/Metadata/GetMetadatafieldsById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MetadatafieldsInfo> GetMetadatafieldsById(Sys.Safety.Request.Metadatafields.MetadatafieldsGetRequest metadatafieldsrequest)
        {
            return _service.GetMetadatafieldsById(metadatafieldsrequest);
        }
    }
}

using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Metadatafields;

namespace Sys.Safety.ServiceContract
{
    public interface IMetadatafieldsService
    {
        BasicResponse<MetadatafieldsInfo> AddMetadatafields(MetadatafieldsAddRequest metadatafieldsrequest);
        BasicResponse<MetadatafieldsInfo> UpdateMetadatafields(MetadatafieldsUpdateRequest metadatafieldsrequest);
        BasicResponse DeleteMetadatafields(MetadatafieldsDeleteRequest metadatafieldsrequest);
        BasicResponse<List<MetadatafieldsInfo>> GetMetadatafieldsList(MetadatafieldsGetListRequest metadatafieldsrequest);
        BasicResponse<MetadatafieldsInfo> GetMetadatafieldsById(MetadatafieldsGetRequest metadatafieldsrequest);
    }
}
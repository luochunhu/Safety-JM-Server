using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Metadata;

namespace Sys.Safety.ServiceContract
{
    public interface IMetadataService
    {
        BasicResponse<MetadataInfo> AddMetadata(MetadataAddRequest metadatarequest);
        BasicResponse<MetadataInfo> UpdateMetadata(MetadataUpdateRequest metadatarequest);
        BasicResponse DeleteMetadata(MetadataDeleteRequest metadatarequest);
        BasicResponse<List<MetadataInfo>> GetMetadataList(MetadataGetListRequest metadatarequest);
        BasicResponse<MetadataInfo> GetMetadataById(MetadataGetRequest metadatarequest);

        BasicResponse ImportMetadata(ImportMetadataRequest request);
    }
}
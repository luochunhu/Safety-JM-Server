using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listmetadata;

namespace Sys.Safety.ServiceContract
{
    public interface IListmetadataService
    {
        BasicResponse<ListmetadataInfo> AddListmetadata(ListmetadataAddRequest listmetadatarequest);
        BasicResponse<ListmetadataInfo> UpdateListmetadata(ListmetadataUpdateRequest listmetadatarequest);
        BasicResponse DeleteListmetadata(ListmetadataDeleteRequest listmetadatarequest);
        BasicResponse<List<ListmetadataInfo>> GetListmetadataList(ListmetadataGetListRequest listmetadatarequest);
        BasicResponse<ListmetadataInfo> GetListmetadataById(ListmetadataGetRequest listmetadatarequest);

        BasicResponse SaveListMetaDataExInfo(SaveListMetaDataExInfoRequest request);
    }
}
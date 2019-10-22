using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IMetadatafieldsRepository : IRepository<MetadatafieldsModel>
    {
        MetadatafieldsModel AddMetadatafields(MetadatafieldsModel metadatafieldsModel);
        void UpdateMetadatafields(MetadatafieldsModel metadatafieldsModel);
        void DeleteMetadatafields(string id);
        IList<MetadatafieldsModel> GetMetadatafieldsList(int pageIndex, int pageSize, out int rowCount);
        MetadatafieldsModel GetMetadatafieldsById(string id);

        IList<MetadatafieldsModel> GetMetadatafieldsListAll();
    }
}
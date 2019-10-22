using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IMetadataRepository : IRepository<MetadataModel>
    {
        MetadataModel AddMetadata(MetadataModel metadataModel);
        void UpdateMetadata(MetadataModel metadataModel);
        void DeleteMetadata(string id);
        IList<MetadataModel> GetMetadataList(int pageIndex, int pageSize, out int rowCount);
        MetadataModel GetMetadataById(string id);

        IList<MetadataModel> GetMetadataListAll();
    }
}
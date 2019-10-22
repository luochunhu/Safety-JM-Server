using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IListmetadataRepository : IRepository<ListmetadataModel>
    {
        ListmetadataModel AddListmetadata(ListmetadataModel listmetadataModel);
        void UpdateListmetadata(ListmetadataModel listmetadataModel);
        void DeleteListmetadata(string id);
        IList<ListmetadataModel> GetListmetadataList(int pageIndex, int pageSize, out int rowCount);
        ListmetadataModel GetListmetadataById(string id);

        IList<ListmetadataModel> GetListmetadataListByListDataId(int id);

        void DeleteListmetadataByListDataID(string id);
    }
}
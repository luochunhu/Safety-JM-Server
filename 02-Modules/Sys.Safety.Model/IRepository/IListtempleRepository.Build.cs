using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IListtempleRepository : IRepository<ListtempleModel>
    {
        ListtempleModel AddListtemple(ListtempleModel listtempleModel);
        void UpdateListtemple(ListtempleModel listtempleModel);
        void DeleteListtemple(string id);
        IList<ListtempleModel> GetListtempleList(int pageIndex, int pageSize, out int rowCount);
        ListtempleModel GetListtempleById(string id);

        ListtempleModel GetListtempleByListDataID(string id);
    }
}
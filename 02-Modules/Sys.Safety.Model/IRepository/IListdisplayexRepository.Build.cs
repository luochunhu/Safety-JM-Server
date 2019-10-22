using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IListdisplayexRepository : IRepository<ListdisplayexModel>
    {
        ListdisplayexModel AddListdisplayex(ListdisplayexModel listdisplayexModel);
        void UpdateListdisplayex(ListdisplayexModel listdisplayexModel);
        void DeleteListdisplayex(string id);
        IList<ListdisplayexModel> GetListdisplayexList(int pageIndex, int pageSize, out int rowCount);
        ListdisplayexModel GetListdisplayexById(string id);

        IList<ListdisplayexModel> GetListdisplayexListByListDataId(int id);

        void DeleteListdisplayexByListDataID(string id);
    }
}
using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IListdataexRepository : IRepository<ListdataexModel>
    {
        ListdataexModel AddListdataex(ListdataexModel listdataexModel);
        void UpdateListdataex(ListdataexModel listdataexModel);
        void DeleteListdataex(string id);
        IList<ListdataexModel> GetListdataexList(int pageIndex, int pageSize, out int rowCount);
        ListdataexModel GetListdataexById(string id);

        IList<ListdataexModel> GetListdataexListByListId(int listId);

        void DeleteListdataexByListId(int id);

        void DeleteListdataexByListIdListDataId(string listId, string listDataId);

        IList<ListdataexModel> GetListdataexListByThreeId(string listID, string userID, string listDataID);

    }
}
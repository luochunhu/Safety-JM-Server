using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ListdataexRepository : RepositoryBase<ListdataexModel>, IListdataexRepository
    {
        public ListdataexModel AddListdataex(ListdataexModel listdataexModel)
        {
            return Insert(listdataexModel);
        }

        public void UpdateListdataex(ListdataexModel listdataexModel)
        {
            Update(listdataexModel);
        }

        public void DeleteListdataex(string id)
        {
            Delete(Convert.ToInt32(id));
        }

        public IList<ListdataexModel> GetListdataexList(int pageIndex, int pageSize, out int rowCount)
        {
            var listdataexModelLists = Datas;
            rowCount = listdataexModelLists.Count();
            return listdataexModelLists.OrderBy(a => a.ListID).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public ListdataexModel GetListdataexById(string id)
        {
            var iId = Convert.ToInt32(id);
            var listdataexModel = Datas.FirstOrDefault(c => c.ListDataID == iId);
            return listdataexModel;
        }

        public IList<ListdataexModel> GetListdataexListByListId(int listId)
        {
            var model = from m in Datas
                where m.ListID == listId
                orderby m.BlnDefault descending
                select m;
            return model.ToList();
        }

        public void DeleteListdataexByListId(int id)
        {
            var model = from m in Datas
                where m.ListID == id
                select m;
            var firstOrDefault = model.FirstOrDefault();
            if (firstOrDefault != null) Delete(firstOrDefault.ListDataID);
        }

        public void DeleteListdataexByListIdListDataId(string listId, string listDataId)
        {
            var iListId = Convert.ToInt32(listId);
            var iListDataId = Convert.ToInt32(listDataId);
            var model = from m in Datas
                where m.ListID == iListId && m.ListDataID == iListDataId
                select m;
            Delete(model.ToList());
        }

        public IList<ListdataexModel> GetListdataexListByThreeId(string listID, string userID, string listDataID)
        {
            var iListId = Convert.ToInt32(listID);
            var iUserId = Convert.ToInt32(userID);
            var iListDataId = Convert.ToInt32(listDataID);
            var model = from m in Datas
                where
                m.ListID == iListId && m.UserID == iUserId && m.ListDataID != iListDataId
                select m;
            return model.ToList();
        }
    }
}
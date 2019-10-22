using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ListdisplayexRepository : RepositoryBase<ListdisplayexModel>, IListdisplayexRepository
    {
        public ListdisplayexModel AddListdisplayex(ListdisplayexModel listdisplayexModel)
        {
            return Insert(listdisplayexModel);
        }

        public void UpdateListdisplayex(ListdisplayexModel listdisplayexModel)
        {
            Update(listdisplayexModel);
        }

        public void DeleteListdisplayex(string id)
        {
            Delete(Convert.ToInt32(id));
        }

        public IList<ListdisplayexModel> GetListdisplayexList(int pageIndex, int pageSize, out int rowCount)
        {
            var listdisplayexModelLists = Datas;
            rowCount = listdisplayexModelLists.Count();
            return listdisplayexModelLists.OrderBy(a => a.ListDataID).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public ListdisplayexModel GetListdisplayexById(string id)
        {
            var iId = Convert.ToInt32(id);
            var listdisplayexModel = Datas.FirstOrDefault(c => c.ListDisplayID == iId);
            return listdisplayexModel;
        }

        public IList<ListdisplayexModel> GetListdisplayexListByListDataId(int id)
        {
            var model = from m in Datas
                where m.ListDataID == id
                select m;
            return model.ToList();
        }

        public void DeleteListdisplayexByListDataID(string id)
        {
            var iId = Convert.ToInt32(id);
            var model = from m in Datas
                where m.ListDataID == iId
                select m;
            //var firstOrDefault = model.FirstOrDefault();
            //if (firstOrDefault != null) Delete(firstOrDefault.ListDisplayID);
            var lisModel = model.ToList();
            Delete(lisModel);
        }
    }
}
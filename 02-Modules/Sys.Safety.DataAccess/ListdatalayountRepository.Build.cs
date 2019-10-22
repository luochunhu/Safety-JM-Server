using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ListdatalayountRepository : RepositoryBase<ListdatalayountModel>, IListdatalayountRepository
    {
        public ListdatalayountModel AddListdatalayount(ListdatalayountModel listdatalayountModel)
        {
            return Insert(listdatalayountModel);
        }

        public void UpdateListdatalayount(ListdatalayountModel listdatalayountModel)
        {
            Update(listdatalayountModel);
        }

        public void DeleteListdatalayount(string id)
        {
            Delete(Convert.ToInt32(id));
        }

        public IList<ListdatalayountModel> GetListdatalayountList(int pageIndex, int pageSize, out int rowCount)
        {
            var listdatalayountModelLists = Datas;
            rowCount = listdatalayountModelLists.Count();
            return listdatalayountModelLists.OrderBy(a => a.StrDate).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public ListdatalayountModel GetListdatalayountById(string id)
        {
            var iId = Convert.ToInt32(id);
            var listdatalayountModel = Datas.FirstOrDefault(c => c.ListDataLayoutID == iId);
            return listdatalayountModel;
        }


        public void DeleteListdatalayountByTimeListDataId(string time, long listDataId)
        {
            var model = Datas.FirstOrDefault(a => a.ListDataID == listDataId && a.StrDate == time);
            if (model != null)
            {
                Delete(model.ListDataLayoutID);
            }
        }

        public IList<ListdatalayountModel> GetListdatalayountByListDataId(string listDataId)
        {
            int iListDataId = Convert.ToInt32(listDataId);
            return Datas.Where(a => a.ListDataID == iListDataId).ToList();
        }

        public ListdatalayountModel GetListdatalayountByListDataIdArrangeName(string listDataId, string arrangeName)
        {
            int iListDataId = Convert.ToInt32(listDataId);
            var ret = Datas.FirstOrDefault(a => a.ListDataID == iListDataId && a.StrDate == arrangeName);
            return ret;
        }
    }
}
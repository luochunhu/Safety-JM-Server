using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ListtempleRepository : RepositoryBase<ListtempleModel>, IListtempleRepository
    {
        public ListtempleModel AddListtemple(ListtempleModel listtempleModel)
        {
            return Insert(listtempleModel);
        }

        public void UpdateListtemple(ListtempleModel listtempleModel)
        {
            Update(listtempleModel);
        }

        public void DeleteListtemple(string id)
        {
            Delete(Convert.ToInt32(id));
        }

        public IList<ListtempleModel> GetListtempleList(int pageIndex, int pageSize, out int rowCount)
        {
            var listtempleModelLists = Datas;
            rowCount = listtempleModelLists.Count();
            return listtempleModelLists.OrderBy(a => a.ListDataID).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public ListtempleModel GetListtempleById(string id)
        {
            var iId = Convert.ToInt32(id);
            var listtempleModel = Datas.FirstOrDefault(c => c.ListTempleID == iId);
            return listtempleModel;
        }

        public ListtempleModel GetListtempleByListDataID(string id)
        {
            var iId = Convert.ToInt32(id);
            var model = from m in Datas
                where m.ListDataID == iId
                select m;
            return model.FirstOrDefault();
        }
    }
}
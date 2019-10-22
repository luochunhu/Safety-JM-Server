using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ListexRepository : RepositoryBase<ListexModel>, IListexRepository
    {
        public ListexModel AddListex(ListexModel listexModel)
        {
            return Insert(listexModel);
        }

        public void UpdateListex(ListexModel listexModel)
        {
            Update(listexModel);
        }

        public void DeleteListex(string id)
        {
            Delete(Convert.ToInt32(Convert.ToInt32(id)));
        }

        public IList<ListexModel> GetListexList(int pageIndex, int pageSize, out int rowCount)
        {
            var listexModelLists = Datas;
            rowCount = listexModelLists.Count();
            return listexModelLists.OrderBy(a => a.DirID).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public ListexModel GetListexById(string id)
        {
            var iId = Convert.ToInt32(id);
            var listexModel = Datas.FirstOrDefault(c => c.ListID == iId);
            return listexModel;
        }

        public IList<ListexModel> GetListexByStrGuidCode(string code)
        {
            var model = from m in Datas
                where m.StrGuidCode == code
                select m;
            return model.ToList();
        }
    }
}
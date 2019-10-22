using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ListcommandexRepository : RepositoryBase<ListcommandexModel>, IListcommandexRepository
    {
        public ListcommandexModel AddListcommandex(ListcommandexModel listcommandexModel)
        {
            return Insert(listcommandexModel);
        }

        public void UpdateListcommandex(ListcommandexModel listcommandexModel)
        {
            Update(listcommandexModel);
        }

        public void DeleteListcommandex(string id)
        {
            Delete(Convert.ToInt32(id));
        }

        public IList<ListcommandexModel> GetListcommandexList(int pageIndex, int pageSize, out int rowCount)
        {
            var listcommandexModelLists = Datas;
            rowCount = listcommandexModelLists.Count();
            return listcommandexModelLists.OrderBy(a => a.ListID).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public ListcommandexModel GetListcommandexById(string id)
        {
            var iId = Convert.ToInt32(id);
            var listcommandexModel = Datas.FirstOrDefault(c => c.ListCommandID == iId);
            return listcommandexModel;
        }
    }
}
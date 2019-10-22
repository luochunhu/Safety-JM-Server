using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ListmetadataRepository : RepositoryBase<ListmetadataModel>, IListmetadataRepository
    {
        public ListmetadataModel AddListmetadata(ListmetadataModel listmetadataModel)
        {
            return Insert(listmetadataModel);
        }

        public void UpdateListmetadata(ListmetadataModel listmetadataModel)
        {
            Update(listmetadataModel);
        }

        public void DeleteListmetadata(string id)
        {
            Delete(Convert.ToInt32(id));
        }

        public IList<ListmetadataModel> GetListmetadataList(int pageIndex, int pageSize, out int rowCount)
        {
            var listmetadataModelLists = Datas;
            rowCount = listmetadataModelLists.Count();
            return listmetadataModelLists.OrderBy(a => a.ID).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public ListmetadataModel GetListmetadataById(string id)
        {
            var iId = Convert.ToInt32(id);
            var listmetadataModel = Datas.FirstOrDefault(c => c.ID == iId);
            return listmetadataModel;
        }

        public IList<ListmetadataModel> GetListmetadataListByListDataId(int id)
        {
            var model = from m in Datas
                where m.ListDataID == id
                select m;
            return model.ToList();
        }

        public void DeleteListmetadataByListDataID(string id)
        {
            var iId = Convert.ToInt32(id);
            var model = from m in Datas
                where m.ListDataID == iId
                select m;
            //var firstOrDefault = model.FirstOrDefault();
            //if (firstOrDefault != null) Delete(firstOrDefault.ID);
            var lisModel = model.ToList();
            Delete(lisModel);
        }
    }
}
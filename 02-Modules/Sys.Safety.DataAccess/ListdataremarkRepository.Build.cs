using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ListdataremarkRepository : RepositoryBase<ListdataremarkModel>, IListdataremarkRepository
    {
        public ListdataremarkModel AddListdataremark(ListdataremarkModel listdataremarkModel)
        {
            listdataremarkModel.Listdataremarkid = IdHelper.CreateLongId().ToString();
            return Insert(listdataremarkModel);
        }

        public void UpdateListdataremark(ListdataremarkModel listdataremarkModel)
        {
            Update(listdataremarkModel);
        }

        public void DeleteListdataremark(string id)
        {
            Delete(id);
        }

        public IList<ListdataremarkModel> GetListdataremarkList(int pageIndex, int pageSize, out int rowCount)
        {
            var listdataremarkModelLists = Datas;
            rowCount = Datas.Count();
            return listdataremarkModelLists.OrderBy(a=>a.Listdataremarkid).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public ListdataremarkModel GetListdataremarkById(string id)
        {
            var listdataremarkModel = Datas.FirstOrDefault(c => c.Listdataremarkid == id);
            return listdataremarkModel;
        }

        public ListdataremarkModel GetListdataremarkByTimeListDataId(DateTime time, long listDataId)
        {
            var listdataremarkModel = Datas.FirstOrDefault(c => c.Listdataid == listDataId.ToString() && c.Time == time);
            return listdataremarkModel;
        }

        public void UpdateListdataremarkByTimeListDataId(ListdataremarkModel listdataremarkModel)
        {
            var model = Datas.FirstOrDefault(c => c.Listdataid == listdataremarkModel.Listdataid && c.Time == listdataremarkModel.Time);
            model.Remark = listdataremarkModel.Remark;
            Update(model);
        }
    }
}
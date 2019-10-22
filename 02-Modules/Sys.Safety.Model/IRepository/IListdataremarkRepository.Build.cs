using System;
using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IListdataremarkRepository : IRepository<ListdataremarkModel>
    {
        ListdataremarkModel AddListdataremark(ListdataremarkModel listdataremarkModel);
        void UpdateListdataremark(ListdataremarkModel listdataremarkModel);
        void DeleteListdataremark(string id);
        IList<ListdataremarkModel> GetListdataremarkList(int pageIndex, int pageSize, out int rowCount);
        ListdataremarkModel GetListdataremarkById(string id);
        ListdataremarkModel GetListdataremarkByTimeListDataId(DateTime time, long listDataId);
        void UpdateListdataremarkByTimeListDataId(ListdataremarkModel listdataremarkModel);

    }
}
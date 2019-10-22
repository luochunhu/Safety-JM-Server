using System;
using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IListdatalayountRepository : IRepository<ListdatalayountModel>
    {
        ListdatalayountModel AddListdatalayount(ListdatalayountModel listdatalayountModel);
        void UpdateListdatalayount(ListdatalayountModel listdatalayountModel);
        void DeleteListdatalayount(string id);
        IList<ListdatalayountModel> GetListdatalayountList(int pageIndex, int pageSize, out int rowCount);
        ListdatalayountModel GetListdatalayountById(string id);
        void DeleteListdatalayountByTimeListDataId(string time, long listDataId);

        /// <summary>
        /// 根据listdataid获取listdatalayount
        /// </summary>
        /// <param name="listDataId"></param>
        /// <returns></returns>
        IList<ListdatalayountModel> GetListdatalayountByListDataId(string listDataId);

        /// <summary>
        /// 根据listdataid、编排时间获取listdatalayount
        /// </summary>
        /// <param name="listDataId"></param>
        /// <param name="arrangeTime"></param>
        /// <returns></returns>
        ListdatalayountModel GetListdatalayountByListDataIdArrangeName(string listDataId, string arrangeTime);
    }
}
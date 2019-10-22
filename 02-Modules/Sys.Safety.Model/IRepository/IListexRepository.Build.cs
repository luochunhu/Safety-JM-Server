using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IListexRepository : IRepository<ListexModel>
    {
        ListexModel AddListex(ListexModel listexModel);
        void UpdateListex(ListexModel listexModel);
        void DeleteListex(string id);
        IList<ListexModel> GetListexList(int pageIndex, int pageSize, out int rowCount);
        ListexModel GetListexById(string id);

        IList<ListexModel> GetListexByStrGuidCode(string code);
    }
}
using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_DefRepository : IRepository<B_DefModel>
    {
        B_DefModel AddDef(B_DefModel defModel);
        void UpdateDef(B_DefModel defModel);
        void DeleteDef(string id);
        IList<B_DefModel> GetDefList(int pageIndex, int pageSize, out int rowCount);
        B_DefModel GetDefById(string id);
    }
}

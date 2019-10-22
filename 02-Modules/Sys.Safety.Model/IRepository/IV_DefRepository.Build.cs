using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IV_DefRepository : IRepository<V_DefModel>
    {
        V_DefModel AddDef(V_DefModel defModel);
        void UpdateDef(V_DefModel defModel);
        void DeleteDef(string id);
        IList<V_DefModel> GetDefList(int pageIndex, int pageSize, out int rowCount);
        V_DefModel GetDefById(string id);
    }
}

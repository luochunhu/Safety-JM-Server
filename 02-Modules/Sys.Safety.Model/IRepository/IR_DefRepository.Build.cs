using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_DefRepository : IRepository<R_DefModel>
    {
        R_DefModel AddDef(R_DefModel defModel);
        void UpdateDef(R_DefModel defModel);
        void DeleteDef(string id);
        IList<R_DefModel> GetDefList(int pageIndex, int pageSize, out int rowCount);
        R_DefModel GetDefById(string id);
    }
}

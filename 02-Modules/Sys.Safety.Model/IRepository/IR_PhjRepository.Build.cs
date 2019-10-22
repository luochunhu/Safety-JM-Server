using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_PhjRepository : IRepository<R_PhjModel>
    {
        R_PhjModel AddPhj(R_PhjModel phjModel);
        void UpdatePhj(R_PhjModel phjModel);
        void DeletePhj(string id);
        IList<R_PhjModel> GetPhjList(int pageIndex, int pageSize, out int rowCount);
        R_PhjModel GetPhjById(string id);
    }
}

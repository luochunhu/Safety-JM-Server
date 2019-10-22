using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_CallRepository : IRepository<R_CallModel>
    {
        R_CallModel AddCall(R_CallModel callModel);
        void UpdateCall(R_CallModel callModel);
        void DeleteCall(string id);
        IList<R_CallModel> GetCallList(int pageIndex, int pageSize, out int rowCount);
        R_CallModel GetCallById(string id);
    }
}

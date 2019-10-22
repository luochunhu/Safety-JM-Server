using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_PbRepository : IRepository<R_PbModel>
    {
        R_PbModel AddPb(R_PbModel pbModel);
        void UpdatePb(R_PbModel pbModel);
        void DeletePb(string id);
        IList<R_PbModel> GetPbList(int pageIndex, int pageSize, out int rowCount);
        R_PbModel GetPbById(string id);

        void BachUpdatePb(List< R_PbModel> pbModels);
    }
}

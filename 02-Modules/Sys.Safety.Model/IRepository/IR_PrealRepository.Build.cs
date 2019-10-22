using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_PrealRepository : IRepository<R_PrealModel>
    {
        R_PrealModel AddPreal(R_PrealModel prealModel);
        void UpdatePreal(R_PrealModel prealModel);
        void DeletePreal(string id);
        IList<R_PrealModel> GetPrealList(int pageIndex, int pageSize, out int rowCount);
        R_PrealModel GetPrealById(string id);
    }
}

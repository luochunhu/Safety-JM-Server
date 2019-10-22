using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_PersoninfRepository : IRepository<R_PersoninfModel>
    {
        R_PersoninfModel AddPersoninf(R_PersoninfModel personinfModel);
        void UpdatePersoninf(R_PersoninfModel personinfModel);
        void DeletePersoninf(string id);
        IList<R_PersoninfModel> GetPersoninfList(int pageIndex, int pageSize, out int rowCount);
        R_PersoninfModel GetPersoninfById(string id);
    }
}

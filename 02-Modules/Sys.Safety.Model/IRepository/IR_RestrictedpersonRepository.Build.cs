using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_RestrictedpersonRepository : IRepository<R_RestrictedpersonModel>
    {
        R_RestrictedpersonModel AddRestrictedperson(R_RestrictedpersonModel restrictedpersonModel);
        void UpdateRestrictedperson(R_RestrictedpersonModel restrictedpersonModel);
        void DeleteRestrictedperson(string id);
        void DeleteRestrictedpersonByPointId(string PointId);
        IList<R_RestrictedpersonModel> GetRestrictedpersonList(int pageIndex, int pageSize, out int rowCount);
        R_RestrictedpersonModel GetRestrictedpersonById(string id);
    }
}

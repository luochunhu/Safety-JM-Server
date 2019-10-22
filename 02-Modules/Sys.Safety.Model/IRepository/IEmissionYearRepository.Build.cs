using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmissionYearRepository : IRepository<Jc_Ll_YModel>
    {
        Jc_Ll_YModel AddEmissionYear(Jc_Ll_YModel jc_Ll_YModel);
        void UpdateEmissionYear(Jc_Ll_YModel jc_Ll_YModel);
        void DeleteEmissionYear(string id);
        IList<Jc_Ll_YModel> GetEmissionYearList(int pageIndex, int pageSize, out int rowCount);
        Jc_Ll_YModel GetEmissionYearById(string id);
    }
}
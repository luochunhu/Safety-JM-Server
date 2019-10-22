using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmissionMonthRepository : IRepository<Jc_Ll_MModel>
    {
        Jc_Ll_MModel AddEmissionMonth(Jc_Ll_MModel jc_Ll_MModel);
        void UpdateEmissionMonth(Jc_Ll_MModel jc_Ll_MModel);
        void DeleteEmissionMonth(string id);
        IList<Jc_Ll_MModel> GetEmissionMonthList(int pageIndex, int pageSize, out int rowCount);
        Jc_Ll_MModel GetEmissionMonthById(string id);
    }
}
using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmissionHourRepository : IRepository<Jc_Ll_HModel>
    {
        Jc_Ll_HModel AddEmissionHour(Jc_Ll_HModel jc_Ll_HModel);
        void UpdateEmissionHour(Jc_Ll_HModel jc_Ll_HModel);
        void DeleteEmissionHour(string id);
        IList<Jc_Ll_HModel> GetEmissionHourList(int pageIndex, int pageSize, out int rowCount);
        Jc_Ll_HModel GetEmissionHourById(string id);
    }
}
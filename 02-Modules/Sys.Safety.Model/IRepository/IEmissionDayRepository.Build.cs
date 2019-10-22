using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmissionDayRepository : IRepository<Jc_Ll_DModel>
    {
        Jc_Ll_DModel AddEmissionDay(Jc_Ll_DModel jc_Ll_DModel);
        void UpdateEmissionDay(Jc_Ll_DModel jc_Ll_DModel);
        void DeleteEmissionDay(string id);
        IList<Jc_Ll_DModel> GetEmissionDayList(int pageIndex, int pageSize, out int rowCount);
        Jc_Ll_DModel GetEmissionDayById(string id);
    }
}
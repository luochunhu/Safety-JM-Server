using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAccumulationDayRepository : IRepository<Jc_Ll_DModel>
    {
        Jc_Ll_DModel AddJc_ll_d(Jc_Ll_DModel Jc_Ll_DModel);
        void UpdateJc_ll_d(Jc_Ll_DModel Jc_Ll_DModel);
        void DeleteJc_ll_d(string id);
        IList<Jc_Ll_DModel> GetJc_ll_dList(int pageIndex, int pageSize, out int rowCount);
        Jc_Ll_DModel GetJc_ll_dById(string id);

        bool IsAccumulationExists(string pointID, DateTime timer);
    }
}

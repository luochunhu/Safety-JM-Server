using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAccumulationMonthRepository : IRepository<Jc_Ll_MModel>
    {
        Jc_Ll_MModel AddJc_ll_m(Jc_Ll_MModel Jc_Ll_MModel);
        void UpdateJc_ll_m(Jc_Ll_MModel Jc_Ll_MModel);
        void DeleteJc_ll_m(string id);
        IList<Jc_Ll_MModel> GetJc_ll_mList(int pageIndex, int pageSize, out int rowCount);
        Jc_Ll_MModel GetJc_ll_mById(string id);

        bool IsAccumulationExists(string pointID, DateTime timer);
    }
}

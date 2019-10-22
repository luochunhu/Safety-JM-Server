using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAccumulationHourRepository : IRepository<Jc_Ll_HModel>
    {
        Jc_Ll_HModel AddJc_ll_h(Jc_Ll_HModel Jc_Ll_HModel);
        void UpdateJc_ll_h(Jc_Ll_HModel Jc_Ll_HModel);
        void DeleteJc_ll_h(string id);
        IList<Jc_Ll_HModel> GetJc_ll_hList(int pageIndex, int pageSize, out int rowCount);
        Jc_Ll_HModel GetJc_ll_hById(string id);

        bool IsAccumulationExists(string pointID, DateTime timer);
    }
}

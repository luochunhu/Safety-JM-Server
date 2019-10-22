using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAccumulationYearRepository : IRepository<Jc_Ll_YModel>
    {
        Jc_Ll_YModel AddJc_ll_y(Jc_Ll_YModel Jc_Ll_YModel);
        void UpdateJc_ll_y(Jc_Ll_YModel Jc_Ll_YModel);
        void DeleteJc_ll_y(string id);
        IList<Jc_Ll_YModel> GetJc_ll_yList(int pageIndex, int pageSize, out int rowCount);
        Jc_Ll_YModel GetJc_ll_yById(string id);

        bool IsAccumulationExists(string pointID, DateTime timer);
    }
}

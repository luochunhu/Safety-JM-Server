using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AccumulationYearRepository : RepositoryBase<Jc_Ll_YModel>, IAccumulationYearRepository
    {

        public Jc_Ll_YModel AddJc_ll_y(Jc_Ll_YModel Jc_Ll_YModel)
        {
            return base.Insert(Jc_Ll_YModel);
        }
        public void UpdateJc_ll_y(Jc_Ll_YModel Jc_Ll_YModel)
        {
            base.Update(Jc_Ll_YModel);
        }
        public void DeleteJc_ll_y(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_Ll_YModel> GetJc_ll_yList(int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = Datas.Count();
            return Datas.OrderBy(o=>o.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public Jc_Ll_YModel GetJc_ll_yById(string id)
        {
            Jc_Ll_YModel Jc_Ll_YModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return Jc_Ll_YModel;
        }

        public bool IsAccumulationExists(string pointID, DateTime timer)
        {
            Jc_Ll_YModel Jc_Ll_YModel = base.Datas.FirstOrDefault(c => c.PointID == pointID && c.Timer == timer);
            return Jc_Ll_YModel != null;
        }
    }
}

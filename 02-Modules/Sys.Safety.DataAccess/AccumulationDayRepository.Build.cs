using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AccumulationDayRepository : RepositoryBase<Jc_Ll_DModel>, IAccumulationDayRepository
    {

        public Jc_Ll_DModel AddJc_ll_d(Jc_Ll_DModel Jc_Ll_DModel)
        {
            return base.Insert(Jc_Ll_DModel);
        }
        public void UpdateJc_ll_d(Jc_Ll_DModel Jc_Ll_DModel)
        {
            base.Update(Jc_Ll_DModel);
        }
        public void DeleteJc_ll_d(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_Ll_DModel> GetJc_ll_dList(int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = Datas.Count();
            return Datas.OrderBy(o=>o.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public Jc_Ll_DModel GetJc_ll_dById(string id)
        {
            Jc_Ll_DModel Jc_Ll_DModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return Jc_Ll_DModel;
        }

        public bool IsAccumulationExists(string pointID, DateTime timer)
        {
            Jc_Ll_DModel Jc_Ll_DModel = base.Datas.FirstOrDefault(c => c.PointID == pointID && c.Timer == timer);
            return Jc_Ll_DModel != null;
        }
    }
}

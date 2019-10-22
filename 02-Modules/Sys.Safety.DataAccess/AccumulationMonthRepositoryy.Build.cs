using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AccumulationMonthRepository : RepositoryBase<Jc_Ll_MModel>, IAccumulationMonthRepository
    {

        public Jc_Ll_MModel AddJc_ll_m(Jc_Ll_MModel Jc_Ll_MModel)
        {
            return base.Insert(Jc_Ll_MModel);
        }
        public void UpdateJc_ll_m(Jc_Ll_MModel Jc_Ll_MModel)
        {
            base.Update(Jc_Ll_MModel);
        }
        public void DeleteJc_ll_m(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_Ll_MModel> GetJc_ll_mList(int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = Datas.Count();
            return Datas.OrderBy(o=>o.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public Jc_Ll_MModel GetJc_ll_mById(string id)
        {
            Jc_Ll_MModel Jc_Ll_MModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return Jc_Ll_MModel;
        }

        public bool IsAccumulationExists(string pointID, DateTime timer)
        {
            Jc_Ll_MModel Jc_Ll_MModel = base.Datas.FirstOrDefault(c => c.PointID == pointID && c.Timer == timer);
            return Jc_Ll_MModel != null;
        }
    }
}

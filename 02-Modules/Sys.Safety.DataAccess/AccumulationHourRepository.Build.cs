using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AccumulationHourRepository : RepositoryBase<Jc_Ll_HModel>, IAccumulationHourRepository
    {

        public Jc_Ll_HModel AddJc_ll_h(Jc_Ll_HModel Jc_Ll_HModel)
        {
            return base.Insert(Jc_Ll_HModel);
        }
        public void UpdateJc_ll_h(Jc_Ll_HModel Jc_Ll_HModel)
        {
            base.Update(Jc_Ll_HModel);
        }
        public void DeleteJc_ll_h(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_Ll_HModel> GetJc_ll_hList(int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = Datas.Count();
            return Datas.OrderBy(o=>o.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public Jc_Ll_HModel GetJc_ll_hById(string id)
        {
            Jc_Ll_HModel Jc_Ll_HModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return Jc_Ll_HModel;
        }

        public bool IsAccumulationExists(string pointID, DateTime timer)
        {
            Jc_Ll_HModel Jc_Ll_HModel = base.Datas.FirstOrDefault(c => c.PointID == pointID && c.Timer == timer);
            return Jc_Ll_HModel != null;
        }
    }
}

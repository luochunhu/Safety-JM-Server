using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class EmissionMonthRepository : RepositoryBase<Jc_Ll_MModel>, IEmissionMonthRepository
    {

                public Jc_Ll_MModel AddEmissionMonth(Jc_Ll_MModel jc_Ll_MModel)
		{
		   return base.Insert(jc_Ll_MModel);
		}
		        public void UpdateEmissionMonth(Jc_Ll_MModel jc_Ll_MModel)
		{
		   base.Update(jc_Ll_MModel);
		}
	            public void DeleteEmissionMonth(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_Ll_MModel> GetEmissionMonthList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_Ll_MModelLists = base.Datas.ToList();
		   rowCount = jc_Ll_MModelLists.Count();
           return jc_Ll_MModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_Ll_MModel GetEmissionMonthById(string id)
		{
		    Jc_Ll_MModel jc_Ll_MModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_Ll_MModel;
		}
    }
}

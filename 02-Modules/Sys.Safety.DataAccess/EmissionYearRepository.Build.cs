using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class EmissionYearRepository : RepositoryBase<Jc_Ll_YModel>, IEmissionYearRepository
    {

                public Jc_Ll_YModel AddEmissionYear(Jc_Ll_YModel jc_Ll_YModel)
		{
		   return base.Insert(jc_Ll_YModel);
		}
		        public void UpdateEmissionYear(Jc_Ll_YModel jc_Ll_YModel)
		{
		   base.Update(jc_Ll_YModel);
		}
	            public void DeleteEmissionYear(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_Ll_YModel> GetEmissionYearList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_Ll_YModelLists = base.Datas.ToList();
		   rowCount = jc_Ll_YModelLists.Count();
           return jc_Ll_YModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_Ll_YModel GetEmissionYearById(string id)
		{
		    Jc_Ll_YModel jc_Ll_YModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_Ll_YModel;
		}
    }
}

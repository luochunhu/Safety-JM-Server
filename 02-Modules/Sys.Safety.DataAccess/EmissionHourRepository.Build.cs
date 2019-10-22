using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class EmissionHourRepository : RepositoryBase<Jc_Ll_HModel>, IEmissionHourRepository
    {

                public Jc_Ll_HModel AddEmissionHour(Jc_Ll_HModel jc_Ll_HModel)
		{
		   return base.Insert(jc_Ll_HModel);
		}
		        public void UpdateEmissionHour(Jc_Ll_HModel jc_Ll_HModel)
		{
		   base.Update(jc_Ll_HModel);
		}
	            public void DeleteEmissionHour(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_Ll_HModel> GetEmissionHourList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_Ll_HModelLists = base.Datas.ToList();
		   rowCount = jc_Ll_HModelLists.Count();
           return jc_Ll_HModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_Ll_HModel GetEmissionHourById(string id)
		{
		    Jc_Ll_HModel jc_Ll_HModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_Ll_HModel;
		}
    }
}

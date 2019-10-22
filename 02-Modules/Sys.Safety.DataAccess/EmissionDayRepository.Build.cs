using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class EmissionDayRepository : RepositoryBase<Jc_Ll_DModel>, IEmissionDayRepository
    {

                public Jc_Ll_DModel AddEmissionDay(Jc_Ll_DModel jc_Ll_DModel)
		{
		   return base.Insert(jc_Ll_DModel);
		}
		        public void UpdateEmissionDay(Jc_Ll_DModel jc_Ll_DModel)
		{
		   base.Update(jc_Ll_DModel);
		}
	            public void DeleteEmissionDay(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_Ll_DModel> GetEmissionDayList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_Ll_DModelLists = base.Datas.ToList();
		   rowCount = jc_Ll_DModelLists.Count();
           return jc_Ll_DModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_Ll_DModel GetEmissionDayById(string id)
		{
		    Jc_Ll_DModel jc_Ll_DModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_Ll_DModel;
		}
    }
}

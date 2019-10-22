using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_YearRepository:RepositoryBase<Jc_YearModel>,IJc_YearRepository
    {

                public Jc_YearModel AddJc_Year(Jc_YearModel jc_YearModel)
		{
		   return base.Insert(jc_YearModel);
		}
		        public void UpdateJc_Year(Jc_YearModel jc_YearModel)
		{
		   base.Update(jc_YearModel);
		}
	            public void DeleteJc_Year(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_YearModel> GetJc_YearList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_YearModelLists = base.Datas.ToList();
		   rowCount = jc_YearModelLists.Count();
           return jc_YearModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_YearModel GetJc_YearById(string id)
		{
		    Jc_YearModel jc_YearModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_YearModel;
		}
    }
}

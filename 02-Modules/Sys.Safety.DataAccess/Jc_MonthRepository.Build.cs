using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_MonthRepository:RepositoryBase<Jc_MonthModel>,IJc_MonthRepository
    {

                public Jc_MonthModel AddJc_Month(Jc_MonthModel jc_MonthModel)
		{
		   return base.Insert(jc_MonthModel);
		}
		        public void UpdateJc_Month(Jc_MonthModel jc_MonthModel)
		{
		   base.Update(jc_MonthModel);
		}
	            public void DeleteJc_Month(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_MonthModel> GetJc_MonthList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_MonthModelLists = base.Datas.ToList();
		   rowCount = jc_MonthModelLists.Count();
           return jc_MonthModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_MonthModel GetJc_MonthById(string id)
		{
		    Jc_MonthModel jc_MonthModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_MonthModel;
		}
    }
}

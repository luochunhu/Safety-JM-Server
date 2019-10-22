using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_DayRepository:RepositoryBase<Jc_DayModel>,IJc_DayRepository
    {

                public Jc_DayModel AddJc_Day(Jc_DayModel jc_DayModel)
		{
		   return base.Insert(jc_DayModel);
		}
		        public void UpdateJc_Day(Jc_DayModel jc_DayModel)
		{
		   base.Update(jc_DayModel);
		}
	            public void DeleteJc_Day(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_DayModel> GetJc_DayList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_DayModelLists = base.Datas.ToList();
		   rowCount = jc_DayModelLists.Count();
           return jc_DayModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_DayModel GetJc_DayById(string id)
		{
		    Jc_DayModel jc_DayModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_DayModel;
		}
    }
}

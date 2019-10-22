using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_RRepository:RepositoryBase<Jc_RModel>,IJc_RRepository
    {

                public Jc_RModel AddJc_R(Jc_RModel jc_RModel)
		{
		   return base.Insert(jc_RModel);
		}
		        public void UpdateJc_R(Jc_RModel jc_RModel)
		{
		   base.Update(jc_RModel);
		}
	            public void DeleteJc_R(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_RModel> GetJc_RList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_RModelLists = base.Datas.ToList();
		   rowCount = jc_RModelLists.Count();
           return jc_RModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_RModel GetJc_RById(string id)
		{
		    Jc_RModel jc_RModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_RModel;
		}
    }
}

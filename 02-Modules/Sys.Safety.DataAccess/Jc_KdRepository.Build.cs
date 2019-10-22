using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_KdRepository:RepositoryBase<Jc_KdModel>,IJc_KdRepository
    {

                public Jc_KdModel AddJc_Kd(Jc_KdModel jc_KdModel)
		{
		   return base.Insert(jc_KdModel);
		}
		        public void UpdateJc_Kd(Jc_KdModel jc_KdModel)
		{
		   base.Update(jc_KdModel);
		}
	            public void DeleteJc_Kd(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_KdModel> GetJc_KdList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_KdModelLists = base.Datas.ToList();
		   rowCount = jc_KdModelLists.Count();
           return jc_KdModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_KdModel GetJc_KdById(string id)
		{
		    Jc_KdModel jc_KdModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_KdModel;
		}
    }
}

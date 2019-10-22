using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_PhjRepository:RepositoryBase<R_PhjModel>,IR_PhjRepository
    {

                public R_PhjModel AddPhj(R_PhjModel phjModel)
		{
		   return base.Insert(phjModel);
		}
		        public void UpdatePhj(R_PhjModel phjModel)
		{
		   base.Update(phjModel);
		}
	            public void DeletePhj(string id)
		{
		   base.Delete(id);
		}
		        public IList<R_PhjModel> GetPhjList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  phjModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return phjModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public R_PhjModel GetPhjById(string id)
		{
		    R_PhjModel phjModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return phjModel;
		}
    }
}

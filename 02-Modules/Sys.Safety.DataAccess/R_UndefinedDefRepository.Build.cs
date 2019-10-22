using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_UndefinedDefRepository : RepositoryBase<R_UndefinedDefModel>, IR_UndefinedDefRepository
    {

                public R_UndefinedDefModel AddUndefinedDef(R_UndefinedDefModel undefinedDefModel)
		{
		   return base.Insert(undefinedDefModel);
		}
		        public void UpdateUndefinedDef(R_UndefinedDefModel undefinedDefModel)
		{
		   base.Update(undefinedDefModel);
		}
	            public void DeleteUndefinedDef(string id)
		{
		   base.Delete(id);
		}
		        public IList<R_UndefinedDefModel> GetUndefinedDefList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  undefinedDefModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return undefinedDefModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public R_UndefinedDefModel GetUndefinedDefById(string id)
		{
		    R_UndefinedDefModel undefinedDefModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return undefinedDefModel;
		}
    }
}

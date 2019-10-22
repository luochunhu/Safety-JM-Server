using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_CallhistorypointlistRepository:RepositoryBase<B_CallhistorypointlistModel>,IB_CallhistorypointlistRepository
    {

                public B_CallhistorypointlistModel AddB_Callhistorypointlist(B_CallhistorypointlistModel b_CallhistorypointlistModel)
		{
		   return base.Insert(b_CallhistorypointlistModel);
		}
		        public void UpdateB_Callhistorypointlist(B_CallhistorypointlistModel b_CallhistorypointlistModel)
		{
		   base.Update(b_CallhistorypointlistModel);
		}
	            public void DeleteB_Callhistorypointlist(string id)
		{
		   base.Delete(id);
		}
		        public IList<B_CallhistorypointlistModel> GetB_CallhistorypointlistList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  b_CallhistorypointlistModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return b_CallhistorypointlistModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public B_CallhistorypointlistModel GetB_CallhistorypointlistById(string id)
		{
		    B_CallhistorypointlistModel b_CallhistorypointlistModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return b_CallhistorypointlistModel;
		}
    }
}

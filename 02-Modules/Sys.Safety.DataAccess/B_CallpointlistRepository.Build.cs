using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_CallpointlistRepository:RepositoryBase<B_CallpointlistModel>,IB_CallpointlistRepository
    {

                public B_CallpointlistModel AddB_Callpointlist(B_CallpointlistModel b_CallpointlistModel)
		{
		   return base.Insert(b_CallpointlistModel);
		}
		        public void UpdateB_Callpointlist(B_CallpointlistModel b_CallpointlistModel)
		{
		   base.Update(b_CallpointlistModel);
		}
	            public void DeleteB_Callpointlist(string id)
		{
		   base.Delete(id);
		}
		        public IList<B_CallpointlistModel> GetB_CallpointlistList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  b_CallpointlistModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return b_CallpointlistModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public B_CallpointlistModel GetB_CallpointlistById(string id)
		{
		    B_CallpointlistModel b_CallpointlistModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return b_CallpointlistModel;
		}
    }
}

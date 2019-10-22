using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_BroadcastplanpointlistRepository:RepositoryBase<B_BroadcastplanpointlistModel>,IB_BroadcastplanpointlistRepository
    {

                public B_BroadcastplanpointlistModel AddB_Broadcastplanpointlist(B_BroadcastplanpointlistModel b_BroadcastplanpointlistModel)
		{
		   return base.Insert(b_BroadcastplanpointlistModel);
		}
		        public void UpdateB_Broadcastplanpointlist(B_BroadcastplanpointlistModel b_BroadcastplanpointlistModel)
		{
		   base.Update(b_BroadcastplanpointlistModel);
		}
	            public void DeleteB_Broadcastplanpointlist(string id)
		{
		   base.Delete(id);
		}
		        public IList<B_BroadcastplanpointlistModel> GetB_BroadcastplanpointlistList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  b_BroadcastplanpointlistModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return b_BroadcastplanpointlistModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public B_BroadcastplanpointlistModel GetB_BroadcastplanpointlistById(string id)
		{
		    B_BroadcastplanpointlistModel b_BroadcastplanpointlistModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return b_BroadcastplanpointlistModel;
		}
    }
}

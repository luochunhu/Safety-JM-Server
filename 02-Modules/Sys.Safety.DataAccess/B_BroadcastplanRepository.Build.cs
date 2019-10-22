using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_BroadcastplanRepository:RepositoryBase<B_BroadcastplanModel>,IB_BroadcastplanRepository
    {

                public B_BroadcastplanModel AddB_Broadcastplan(B_BroadcastplanModel b_BroadcastplanModel)
		{
		   return base.Insert(b_BroadcastplanModel);
		}
		        public void UpdateB_Broadcastplan(B_BroadcastplanModel b_BroadcastplanModel)
		{
		   base.Update(b_BroadcastplanModel);
		}
	            public void DeleteB_Broadcastplan(string id)
		{
		   base.Delete(id);
		}
		        public IList<B_BroadcastplanModel> GetB_BroadcastplanList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  b_BroadcastplanModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return b_BroadcastplanModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public B_BroadcastplanModel GetB_BroadcastplanById(string id)
		{
		    B_BroadcastplanModel b_BroadcastplanModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return b_BroadcastplanModel;
		}
    }
}

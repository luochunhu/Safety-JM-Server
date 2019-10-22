using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_BroadcastplanRepository : IRepository<B_BroadcastplanModel>
    {
                B_BroadcastplanModel AddB_Broadcastplan(B_BroadcastplanModel b_BroadcastplanModel);
		        void UpdateB_Broadcastplan(B_BroadcastplanModel b_BroadcastplanModel);
	            void DeleteB_Broadcastplan(string id);
		        IList<B_BroadcastplanModel> GetB_BroadcastplanList(int pageIndex, int pageSize, out int rowCount);
				B_BroadcastplanModel GetB_BroadcastplanById(string id);
    }
}

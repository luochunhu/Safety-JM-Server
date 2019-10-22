using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_BroadcastplanpointlistRepository : IRepository<B_BroadcastplanpointlistModel>
    {
                B_BroadcastplanpointlistModel AddB_Broadcastplanpointlist(B_BroadcastplanpointlistModel b_BroadcastplanpointlistModel);
		        void UpdateB_Broadcastplanpointlist(B_BroadcastplanpointlistModel b_BroadcastplanpointlistModel);
	            void DeleteB_Broadcastplanpointlist(string id);
		        IList<B_BroadcastplanpointlistModel> GetB_BroadcastplanpointlistList(int pageIndex, int pageSize, out int rowCount);
				B_BroadcastplanpointlistModel GetB_BroadcastplanpointlistById(string id);
    }
}

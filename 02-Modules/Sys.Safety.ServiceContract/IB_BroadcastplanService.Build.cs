using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Broadcastplan;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_BroadcastplanService
    {  
	            BasicResponse<B_BroadcastplanInfo> AddB_Broadcastplan(B_BroadcastplanAddRequest b_BroadcastplanRequest);		
		        BasicResponse<B_BroadcastplanInfo> UpdateB_Broadcastplan(B_BroadcastplanUpdateRequest b_BroadcastplanRequest);	 
		        BasicResponse DeleteB_Broadcastplan(B_BroadcastplanDeleteRequest b_BroadcastplanRequest);
		        BasicResponse<List<B_BroadcastplanInfo>> GetB_BroadcastplanList(B_BroadcastplanGetListRequest b_BroadcastplanRequest);
		         BasicResponse<B_BroadcastplanInfo> GetB_BroadcastplanById(B_BroadcastplanGetRequest b_BroadcastplanRequest);	
    }
}


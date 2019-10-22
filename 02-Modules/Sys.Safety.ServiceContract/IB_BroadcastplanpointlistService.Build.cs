using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Broadcastplanpointlist;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_BroadcastplanpointlistService
    {  
	            BasicResponse<B_BroadcastplanpointlistInfo> AddB_Broadcastplanpointlist(B_BroadcastplanpointlistAddRequest b_BroadcastplanpointlistRequest);		
		        BasicResponse<B_BroadcastplanpointlistInfo> UpdateB_Broadcastplanpointlist(B_BroadcastplanpointlistUpdateRequest b_BroadcastplanpointlistRequest);	 
		        BasicResponse DeleteB_Broadcastplanpointlist(B_BroadcastplanpointlistDeleteRequest b_BroadcastplanpointlistRequest);
		        BasicResponse<List<B_BroadcastplanpointlistInfo>> GetB_BroadcastplanpointlistList(B_BroadcastplanpointlistGetListRequest b_BroadcastplanpointlistRequest);
		         BasicResponse<B_BroadcastplanpointlistInfo> GetB_BroadcastplanpointlistById(B_BroadcastplanpointlistGetRequest b_BroadcastplanpointlistRequest);	
    }
}


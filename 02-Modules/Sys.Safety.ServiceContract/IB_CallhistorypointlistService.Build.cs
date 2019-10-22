using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Callhistorypointlist;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_CallhistorypointlistService
    {  
	            BasicResponse<B_CallhistorypointlistInfo> AddB_Callhistorypointlist(B_CallhistorypointlistAddRequest b_CallhistorypointlistRequest);		
		        BasicResponse<B_CallhistorypointlistInfo> UpdateB_Callhistorypointlist(B_CallhistorypointlistUpdateRequest b_CallhistorypointlistRequest);	 
		        BasicResponse DeleteB_Callhistorypointlist(B_CallhistorypointlistDeleteRequest b_CallhistorypointlistRequest);
		        BasicResponse<List<B_CallhistorypointlistInfo>> GetB_CallhistorypointlistList(B_CallhistorypointlistGetListRequest b_CallhistorypointlistRequest);
		         BasicResponse<B_CallhistorypointlistInfo> GetB_CallhistorypointlistById(B_CallhistorypointlistGetRequest b_CallhistorypointlistRequest);	
    }
}


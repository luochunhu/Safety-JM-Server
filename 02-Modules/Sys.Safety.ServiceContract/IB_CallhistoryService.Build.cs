using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Callhistory;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_CallhistoryService
    {  
	            BasicResponse<B_CallhistoryInfo> AddB_Callhistory(B_CallhistoryAddRequest b_CallhistoryRequest);		
		        BasicResponse<B_CallhistoryInfo> UpdateB_Callhistory(B_CallhistoryUpdateRequest b_CallhistoryRequest);	 
		        BasicResponse DeleteB_Callhistory(B_CallhistoryDeleteRequest b_CallhistoryRequest);
		        BasicResponse<List<B_CallhistoryInfo>> GetB_CallhistoryList(B_CallhistoryGetListRequest b_CallhistoryRequest);
		         BasicResponse<B_CallhistoryInfo> GetB_CallhistoryById(B_CallhistoryGetRequest b_CallhistoryRequest);	
    }
}


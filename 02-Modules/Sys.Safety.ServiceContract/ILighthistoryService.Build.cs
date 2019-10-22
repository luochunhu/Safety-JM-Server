using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Lighthistory;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface ILighthistoryService
    {  
	            BasicResponse<LighthistoryInfo> AddLighthistory(LighthistoryAddRequest lighthistoryrequest);		
		        BasicResponse<LighthistoryInfo> UpdateLighthistory(LighthistoryUpdateRequest lighthistoryrequest);	 
		        BasicResponse DeleteLighthistory(LighthistoryDeleteRequest lighthistoryrequest);
		        BasicResponse<List<LighthistoryInfo>> GetLighthistoryList(LighthistoryGetListRequest lighthistoryrequest);
		         BasicResponse<LighthistoryInfo> GetLighthistoryById(LighthistoryGetRequest lighthistoryrequest);	
    }
}


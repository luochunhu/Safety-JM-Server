using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Webauthority;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IWebauthorityService
    {  
	            BasicResponse<WebauthorityInfo> AddWebauthority(WebauthorityAddRequest webauthorityrequest);		
		        BasicResponse<WebauthorityInfo> UpdateWebauthority(WebauthorityUpdateRequest webauthorityrequest);	 
		        BasicResponse DeleteWebauthority(WebauthorityDeleteRequest webauthorityrequest);
		        BasicResponse<List<WebauthorityInfo>> GetWebauthorityList(WebauthorityGetListRequest webauthorityrequest);
		         BasicResponse<WebauthorityInfo> GetWebauthorityById(WebauthorityGetRequest webauthorityrequest);	
    }
}


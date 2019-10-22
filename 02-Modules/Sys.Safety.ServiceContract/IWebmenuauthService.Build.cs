using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Webmenuauth;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IWebmenuauthService
    {  
	            BasicResponse<WebmenuauthInfo> AddWebmenuauth(WebmenuauthAddRequest webmenuauthrequest);		
		        BasicResponse<WebmenuauthInfo> UpdateWebmenuauth(WebmenuauthUpdateRequest webmenuauthrequest);	 
		        BasicResponse DeleteWebmenuauth(WebmenuauthDeleteRequest webmenuauthrequest);
		        BasicResponse<List<WebmenuauthInfo>> GetWebmenuauthList(WebmenuauthGetListRequest webmenuauthrequest);
		         BasicResponse<WebmenuauthInfo> GetWebmenuauthById(WebmenuauthGetRequest webmenuauthrequest);	
    }
}


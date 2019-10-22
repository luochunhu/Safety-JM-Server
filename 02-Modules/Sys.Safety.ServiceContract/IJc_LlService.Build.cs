using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Ll;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_LlService
    {  
	            BasicResponse<Jc_LlInfo> AddJc_Ll(Jc_LlAddRequest jc_Llrequest);		
		        BasicResponse<Jc_LlInfo> UpdateJc_Ll(Jc_LlUpdateRequest jc_Llrequest);	 
		        BasicResponse DeleteJc_Ll(Jc_LlDeleteRequest jc_Llrequest);
		        BasicResponse<List<Jc_LlInfo>> GetJc_LlList(Jc_LlGetListRequest jc_Llrequest);
		         BasicResponse<Jc_LlInfo> GetJc_LlById(Jc_LlGetRequest jc_Llrequest);	
    }
}


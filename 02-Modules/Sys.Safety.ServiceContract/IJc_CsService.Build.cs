using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Cs;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_CsService
    {  
	            BasicResponse<Jc_CsInfo> AddJc_Cs(Jc_CsAddRequest jc_Csrequest);		
		        BasicResponse<Jc_CsInfo> UpdateJc_Cs(Jc_CsUpdateRequest jc_Csrequest);	 
		        BasicResponse DeleteJc_Cs(Jc_CsDeleteRequest jc_Csrequest);
		        BasicResponse<List<Jc_CsInfo>> GetJc_CsList(Jc_CsGetListRequest jc_Csrequest);
		         BasicResponse<Jc_CsInfo> GetJc_CsById(Jc_CsGetRequest jc_Csrequest);	
    }
}


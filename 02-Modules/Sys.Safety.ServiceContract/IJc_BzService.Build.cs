using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Bz;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_BzService
    {  
	            BasicResponse<Jc_BzInfo> AddJc_Bz(Jc_BzAddRequest jc_Bzrequest);		
		        BasicResponse<Jc_BzInfo> UpdateJc_Bz(Jc_BzUpdateRequest jc_Bzrequest);	 
		        BasicResponse DeleteJc_Bz(Jc_BzDeleteRequest jc_Bzrequest);
		        BasicResponse<List<Jc_BzInfo>> GetJc_BzList(Jc_BzGetListRequest jc_Bzrequest);
		         BasicResponse<Jc_BzInfo> GetJc_BzById(Jc_BzGetRequest jc_Bzrequest);	
    }
}


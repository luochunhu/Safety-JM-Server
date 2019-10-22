using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Year;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_YearService
    {  
	            BasicResponse<Jc_YearInfo> AddJc_Year(Jc_YearAddRequest jc_Yearrequest);		
		        BasicResponse<Jc_YearInfo> UpdateJc_Year(Jc_YearUpdateRequest jc_Yearrequest);	 
		        BasicResponse DeleteJc_Year(Jc_YearDeleteRequest jc_Yearrequest);
		        BasicResponse<List<Jc_YearInfo>> GetJc_YearList(Jc_YearGetListRequest jc_Yearrequest);
		         BasicResponse<Jc_YearInfo> GetJc_YearById(Jc_YearGetRequest jc_Yearrequest);	
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Season;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_SeasonService
    {  
	            BasicResponse<Jc_SeasonInfo> AddJc_Season(Jc_SeasonAddRequest jc_Seasonrequest);		
		        BasicResponse<Jc_SeasonInfo> UpdateJc_Season(Jc_SeasonUpdateRequest jc_Seasonrequest);	 
		        BasicResponse DeleteJc_Season(Jc_SeasonDeleteRequest jc_Seasonrequest);
		        BasicResponse<List<Jc_SeasonInfo>> GetJc_SeasonList(Jc_SeasonGetListRequest jc_Seasonrequest);
		         BasicResponse<Jc_SeasonInfo> GetJc_SeasonById(Jc_SeasonGetRequest jc_Seasonrequest);	
    }
}


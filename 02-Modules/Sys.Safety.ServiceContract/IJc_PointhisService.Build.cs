using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Pointhis;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_PointhisService
    {  
	            BasicResponse<Jc_PointhisInfo> AddJc_Pointhis(Jc_PointhisAddRequest jc_Pointhisrequest);		
		        BasicResponse<Jc_PointhisInfo> UpdateJc_Pointhis(Jc_PointhisUpdateRequest jc_Pointhisrequest);	 
		        BasicResponse DeleteJc_Pointhis(Jc_PointhisDeleteRequest jc_Pointhisrequest);
		        BasicResponse<List<Jc_PointhisInfo>> GetJc_PointhisList(Jc_PointhisGetListRequest jc_Pointhisrequest);
		         BasicResponse<Jc_PointhisInfo> GetJc_PointhisById(Jc_PointhisGetRequest jc_Pointhisrequest);	
    }
}


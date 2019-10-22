using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_R;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_RService
    {  
	            BasicResponse<Jc_RInfo> AddJc_R(Jc_RAddRequest jc_Rrequest);		
		        BasicResponse<Jc_RInfo> UpdateJc_R(Jc_RUpdateRequest jc_Rrequest);	 
		        BasicResponse DeleteJc_R(Jc_RDeleteRequest jc_Rrequest);
		        BasicResponse<List<Jc_RInfo>> GetJc_RList(Jc_RGetListRequest jc_Rrequest);
		         BasicResponse<Jc_RInfo> GetJc_RById(Jc_RGetRequest jc_Rrequest);
                 BasicResponse<Jc_RInfo> GetJc_RByDataAndId(Jc_RGetByDateAndIdRequest jc_Rrequest);
    }
}


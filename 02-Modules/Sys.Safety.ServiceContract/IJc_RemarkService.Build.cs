using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Remark;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_RemarkService
    {  
	            BasicResponse<Jc_RemarkInfo> AddJc_Remark(Jc_RemarkAddRequest jc_Remarkrequest);		
		        BasicResponse<Jc_RemarkInfo> UpdateJc_Remark(Jc_RemarkUpdateRequest jc_Remarkrequest);	 
		        BasicResponse DeleteJc_Remark(Jc_RemarkDeleteRequest jc_Remarkrequest);
		        BasicResponse<List<Jc_RemarkInfo>> GetJc_RemarkList(Jc_RemarkGetListRequest jc_Remarkrequest);
		         BasicResponse<Jc_RemarkInfo> GetJc_RemarkById(Jc_RemarkGetRequest jc_Remarkrequest);	
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Flag;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IFlagService
    {  
	            BasicResponse<FlagInfo> AddFlag(FlagAddRequest flagrequest);		
		        BasicResponse<FlagInfo> UpdateFlag(FlagUpdateRequest flagrequest);	 
		        BasicResponse DeleteFlag(FlagDeleteRequest flagrequest);
		        BasicResponse<List<FlagInfo>> GetFlagList(FlagGetListRequest flagrequest);
		         BasicResponse<FlagInfo> GetFlagById(FlagGetRequest flagrequest);	
    }
}


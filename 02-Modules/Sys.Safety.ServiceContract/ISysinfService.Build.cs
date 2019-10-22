using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Sysinf;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface ISysinfService
    {  
	            BasicResponse<SysinfInfo> AddSysinf(SysinfAddRequest sysinfrequest);		
		        BasicResponse<SysinfInfo> UpdateSysinf(SysinfUpdateRequest sysinfrequest);	 
		        BasicResponse DeleteSysinf(SysinfDeleteRequest sysinfrequest);
		        BasicResponse<List<SysinfInfo>> GetSysinfList(SysinfGetListRequest sysinfrequest);
		         BasicResponse<SysinfInfo> GetSysinfById(SysinfGetRequest sysinfrequest);	
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Deptinf;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IDeptinfService
    {  
	            BasicResponse<DeptinfInfo> AddDeptinf(DeptinfAddRequest deptinfrequest);		
		        BasicResponse<DeptinfInfo> UpdateDeptinf(DeptinfUpdateRequest deptinfrequest);	 
		        BasicResponse DeleteDeptinf(DeptinfDeleteRequest deptinfrequest);
		        BasicResponse<List<DeptinfInfo>> GetDeptinfList(DeptinfGetListRequest deptinfrequest);
		         BasicResponse<DeptinfInfo> GetDeptinfById(DeptinfGetRequest deptinfrequest);	
    }
}


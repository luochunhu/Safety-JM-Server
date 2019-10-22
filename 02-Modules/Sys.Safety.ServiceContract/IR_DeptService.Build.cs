using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Dept;

namespace Sys.Safety.ServiceContract
{
    public interface IR_DeptService
    {
        BasicResponse<R_DeptInfo> AddDept(R_DeptAddRequest deptRequest);
        BasicResponse<R_DeptInfo> UpdateDept(R_DeptUpdateRequest deptRequest);
        BasicResponse DeleteDept(R_DeptDeleteRequest deptRequest);
        BasicResponse<List<R_DeptInfo>> GetDeptList(R_DeptGetListRequest deptRequest);
        BasicResponse<R_DeptInfo> GetDeptById(R_DeptGetRequest deptRequest);

        BasicResponse<List<R_DeptInfo>> GetAllDept(BasicRequest deptRequest);
    }
}


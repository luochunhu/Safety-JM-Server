using Basic.Framework.Service;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class R_DeptController : BasicApiController, IR_DeptService
    {
        IR_DeptService _deptService = ServiceFactory.Create<IR_DeptService>();

        [HttpPost]
        [Route("v1/R_Dept/AddDept")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_DeptInfo> AddDept(Sys.Safety.Request.R_Dept.R_DeptAddRequest deptRequest)
        {
            return _deptService.AddDept(deptRequest);
        }

        [HttpPost]
        [Route("v1/R_Dept/UpdateDept")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_DeptInfo> UpdateDept(Sys.Safety.Request.R_Dept.R_DeptUpdateRequest deptRequest)
        {
            return _deptService.UpdateDept(deptRequest);
        }

        [HttpPost]
        [Route("v1/R_Dept/DeleteDept")]
        public Basic.Framework.Web.BasicResponse DeleteDept(Sys.Safety.Request.R_Dept.R_DeptDeleteRequest deptRequest)
        {
            return _deptService.DeleteDept(deptRequest);
        }

        [HttpPost]
        [Route("v1/R_Dept/GetDeptList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_DeptInfo>> GetDeptList(Sys.Safety.Request.R_Dept.R_DeptGetListRequest deptRequest)
        {
            return _deptService.GetDeptList(deptRequest);
        }

        [HttpPost]
        [Route("v1/R_Dept/GetDeptById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_DeptInfo> GetDeptById(Sys.Safety.Request.R_Dept.R_DeptGetRequest deptRequest)
        {
            return _deptService.GetDeptById(deptRequest);
        }

        [HttpPost]
        [Route("v1/R_Dept/GetAllDept")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_DeptInfo>> GetAllDept(BasicRequest deptRequest)
        {
            return _deptService.GetAllDept(deptRequest);
        }
    }
}

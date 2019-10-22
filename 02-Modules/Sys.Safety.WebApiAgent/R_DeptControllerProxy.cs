using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class R_DeptControllerProxy : BaseProxy, IR_DeptService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.R_DeptInfo> AddDept(Sys.Safety.Request.R_Dept.R_DeptAddRequest deptRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Dept/AddDept?token=" + Token, JSONHelper.ToJSONString(deptRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_DeptInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_DeptInfo> UpdateDept(Sys.Safety.Request.R_Dept.R_DeptUpdateRequest deptRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Dept/UpdateDept?token=" + Token, JSONHelper.ToJSONString(deptRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_DeptInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteDept(Sys.Safety.Request.R_Dept.R_DeptDeleteRequest deptRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Dept/DeleteDept?token=" + Token, JSONHelper.ToJSONString(deptRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_DeptInfo>> GetDeptList(Sys.Safety.Request.R_Dept.R_DeptGetListRequest deptRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Dept/GetDeptList?token=" + Token, JSONHelper.ToJSONString(deptRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.R_DeptInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_DeptInfo> GetDeptById(Sys.Safety.Request.R_Dept.R_DeptGetRequest deptRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Dept/GetDeptById?token=" + Token, JSONHelper.ToJSONString(deptRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_DeptInfo>>(responseStr);
        }


        public BasicResponse<List<R_DeptInfo>> GetAllDept(BasicRequest deptRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Dept/GetAllDept?token=" + Token, JSONHelper.ToJSONString(deptRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_DeptInfo>>>(responseStr);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApiAgent
{
    public class ClassControllerProxy : BaseProxy, IClassService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ClassInfo> AddClass(Sys.Safety.Request.Class.ClassAddRequest classrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/AddClass?token=" + Token, JSONHelper.ToJSONString(classrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ClassInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ClassInfo> UpdateClass(Sys.Safety.Request.Class.ClassUpdateRequest classrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/UpdateClass?token=" + Token, JSONHelper.ToJSONString(classrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ClassInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteClass(Sys.Safety.Request.Class.ClassDeleteRequest classrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/DeleteClass?token=" + Token, JSONHelper.ToJSONString(classrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ClassInfo>> GetClassList(Sys.Safety.Request.Class.ClassGetListRequest classrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/GetClassList?token=" + Token, JSONHelper.ToJSONString(classrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ClassInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ClassInfo> GetClassById(Sys.Safety.Request.Class.ClassGetRequest classrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/GetClassById?token=" + Token, JSONHelper.ToJSONString(classrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ClassInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveClassList(Sys.Safety.Request.Class.ClassListAddRequest list)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/SaveClassList?token=" + Token, JSONHelper.ToJSONString(list));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteClassByCode(Sys.Safety.Request.Class.ClassCodeRequest code)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/DeleteClassByCode?token=" + Token, JSONHelper.ToJSONString(code));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ClassInfo> GetClassDtoByCode(Sys.Safety.Request.Class.ClassCodeRequest code)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/GetClassDtoByCode?token=" + Token, JSONHelper.ToJSONString(code));
            return JSONHelper.ParseJSONString<BasicResponse<ClassInfo>>(responseStr);
        }

        public BasicResponse<ClassInfo> GetClassByStrName(Sys.Safety.Request.Class.GetClassByStrNameRequest classrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/GetClassByStrName?token=" + Token, JSONHelper.ToJSONString(classrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ClassInfo>>(responseStr);
        }

        public BasicResponse SaveClassByCondition(Sys.Safety.Request.Class.SaveClassByConditionRequest classrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Class/SaveClassByCondition?token=" + Token, JSONHelper.ToJSONString(classrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ClassInfo>>(responseStr);
        }


        public BasicResponse<List<ClassInfo>> GetAllClassList()
        {
            throw new NotImplementedException();
        }
    }
}

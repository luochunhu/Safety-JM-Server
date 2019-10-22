using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Login;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.UserRoleAuthorize
{
    public class LoginControllerProxy : BaseProxy, ILoginService
    {
        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <param name="loginrequest"></param>
        /// <returns></returns>       
        public BasicResponse<Dictionary<string, object>> Login(LoginRequest loginrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Login/Login?token=" + Token, JSONHelper.ToJSONString(loginrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Dictionary<string, object>>>(responseStr);
        }

        /// <summary>
        /// 注销
        /// </summary>        
        public BasicResponse Logout(LoginOutRequest loginrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Login/Logout?token=" + Token, JSONHelper.ToJSONString(loginrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<DataContract.UserInfo> UserLogin(UserLoginRequest userLoginRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Login/UserLogin?token=" + Token, JSONHelper.ToJSONString(userLoginRequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserInfo>>(responseStr);
        }
    }
}

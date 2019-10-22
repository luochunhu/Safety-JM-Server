using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request;
using System.Data;
using Sys.Safety.Request.Config;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using Sys.Safety.Request.Login;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 用户登录WebApi接口
    /// </summary>
    public class LoginController : Basic.Framework.Web.WebApi.BasicApiController, ILoginService
    {
        static LoginController()
        {

        }
        ILoginService _loginService = ServiceFactory.Create<ILoginService>();
        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <param name="loginrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Login/Login")]
        public BasicResponse<Dictionary<string, object>> Login(LoginRequest loginrequest)
        {
            return _loginService.Login(loginrequest);
        }

        /// <summary>
        /// 注销
        /// </summary>
        [HttpPost]
        [Route("v1/Login/Logout")]
        public BasicResponse Logout(LoginOutRequest loginrequest)
        {
            return _loginService.Logout(loginrequest);
        }

        /// <summary>
        /// 用户登陆（成功返回用户信息）
        /// </summary>
        [HttpPost]
        [Route("v1/Login/UserLogin")]
        public BasicResponse<UserInfo> UserLogin(UserLoginRequest userLoginRequest)
        {
            return _loginService.UserLogin(userLoginRequest);
        }
    }
}

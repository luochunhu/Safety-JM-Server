using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.UserRoleAuthorize
{
    public interface ILoginService
    {
        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <param name="loginContext"></param>
        /// <returns></returns>
        BasicResponse<Dictionary<string, object>> Login(LoginRequest loginrequest);

        /// <summary>
        /// 注销
        /// </summary>
        BasicResponse Logout(LoginOutRequest loginrequest);

        /// <summary>
        /// 用户登录(登陆成功返回用户信息)
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        BasicResponse<UserInfo> UserLogin(UserLoginRequest userLoginRequest);
    }
}

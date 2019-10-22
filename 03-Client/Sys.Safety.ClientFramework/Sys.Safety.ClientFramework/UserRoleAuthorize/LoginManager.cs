using Basic.Framework.Common;
using Basic.Framework.Service;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.Login;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sys.Safety.ClientFramework.UserRoleAuthorize
{
    /// <summary>
    /// 登录管理器
    /// </summary>
    public class LoginManager
    {
        static ILoginService _LoginService = ServiceFactory.Create<ILoginService>();
        /// <summary>
        /// 登录用户发生改变的事件
        /// </summary>
        public static event EventHandler LogOnChangeEvent;
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static string LoginUserNameNow = "";
        /// <summary>
        /// 登录成功是否加载菜单标记(根据此标记来判断是否是主控操作登录验证)  20170706
        /// </summary>
        public static bool LoginSuccessIsLoadMenu = false;
        /// <summary>
        /// 登录成功与否标记（用于主控模块操作登录验证）  20170711
        /// </summary>
        public static bool isLoginSuccess = false;


        public static void OnLogOnChanged()
        {
            //当重新登录时响应
            if (null != LogOnChangeEvent)
            {
                LogOnChangeEvent(null, null);
            }
        }

        /// <summary>
        /// 登录，客户端调用
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void Login(string userName, string password, string menutype)
        {
            Dictionary<string, object> loginContext = new Dictionary<string, object>();

            loginContext.Add(KeyConst.LoginUserKey, userName);
            loginContext.Add(KeyConst.LoginPasswordKey, password);
            loginContext.Add(KeyConst.UserMenuTypeKey, menutype);


            LoginRequest loginrequest = new LoginRequest();
            loginrequest.loginContext = loginContext;
            var Result = _LoginService.Login(loginrequest);
            if (Result.Code == 1)
            {
                throw new Exception("&&" + Result.Message);
            }
            Dictionary<string, object> Rvalue = Result.Data;
            ClientPermission.InitLogin(Rvalue);

            if (IsLogin)
            {
                LoginUserNameNow = userName;
                OnLogOnChanged();
            }
        }
        /// <summary>
        /// 用户菜单加载
        /// </summary>
        /// <param name="MenuType"></param>
        public static void MenuChange(string MenuType)
        {
            //获取当前登录用户
            string UserNameNow = Basic.Framework.Data.PlatRuntime.Items[KeyConst.LoginUserKey].ToString();
            string PasswordNow = Basic.Framework.Data.PlatRuntime.Items[KeyConst.LoginPasswordKey].ToString();
            //重新登录并加载菜单 
            if (IsLogin)
            {
                Logout();//退出当前登录
            }
            Login(UserNameNow, PasswordNow, MenuType);
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void Logout()
        {
            if (!IsLogin)
            {
                //modified by qy 20150108
                return;
            }
            LoginOutRequest _LoginOutRequest = new LoginOutRequest();
            _LoginOutRequest.UserName = LoginUserNameNow;
            //调用服务端logout方法并清除相应缓存            
            _LoginService.Logout(_LoginOutRequest);

            //清除客户端相应缓存
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove(KeyConst.ClientItemKey);
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_LoginUserCode"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_LoginUserCode");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_LoginPassword"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_LoginPassword");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_MenuType"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_MenuType");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_UserDto"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_UserDto");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_SessionId"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_SessionId");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_LoginTime"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_LoginTime");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_LastUpdateTime"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_LastUpdateTime");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_IsLogin"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_IsLogin");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_Rights"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_Rights");
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_Menus"))
            {
                Basic.Framework.Data.PlatRuntime.Items.Remove("_Menus");
            }
        }

        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.IsLoginKey))
                {
                    return TypeConvert.ToBool(Basic.Framework.Data.PlatRuntime.Items[KeyConst.IsLoginKey]);
                }
                else
                {
                    return false;
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Basic.Framework.Logging;
using Sys.Safety.Request.Login;
using Basic.Framework.Web;

namespace Sys.Safety.Services.UserRoleAuthorize
{
    /// <summary>
    /// 用户登录基类
    /// </summary>    
    public class BaseLoginService
    {
        /// <summary>
        /// 登录上下文
        /// </summary>
        protected Dictionary<string, object> _loginContext = new Dictionary<string, object>();

        /// <summary>
        /// 登录验证，虚方法，需要子类重写
        /// </summary>
        protected virtual void ValidateLogin()
        {

        }

        /// <summary>
        /// 设置登录上下文，虚方法，需要子类重写
        /// </summary>
        protected virtual void SetLoginContext()
        {

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginContext">登录信息</param>
        /// <returns></returns>
        public Dictionary<string, object> Login(Dictionary<string, object> loginContext)
        {
            try
            {
                _loginContext = new Dictionary<string, object>();

                _loginContext = loginContext;

                //1.先登录效验
                ValidateLogin();

                //2.验证通过后设置相应上下文
                //给用户分配session号
                string sessionId = Guid.NewGuid().ToString();
                _loginContext.Add(KeyConst.SessionIdKey, sessionId);
                //设置登录时间
                _loginContext.Add(KeyConst.LoginTimeKey, DateTime.Now);

                //最近一次操作时间
                _loginContext.Add(KeyConst.LastUpdateTimeKey, DateTime.Now);

                _loginContext.Add(KeyConst.IsLoginKey, true);

                SetLoginContext();

                //设置登录的信息到服务端上下文缓存,修改成根据用户进行缓存                 
                //ServerContext.Current.SetContext(loginContext[KeyConst.LoginUserKey].ToString(), loginContext);
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(loginContext[KeyConst.LoginUserKey].ToString()))
                {
                    Basic.Framework.Data.PlatRuntime.Items[loginContext[KeyConst.LoginUserKey].ToString()] = loginContext;
                }
                else
                {
                    Basic.Framework.Data.PlatRuntime.Items.Add(loginContext[KeyConst.LoginUserKey].ToString(), loginContext);
                }
            }
            catch (Exception ex)
            {
                //Log.WriteError(ex);
                LogHelper.Error("Login:" + "错误：\n" + ex.Message + ex.StackTrace);
                throw ex;
            }
            return _loginContext;
        }

        /// <summary>
        /// 登出操作
        /// </summary>
        public virtual BasicResponse Logout(LoginOutRequest loginrequest)
        {
            BasicResponse Result = new BasicResponse();
            try
            {
                //ServerContext.Current.RemoveContext();
                Basic.Framework.Data.PlatRuntime.Items.Remove(loginrequest.UserName);
            }
            catch (Exception ex)
            {
                //Log.WriteError(ex);
                LogHelper.Error("Logout:" + "错误：\n" + ex.Message + ex.StackTrace);
                throw ex;
            }
            return Result;
        }
    }
}

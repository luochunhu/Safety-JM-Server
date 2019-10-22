using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using Basic.Framework.Logging;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Basic.Framework.Common;
using Sys.Safety.Model;
using Sys.Safety.DataContract;
using Basic.Framework.Web;
using Sys.Safety.Request.Login;

namespace Sys.Safety.Services.UserRoleAuthorize
{
    /// <summary>
    /// 登录服务实现
    /// </summary>
    [Serializable]
    public class LoginService : BaseLoginService, ILoginService
    {
        /// <summary>
        /// 用户操作接口
        /// </summary>
        private IUserRepository _UserRepository;
        /// <summary>
        /// 配置操作接口
        /// </summary>
        private ISettingRepository _SettingRepository;
        /// <summary>
        /// 权限操作接口
        /// </summary>
        private IRightRepository _RightRepository;

        public LoginService(IUserRepository UserRepository, ISettingRepository SettingRepository, IRightRepository RightRepository)
        {
            this._UserRepository = UserRepository;
            this._SettingRepository = SettingRepository;
            this._RightRepository = RightRepository;
        }
        public void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("LoginService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        protected override void ValidateLogin()
        {
            try
            {
                if (base._loginContext == null || !base._loginContext.ContainsKey(KeyConst.LoginUserKey) || !base._loginContext.ContainsKey(KeyConst.LoginPasswordKey))
                {
                    ThrowException("ValidateLogin", new Exception("请勿非法登录访问！"));
                }

                string userCode = base._loginContext[KeyConst.LoginUserKey].ToString();
                string password = base._loginContext[KeyConst.LoginPasswordKey].ToString();

                if (string.IsNullOrEmpty(userCode))
                {
                    ThrowException("ValidateLogin", new Exception("用户名不能为空！"));
                }

                if (string.IsNullOrEmpty(password))
                {
                    ThrowException("ValidateLogin", new Exception("密码不能为空！"));
                }

                //1. todo 调用用户接口验证用户名和密码 
                var item = _UserRepository.GetUserByCode(userCode);
                UserInfo user = ObjectConverter.Copy<UserModel, UserInfo>(item);

                if (user == null)
                {
                    ThrowException("ValidateLogin", new Exception("登录用户名不存在，请重新输入！"));
                }
                if (user.Password != Basic.Framework.Common.MD5Helper.MD5Encrypt(password))
                {
                    ThrowException("ValidateLogin", new Exception("登录密码错误，请重新输入！"));
                }
                if (user.UserFlag == 0)
                {
                    ThrowException("ValidateLogin", new Exception("此帐户已经被停用！"));
                }

                base._loginContext.Add(KeyConst.UserDtoKey, user);

                base.ValidateLogin();
            }
            catch (System.Exception ex)
            {
                ThrowException("ValidateLogin", ex);
            }
        }
        /// <summary>
        /// 设置登录信息、权限及上下文
        /// </summary>
        protected override void SetLoginContext()
        {
            try
            {

                Dictionary<string, RightItem> rights = new Dictionary<string, RightItem>();

                //1. 调用权限接口获取权限
                string userCode = base._loginContext[KeyConst.LoginUserKey].ToString();

                List<RightModel> rightList = _UserRepository.GetUserRights(userCode);

                if (rightList != null && rightList.Count > 0)
                {
                    foreach (RightModel rightDto in rightList)
                    {
                        RightItem item = new RightItem()
                        {
                            RightCode = rightDto.RightCode,
                            RightName = rightDto.RightName
                        };

                        rights.Add(item.RightCode, item);
                    }
                }

                base._loginContext.Add(KeyConst.RightKey, rights);

                InitSetting();

                InitMenuList();

                //2.todo 调用数据权限            
                base.SetLoginContext();
            }
            catch (System.Exception ex)
            {
                ThrowException("SetLoginContext", ex);
            }
        }
        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <param name="loginContext"></param>
        /// <returns></returns>
        public BasicResponse<Dictionary<string, object>> Login(LoginRequest loginrequest)
        {
            BasicResponse<Dictionary<string, object>> Result = new BasicResponse<Dictionary<string, object>>();
            try
            {
                Dictionary<string, object> rvalue = base.Login(loginrequest.loginContext);
                Result.Data = rvalue;
            }
            catch(Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                Result.Code = 1;
                Result.Message = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 根据用户信息获取菜单列表
        /// 2015-5-7 将菜单列表转化为字典进行存储
        /// </summary>
        private void InitMenuList()
        {
            try
            {
                string userCode = base._loginContext[KeyConst.LoginUserKey].ToString();
                string userMenuType = base._loginContext[KeyConst.UserMenuTypeKey].ToString();

                IDictionary<string, MenuInfo> dic = new Dictionary<string, MenuInfo>();

                List<MenuModel> menuLstModel = _UserRepository.GetUserMenus(userCode, userMenuType);

                List<MenuInfo> menuLst = ObjectConverter.CopyList<MenuModel, MenuInfo>(menuLstModel).ToList();

                if (menuLst != null && menuLst.Count > 0)
                {
                    for (int i = 0; i < menuLst.Count; i++)
                    {
                        string strKey = menuLst[i].MenuID.ToString();
                        MenuInfo menuDTOValue = menuLst[i];
                        dic.Add(strKey, menuDTOValue);
                    }
                }
                base._loginContext.Add("_Menus", dic);
            }
            catch (System.Exception ex)
            {
                ThrowException("InitMenuList", ex);
            }
        }
        /// <summary>
        /// 初始系统配置信息
        /// </summary>
        private void InitSetting()
        {
            try
            {
                IDictionary<string, string> dic = new Dictionary<string, string>();
                List<SettingModel> ListSetting = _SettingRepository.GetSettingList();
                foreach (SettingModel row in ListSetting)
                {
                    string strKey = Convert.ToString(row.StrKey);
                    string strValue = Convert.ToString(row.StrValue);
                    if (!dic.ContainsKey(strKey))
                    {
                        dic.Add(strKey, strValue);
                    }
                }
                base._loginContext.Add("CustomerSetting", dic);



                ////2017-04-13 加载枚举缓存
                //IDictionary<string, DataTable> dict = new Dictionary<string, DataTable>();
                //IRequestService requestSercie = ServiceFactory.CreateService<IRequestService>();
                //dt = requestSercie.GetDataTableBySQL("select * from BFT_EnumCode");
                //dict.Add("CustomerEnumCode",TypeConvert.ToEntityFromTable(Cbf);
                //base._loginContext.Add("CustomerEnumCode", dict);

            }
            catch (System.Exception ex)
            {
                ThrowException("InitSetting", ex);
            }
        }
        /// <summary>
        /// 注销
        /// </summary>
        public override BasicResponse Logout(LoginOutRequest loginrequest)
        {
            BasicResponse Result = new BasicResponse();
            base.Logout(loginrequest);
            return Result;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        public BasicResponse<UserInfo> UserLogin(UserLoginRequest userLoginRequest)
        {
            BasicResponse<UserInfo> userresponse = new BasicResponse<UserInfo>();

            var item = _UserRepository.GetUserByCode(userLoginRequest.UserName);
            UserInfo user = ObjectConverter.Copy<UserModel, UserInfo>(item);

            if (user == null)
            {
                ThrowException("ValidateLogin", new Exception("登录用户名不存在，请重新输入！"));
            }
            if (user.Password != Basic.Framework.Common.MD5Helper.MD5Encrypt(userLoginRequest.Password))
            {
                ThrowException("ValidateLogin", new Exception("登录密码错误，请重新输入！"));
            }
            if (user.UserFlag == 0)
            {
                ThrowException("ValidateLogin", new Exception("此帐户已经被停用！"));
            }
            userresponse.Data = user;
            return userresponse;
        }
    }
}

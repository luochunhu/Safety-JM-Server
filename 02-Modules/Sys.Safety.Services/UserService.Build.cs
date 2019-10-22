using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.User;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class UserService : IUserService
    {
        private IUserRepository _Repository;
        private IUserroleRepository _userRoleRepository;

        public UserService(IUserRepository _Repository,IUserroleRepository userRoleRepository)
        {
            this._Repository = _Repository;
            this._userRoleRepository = userRoleRepository;
        }
        public BasicResponse<UserInfo> AddUser(UserAddRequest userrequest)
        {
            UserInfo userDTO = userrequest.UserInfo;
            if (userDTO.CreateTime == new DateTime(1, 1, 1, 0, 0, 0))
            {
                userDTO.CreateTime = DateTime.Now;
            }
            if (userDTO.LastLoginTime == new DateTime(1, 1, 1, 0, 0, 0))
            {
                userDTO.LastLoginTime = DateTime.Now;
            }
            if (userDTO.LoginTime == new DateTime(1, 1, 1, 0, 0, 0))
            {
                userDTO.LoginTime = DateTime.Now;
            }
            if (userDTO.CreateTime != null)
            {
                userDTO.CreateTime = Convert.ToDateTime(userDTO.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (userDTO.LastLoginTime != null)
            {
                userDTO.LastLoginTime = Convert.ToDateTime(userDTO.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (userDTO.LoginTime != null)
            {
                userDTO.LoginTime = Convert.ToDateTime(userDTO.LoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (userDTO == null)
            {
                //throw new BusinessException("用户DTO对象不能为空！");
                ThrowException("AddUser", new Exception("用户DTO对象不能为空！"));
            }
            if (string.IsNullOrEmpty(userDTO.UserCode))
            {
                //throw new BusinessException("用户名不能为空，请重新输入！");
                ThrowException("AddUser", new Exception("用户名不能为空，请重新输入！"));
            }
            if (CheckUserNameExist(userDTO.UserCode))
            {
                //校验用户名是否重复                
                ThrowException("AddUser", new Exception(String.Format("用户名:{0} 已经重复，请重新输入！", userDTO.UserCode)));
            }

            var _user = ObjectConverter.Copy<UserInfo, UserModel>(userDTO);
            var resultuser = _Repository.AddUser(_user);
            var userresponse = new BasicResponse<UserInfo>();
            userresponse.Data = ObjectConverter.Copy<UserModel, UserInfo>(resultuser);
            return userresponse;
        }
        public BasicResponse<UserInfo> UpdateUser(UserUpdateRequest userrequest)
        {
            var _user = ObjectConverter.Copy<UserInfo, UserModel>(userrequest.UserInfo);
            _Repository.UpdateUser(_user);
            var userresponse = new BasicResponse<UserInfo>();
            userresponse.Data = ObjectConverter.Copy<UserModel, UserInfo>(_user);
            return userresponse;
        }
        public BasicResponse DeleteUser(UserDeleteRequest userrequest)
        {
            UserModel userDTO = _Repository.GetUserById(userrequest.Id);
            if (userDTO == null)
            {                
                ThrowException("DeleteUser", new Exception("未找到当前删除的对象！"));
            }            
            if (userDTO.UserCode == "admin")
            {                
                ThrowException("DeleteUser", new Exception("admin为超级用户不能进行删除操作！"));
            }
            _Repository.DeleteUser(userrequest.Id);
            var userresponse = new BasicResponse();
            return userresponse;
        }
        public BasicResponse<List<UserInfo>> GetUserList(UserGetListRequest userrequest)
        {
            var userresponse = new BasicResponse<List<UserInfo>>();
            userrequest.PagerInfo.PageIndex = userrequest.PagerInfo.PageIndex - 1;
            if (userrequest.PagerInfo.PageIndex < 0)
            {
                userrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var userModelLists = _Repository.GetUserList(userrequest.PagerInfo.PageIndex, userrequest.PagerInfo.PageSize, out rowcount);
            var userInfoLists = new List<UserInfo>();
            foreach (var item in userModelLists)
            {
                var UserInfo = ObjectConverter.Copy<UserModel, UserInfo>(item);
                userInfoLists.Add(UserInfo);
            }
            userresponse.Data = userInfoLists;
            return userresponse;
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<UserInfo>> GetUserList()
        {
            var userresponse = new BasicResponse<List<UserInfo>>();           
            var userModelLists = _Repository.GetUserList();
            var userInfoLists = new List<UserInfo>();

            var userrolelist=_userRoleRepository.Datas.ToList();

            foreach (var item in userModelLists)
            {
                var userrole = userrolelist.FirstOrDefault(r => r.UserID == item.UserID);
                var UserInfo = ObjectConverter.Copy<UserModel, UserInfo>(item);

                UserInfo.RoleID = userrole == null ? string.Empty : userrole.RoleID;

                userInfoLists.Add(UserInfo);
            }
            userresponse.Data = userInfoLists;
            return userresponse;
        }
        public BasicResponse<UserInfo> GetUserById(UserGetRequest userrequest)
        {
            var result = _Repository.GetUserById(userrequest.Id);
            var userInfo = ObjectConverter.Copy<UserModel, UserInfo>(result);
            var userresponse = new BasicResponse<UserInfo>();
            userresponse.Data = userInfo;
            return userresponse;
        }
        /// <summary>
        /// 根据用户编码获取用户信息
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        public BasicResponse<UserInfo> GetUserByCode(UserGetByCodeRequest userrequest)
        {
            var result = _Repository.GetUserByCode(userrequest.Code);
            var userInfo = ObjectConverter.Copy<UserModel, UserInfo>(result);
            var userresponse = new BasicResponse<UserInfo>();
            userresponse.Data = userInfo;
            return userresponse;
        }
        /// <summary>
        /// 获取已启用的用户
        /// </summary>
        /// <returns></returns>
        public BasicResponse<UserInfo> GetEnableUser()
        {
            var result = _Repository.GetEnableUser();
            var userInfo = ObjectConverter.Copy<UserModel, UserInfo>(result);
            var userresponse = new BasicResponse<UserInfo>();
            userresponse.Data = userInfo;
            return userresponse;
        }
        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("MenuService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        /// <summary>
        /// 添加一个全新信息到用户表并返回成功后的用户对象(支持添加和更新操作)
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public BasicResponse<UserInfo> AddUserEx(UserAddRequest userrequest)
        {
            BasicResponse<UserInfo> Result = new BasicResponse<UserInfo>();
            UserInfo userDTO = userrequest.UserInfo;
            try
            {
                long ID = 0;
                if (userDTO.CreateTime == new DateTime(1, 1, 1, 0, 0, 0))
                {
                    userDTO.CreateTime = DateTime.Now;
                }
                if (userDTO.LastLoginTime == new DateTime(1, 1, 1, 0, 0, 0))
                {
                    userDTO.LastLoginTime = DateTime.Now;
                }
                if (userDTO.LoginTime == new DateTime(1, 1, 1, 0, 0, 0))
                {
                    userDTO.LoginTime = DateTime.Now;
                }
                if (userDTO.CreateTime != null)
                {
                    userDTO.CreateTime = Convert.ToDateTime(userDTO.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (userDTO.LastLoginTime != null)
                {
                    userDTO.LastLoginTime = Convert.ToDateTime(userDTO.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (userDTO.LoginTime != null)
                {
                    userDTO.LoginTime = Convert.ToDateTime(userDTO.LoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (userDTO == null)
                {
                    ThrowException("AddUserEx", new Exception("用户DTO对象不能为空！"));
                }
                if (string.IsNullOrEmpty(userDTO.UserCode))
                {
                    ThrowException("AddUserEx", new Exception("用户名不能为空，请重新输入！"));
                }
                if (userDTO.InfoState == InfoState.AddNew)
                {
                    //判断用户名是否存在，只有不存在才能插入
                    if (!CheckUserNameExist(userDTO.UserCode))
                    {
                        ID = IdHelper.CreateLongId();
                        userDTO.UserID = ID.ToString();
                        var _user = ObjectConverter.Copy<UserInfo, UserModel>(userDTO);
                        var resultuser = _Repository.AddUser(_user);
                        Result.Data = ObjectConverter.Copy<UserModel, UserInfo>(resultuser);
                    }
                    else
                    {
                        //校验用户名是否重复                       
                        ThrowException("AddUserEx", new Exception("用户已存在，请重新输入！"));
                    }
                }
                else
                {
                    var _user = ObjectConverter.Copy<UserInfo, UserModel>(userDTO);
                    _Repository.UpdateUser(_user);
                    Result.Data = userDTO;
                }               
            }
            catch (System.Exception ex)
            {
                Result.Code = 1;
                Result.Message = ex.Message;
            }
            return Result;
        }

        /// <summary>
        /// 获取用户对应的菜单信息     
        /// </summary>    
        /// <returns>菜单列表</returns>       
        public BasicResponse<List<MenuInfo>> GetUserMenus(UserMenusGetRequest userrequest)
        {
            BasicResponse<List<MenuInfo>> Result = new BasicResponse<List<MenuInfo>>();
            List<MenuInfo> menuList = new List<MenuInfo>();
            try
            {
                List<MenuModel> menuModelList = _Repository.GetUserMenus(userrequest.userCode, userrequest.MenuType);
                menuList = ObjectConverter.CopyList<MenuModel, MenuInfo>(menuModelList).ToList();
                Result.Data = menuList;
            }
            catch (System.Exception ex)
            {
                ThrowException("GetUserMenus", ex);
            }
            return Result;
        }
        /// <summary>
        /// 获取用户对应的权限信息
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        public BasicResponse<List<RightInfo>> GetUserRights(UserRightsGetRequest userrequest)
        {
            BasicResponse<List<RightInfo>> Result = new BasicResponse<List<RightInfo>>();
            List<RightInfo> rightList = new List<RightInfo>();
            try
            {
                List<RightModel> rightModelList = _Repository.GetUserRights(userrequest.userCode);
                rightList = ObjectConverter.CopyList<RightModel, RightInfo>(rightModelList).ToList();
                Result.Data = rightList;
            }
            catch (System.Exception ex)
            {
                ThrowException("GetUserRights", ex);
            }
            return Result;
        }
        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        public BasicResponse EnableUser(UsersRequest userrequest)
        {
            BasicResponse Result = new BasicResponse();
            List<UserInfo> lstUserDTO = userrequest.UserInfo;
            try
            {
                if (lstUserDTO.Count <= 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                foreach (UserInfo tempUserDTO in lstUserDTO)
                {
                    tempUserDTO.UserFlag = 1;
                    var _request = ObjectConverter.Copy<UserInfo, UserModel>(tempUserDTO);
                    _Repository.Update(_request);
                }
            }
            catch
            {
                Result.Code = 2;
                Result.Message = "操作失败";
            }
            return Result;
        }
        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        public BasicResponse DisableUser(UsersRequest userrequest)
        {
            BasicResponse Result = new BasicResponse();
            List<UserInfo> lstUserDTO = userrequest.UserInfo;
            try
            {
                if (lstUserDTO.Count <= 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                foreach (UserInfo tempUserDTO in lstUserDTO)
                {
                    tempUserDTO.UserFlag = 0;
                    var _request = ObjectConverter.Copy<UserInfo, UserModel>(tempUserDTO);
                    _Repository.Update(_request);
                }
            }
            catch
            {
                Result.Code = 2;
                Result.Message = "操作失败";
            }
            return Result;
        }

        /// <summary>
        /// 根据传入的登录名判断当前用户是否已存在
        /// </summary>
        /// <param name="userName">登录用户名</param>
        /// <returns>true:存在  false:不存在</returns>
        private bool CheckUserNameExist(string userName)
        {            
            bool isExist = false;
            string strSql = "";
            UserModel userDTO = null;
            try
            {
                List<UserModel> UserList = _Repository.GetUserList();
                userDTO = UserList.Find(a => a.UserCode == userName);
                if (userDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(userDTO.UserID) > 0)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch
            {
                isExist = false;
            }            
            return isExist;
        }
    }
}



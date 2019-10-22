using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.User;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IUserService
    {
        BasicResponse<UserInfo> AddUser(UserAddRequest userrequest);
        BasicResponse<UserInfo> UpdateUser(UserUpdateRequest userrequest);
        BasicResponse DeleteUser(UserDeleteRequest userrequest);
        BasicResponse<List<UserInfo>> GetUserList(UserGetListRequest userrequest);
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<UserInfo>> GetUserList();
        BasicResponse<UserInfo> GetUserById(UserGetRequest userrequest);
        /// <summary>
        /// 根据用户编码获取用户信息
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        BasicResponse<UserInfo> GetUserByCode(UserGetByCodeRequest userrequest);
        /// <summary>
        /// 获取已启用的用户
        /// </summary>
        /// <returns></returns>
        BasicResponse<UserInfo> GetEnableUser();
        /// <summary>
        /// 添加一个全新信息到用户表并返回成功后的用户对象(支持添加和更新操作)
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        BasicResponse<UserInfo> AddUserEx(UserAddRequest userrequest);
        /// <summary>
        /// 获取用户对应的菜单信息     
        /// </summary>    
        /// <returns>菜单列表</returns>       
        BasicResponse<List<MenuInfo>> GetUserMenus(UserMenusGetRequest userrequest);
        /// <summary>
        /// 获取用户对应的权限信息
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        BasicResponse<List<RightInfo>> GetUserRights(UserRightsGetRequest userrequest);
        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        BasicResponse EnableUser(UsersRequest userrequest);
        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        BasicResponse DisableUser(UsersRequest userrequest);        
    }
}


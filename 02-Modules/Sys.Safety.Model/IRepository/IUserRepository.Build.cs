using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data;

namespace Sys.Safety.Model
{
    public interface IUserRepository : IRepository<UserModel>
    {
        UserModel AddUser(UserModel userModel);
        void UpdateUser(UserModel userModel);
        void DeleteUser(string id);
        IList<UserModel> GetUserList(int pageIndex, int pageSize, out int rowCount);
        List<UserModel> GetUserList();
        UserModel GetUserById(string id);
        /// <summary>
        /// 获取已启用的用户列表
        /// </summary>       
        /// <returns></returns>
        UserModel GetEnableUser();
        /// <summary>
        /// 根据用户编码获取用户信息
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        UserModel GetUserByCode(string UserCode);
        /// <summary>
        /// 根据用户编码获取用户对应的权限信息
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        List<RightModel> GetUserRights(string UserCode);
        /// <summary>
        /// 根据用户编码获取用户对应的菜单信息
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="MenuType"></param>
        /// <returns></returns>
        List<MenuModel> GetUserMenus(string UserCode, string MenuType);
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;
using System.Data;

namespace Sys.Safety.DataAccess
{
    public partial class UserRepository : RepositoryBase<UserModel>, IUserRepository
    {

        public UserModel AddUser(UserModel userModel)
        {
            return base.Insert(userModel);
        }
        public void UpdateUser(UserModel userModel)
        {
            base.Update(userModel);
        }
        public void DeleteUser(string id)
        {
            base.Delete(id);
        }
        public IList<UserModel> GetUserList(int pageIndex, int pageSize, out int rowCount)
        {
            var userModelLists = base.Datas;
            rowCount = userModelLists.Count();
            return userModelLists.OrderBy(p => p.CreateTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<UserModel> GetUserList()
        {
            var userModelLists = base.Datas.ToList();
            return userModelLists;
        }
        public UserModel GetUserById(string id)
        {
            UserModel userModel = base.Datas.FirstOrDefault(c => c.UserID == id);
            return userModel;
        }
        /// <summary>
        /// 根据用户编码获取用户信息
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public UserModel GetUserByCode(string UserCode)
        {
            UserModel userModel = base.Datas.FirstOrDefault(c => c.UserCode == UserCode);
            return userModel;
        }
        /// <summary>
        /// 获取已启用的用户列表
        /// </summary>       
        /// <returns></returns>
        public UserModel GetEnableUser()
        {
            UserModel userModel = base.Datas.FirstOrDefault(c => c.UserFlag == 1);
            return userModel;
        }

        /// <summary>
        /// 根据用户编码获取用户对应的权限信息
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public List<RightModel> GetUserRights(string UserCode)
        {
            DataTable userRights = base.QueryTable("global_UserService_GetRightsByUserCode", UserCode);
            return Basic.Framework.Common.ObjectConverter.Copy<RightModel>(userRights);
        }
        /// <summary>
        /// 根据用户编码获取用户对应的菜单信息
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="MenuType"></param>
        /// <returns></returns>
        public List<MenuModel> GetUserMenus(string UserCode, string MenuType)
        {
            DataTable userRights = base.QueryTable("global_UserService_GetMenusByUserCodeMenuType", UserCode, MenuType);
            return Basic.Framework.Common.ObjectConverter.Copy<MenuModel>(userRights);
        }
    }
}

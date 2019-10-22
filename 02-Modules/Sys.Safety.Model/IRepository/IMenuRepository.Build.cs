using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IMenuRepository : IRepository<MenuModel>
    {
        MenuModel AddMenu(MenuModel menuModel);
        void UpdateMenu(MenuModel menuModel);
        void DeleteMenu(string id);
        IList<MenuModel> GetMenuList(int pageIndex, int pageSize, out int rowCount);
        List<MenuModel> GetMenuList();
        MenuModel GetMenuById(string id);
        /// <summary>
        /// 根据编码查找
        /// </summary>
        /// <param name="MenuCode"></param>
        /// <returns></returns>
        MenuModel GetMenuByCode(string MenuCode);
    }
}

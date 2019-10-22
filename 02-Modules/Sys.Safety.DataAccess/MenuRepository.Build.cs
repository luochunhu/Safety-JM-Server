using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class MenuRepository : RepositoryBase<MenuModel>, IMenuRepository
    {

        public MenuModel AddMenu(MenuModel menuModel)
        {
            return base.Insert(menuModel);
        }
        public void UpdateMenu(MenuModel menuModel)
        {
            base.Update(menuModel);
        }
        public void DeleteMenu(string id)
        {
            base.Delete(id);
        }
        public IList<MenuModel> GetMenuList(int pageIndex, int pageSize, out int rowCount)
        {
            var menuModelLists = base.Datas;
            rowCount = menuModelLists.Count();
            return menuModelLists.OrderBy(p => p.MenuID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<MenuModel> GetMenuList()
        {
            var menuModelLists = base.Datas.ToList();           
            return menuModelLists;
        }
        public MenuModel GetMenuById(string id)
        {
            MenuModel menuModel = base.Datas.FirstOrDefault(c => c.MenuID == id);
            return menuModel;
        }
        /// <summary>
        /// 根据编码查找
        /// </summary>
        /// <param name="MenuCode"></param>
        /// <returns></returns>
        public MenuModel GetMenuByCode(string MenuCode)
        {
            MenuModel menuModel = base.Datas.FirstOrDefault(c => c.MenuCode == MenuCode);
            return menuModel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Menu;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IMenuService
    {
        BasicResponse<MenuInfo> AddMenu(MenuAddRequest menurequest);
        BasicResponse<MenuInfo> UpdateMenu(MenuUpdateRequest menurequest);
        BasicResponse DeleteMenu(MenuDeleteRequest menurequest);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>
        BasicResponse DeleteMenus(MenusDeleteRequest menurequest);
        BasicResponse<List<MenuInfo>> GetMenuList(MenuGetListRequest menurequest);
        /// <summary>
        /// 获取所有菜单信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<MenuInfo>> GetMenuList();
        BasicResponse<MenuInfo> GetMenuById(MenuGetRequest menurequest);
        /// <summary>
        /// 根据菜单编码得到菜单名称
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>
        BasicResponse<string> GetMenuNameByMenuCode(MenuGetByCOdeRequest menurequest);
        /// <summary>
        /// 添加一个全新信息到菜单表并返回成功后的菜单对象(支持添加和更新)
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>
        BasicResponse<MenuInfo> AddMenuEx(MenuAddRequest menurequest);
        /// <summary>
        /// 启用菜单
        /// </summary>
        /// <param name="lstMenuDTO"></param>
        /// <returns></returns>
        BasicResponse EnablMenu(MenusUpdateRequest lstMenuDTO);
        /// <summary>
        /// 禁用菜单
        /// </summary>
        /// <param name="lstMenuDTO"></param>
        /// <returns></returns>
        BasicResponse DisableMenu(MenusUpdateRequest lstMenuDTO);
    }
}


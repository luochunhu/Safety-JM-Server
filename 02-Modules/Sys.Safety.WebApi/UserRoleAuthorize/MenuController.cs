using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request;
using System.Data;
using Sys.Safety.Request.Config;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using Sys.Safety.Request.Login;
using Sys.Safety.Request.Menu;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 菜单管理WebApi接口
    /// </summary>
    public class MenuController : Basic.Framework.Web.WebApi.BasicApiController, IMenuService
    {
        static MenuController()
        {

        }
        IMenuService _menuService = ServiceFactory.Create<IMenuService>();        
        [HttpPost]
        [Route("v1/Menu/Add")]
        public BasicResponse<MenuInfo> AddMenu(MenuAddRequest menurequest)
        {
            return _menuService.AddMenu(menurequest);
        }
        [HttpPost]
        [Route("v1/Menu/Update")]
        public BasicResponse<MenuInfo> UpdateMenu(MenuUpdateRequest menurequest)
        {
            return _menuService.UpdateMenu(menurequest);
        }
        [HttpPost]
        [Route("v1/Menu/Delete")]
        public BasicResponse DeleteMenu(MenuDeleteRequest menurequest)
        {
            return _menuService.DeleteMenu(menurequest);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Menu/DeleteMenus")]
        public BasicResponse DeleteMenus(MenusDeleteRequest menurequest)
        {
            return _menuService.DeleteMenus(menurequest);
        }
        [HttpPost]
        [Route("v1/Menu/GetPageList")]
        public BasicResponse<List<MenuInfo>> GetMenuList(MenuGetListRequest menurequest)
        {
            return _menuService.GetMenuList(menurequest);
        }
        [HttpPost]
        [Route("v1/Menu/GetAllList")]
        public BasicResponse<List<MenuInfo>> GetMenuList()
        {
            return _menuService.GetMenuList();
        }
        [HttpPost]
        [Route("v1/Menu/Get")]
        public BasicResponse<MenuInfo> GetMenuById(MenuGetRequest menurequest)
        {
            return _menuService.GetMenuById(menurequest);
        }
        /// <summary>
        /// 根据菜单编码得到菜单名称
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Menu/GetMenuNameByMenuCode")]
        public BasicResponse<string> GetMenuNameByMenuCode(MenuGetByCOdeRequest menurequest)
        {
            return _menuService.GetMenuNameByMenuCode(menurequest);
        }
        /// <summary>
        /// 添加一个全新信息到菜单表并返回成功后的菜单对象(支持添加和更新)
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>    
        [HttpPost]
        [Route("v1/Menu/AddMenuEx")]
        public BasicResponse<MenuInfo> AddMenuEx(MenuAddRequest menurequest)
        {
            return _menuService.AddMenuEx(menurequest);
        }
        /// <summary>
        /// 启用菜单
        /// </summary>
        /// <param name="lstMenuDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Menu/EnablMenu")]
        public BasicResponse EnablMenu(MenusUpdateRequest lstMenuDTO)
        {
            return _menuService.EnablMenu(lstMenuDTO);
        }
        /// <summary>
        /// 禁用菜单
        /// </summary>
        /// <param name="lstMenuDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Menu/DisableMenu")]
        public BasicResponse DisableMenu(MenusUpdateRequest lstMenuDTO)
        {
            return _menuService.DisableMenu(lstMenuDTO);
        }
    }
}

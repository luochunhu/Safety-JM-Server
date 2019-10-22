using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Menu;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.UserRoleAuthorize
{
    public class MenuControllerProxy : BaseProxy, IMenuService
    {
        public BasicResponse<MenuInfo> AddMenu(MenuAddRequest menurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/Add?token=" + Token, JSONHelper.ToJSONString(menurequest));
            return JSONHelper.ParseJSONString<BasicResponse<MenuInfo>>(responseStr);
        }
        public BasicResponse<MenuInfo> UpdateMenu(MenuUpdateRequest menurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/Update?token=" + Token, JSONHelper.ToJSONString(menurequest));
            return JSONHelper.ParseJSONString<BasicResponse<MenuInfo>>(responseStr);
        }
        public BasicResponse DeleteMenu(MenuDeleteRequest menurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/Delete?token=" + Token, JSONHelper.ToJSONString(menurequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>        
        public BasicResponse DeleteMenus(MenusDeleteRequest menurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/DeleteMenus?token=" + Token, JSONHelper.ToJSONString(menurequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<List<MenuInfo>> GetMenuList(MenuGetListRequest menurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/GetPageList?token=" + Token, JSONHelper.ToJSONString(menurequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<MenuInfo>>>(responseStr);
        }
        public BasicResponse<List<MenuInfo>> GetMenuList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<MenuInfo>>>(responseStr);
        }
        public BasicResponse<MenuInfo> GetMenuById(MenuGetRequest menurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/Get?token=" + Token, JSONHelper.ToJSONString(menurequest));
            return JSONHelper.ParseJSONString<BasicResponse<MenuInfo>>(responseStr);
        }
        /// <summary>
        /// 根据菜单编码得到菜单名称
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>        
        public BasicResponse<string> GetMenuNameByMenuCode(MenuGetByCOdeRequest menurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/GetMenuNameByMenuCode?token=" + Token, JSONHelper.ToJSONString(menurequest));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
        }
        /// <summary>
        /// 添加一个全新信息到菜单表并返回成功后的菜单对象(支持添加和更新)
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns> 
        public BasicResponse<MenuInfo> AddMenuEx(MenuAddRequest menurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/AddMenuEx?token=" + Token, JSONHelper.ToJSONString(menurequest));
            return JSONHelper.ParseJSONString<BasicResponse<MenuInfo>>(responseStr);
        }
        /// <summary>
        /// 启用菜单
        /// </summary>
        /// <param name="lstMenuDTO"></param>
        /// <returns></returns>        
        public BasicResponse EnablMenu(MenusUpdateRequest lstMenuDTO)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/EnablMenu?token=" + Token, JSONHelper.ToJSONString(lstMenuDTO));
            return JSONHelper.ParseJSONString<BasicResponse<MenuInfo>>(responseStr);
        }
        /// <summary>
        /// 禁用菜单
        /// </summary>
        /// <param name="lstMenuDTO"></param>
        /// <returns></returns>        
        public BasicResponse DisableMenu(MenusUpdateRequest lstMenuDTO)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Menu/DisableMenu?token=" + Token, JSONHelper.ToJSONString(lstMenuDTO));
            return JSONHelper.ParseJSONString<BasicResponse<MenuInfo>>(responseStr);
        }
    }
}

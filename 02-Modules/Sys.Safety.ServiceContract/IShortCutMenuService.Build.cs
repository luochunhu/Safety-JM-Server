using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.ShortCutMenu;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IShortCutMenuService
    {
        BasicResponse<ShortCutMenuInfo> AddShortCutMenu(ShortCutMenuAddRequest shortCutMenuRequest);
        BasicResponse<ShortCutMenuInfo> UpdateShortCutMenu(ShortCutMenuUpdateRequest shortCutMenuRequest);
        BasicResponse DeleteShortCutMenu(ShortCutMenuDeleteRequest shortCutMenuRequest);
        BasicResponse<List<ShortCutMenuInfo>> GetShortCutMenuList(ShortCutMenuGetListRequest shortCutMenuRequest);
        BasicResponse<ShortCutMenuInfo> GetShortCutMenuById(ShortCutMenuGetRequest shortCutMenuRequest);
        /// <summary>
        /// 根据用户名删除快捷菜单
        /// </summary>
        /// <param name="shortCutMenuRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> DeleteShortCutMenuByUserId(ShortCutMenuUserRequest shortCutMenuRequest);
        /// <summary>
        /// 批量添加快捷菜单
        /// </summary>
        /// <param name="shortCutMenuRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> BatchInsetShortCutMenu(ShortCutMenuBatchInsertRequest shortCutMenuRequest);
        /// <summary>
        /// 根据用户Id获取快捷菜单
        /// </summary>
        /// <param name="shortCutMenuRequest"></param>
        /// <returns></returns>
        BasicResponse<List<ShortCutMenuInfo>> GetShortCutMenuByUserId(ShortCutMenuUserRequest shortCutMenuRequest);
    }
}


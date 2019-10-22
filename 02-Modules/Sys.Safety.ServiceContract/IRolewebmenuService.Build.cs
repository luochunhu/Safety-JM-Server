using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Rolewebmenu;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRolewebmenuService
    {
        BasicResponse<RolewebmenuInfo> AddRolewebmenu(RolewebmenuAddRequest rolewebmenurequest);
        BasicResponse<RolewebmenuInfo> UpdateRolewebmenu(RolewebmenuUpdateRequest rolewebmenurequest);
        BasicResponse DeleteRolewebmenu(RolewebmenuDeleteRequest rolewebmenurequest);
        BasicResponse<List<RolewebmenuInfo>> GetRolewebmenuList(RolewebmenuGetListRequest rolewebmenurequest);
        BasicResponse<RolewebmenuInfo> GetRolewebmenuById(RolewebmenuGetRequest rolewebmenurequest);

        /// <summary>
        /// 根据角色更新角色菜单 先删除该角色已存在的关联菜单，再添加新的菜单
        /// </summary>
        /// <param name="rolewebmenurequest"></param>
        /// <returns></returns>
        BasicResponse<bool> UpdateWebRoleByRoleMenuInfo(RoleWebMenuUpdateByRoleRequest rolewebmenurequest);

        BasicResponse<List<RolewebmenuInfo>> GetRolewebmenuInfoByRole(RolewebmenuGetByRoleRequest rolewebmenurequest);
    }
}


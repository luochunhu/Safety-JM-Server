using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class RolewebmenuController : BasicApiController, IRolewebmenuService
    {
        private readonly IRolewebmenuService _rolewebmenuService;

        public RolewebmenuController()
        {
            _rolewebmenuService = ServiceFactory.Create<IRolewebmenuService>();
        }

        [HttpPost]
        [Route("v1/Rolewebmenu/AddRolewebmenu")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.RolewebmenuInfo> AddRolewebmenu(Sys.Safety.Request.Rolewebmenu.RolewebmenuAddRequest rolewebmenurequest)
        {
            return _rolewebmenuService.AddRolewebmenu(rolewebmenurequest);
        }

        [HttpPost]
        [Route("v1/Rolewebmenu/UpdateRolewebmenu")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.RolewebmenuInfo> UpdateRolewebmenu(Sys.Safety.Request.Rolewebmenu.RolewebmenuUpdateRequest rolewebmenurequest)
        {
            return _rolewebmenuService.UpdateRolewebmenu(rolewebmenurequest);
        }

        [HttpPost]
        [Route("v1/Rolewebmenu/DeleteRolewebmenu")]
        public Basic.Framework.Web.BasicResponse DeleteRolewebmenu(Sys.Safety.Request.Rolewebmenu.RolewebmenuDeleteRequest rolewebmenurequest)
        {
            return _rolewebmenuService.DeleteRolewebmenu(rolewebmenurequest);
        }

        [HttpPost]
        [Route("v1/Rolewebmenu/GetRolewebmenuList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.RolewebmenuInfo>> GetRolewebmenuList(Sys.Safety.Request.Rolewebmenu.RolewebmenuGetListRequest rolewebmenurequest)
        {
            return _rolewebmenuService.GetRolewebmenuList(rolewebmenurequest);
        }

        [HttpPost]
        [Route("v1/Rolewebmenu/GetRolewebmenuById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.RolewebmenuInfo> GetRolewebmenuById(Sys.Safety.Request.Rolewebmenu.RolewebmenuGetRequest rolewebmenurequest)
        {
            return _rolewebmenuService.GetRolewebmenuById(rolewebmenurequest);
        }

        [HttpPost]
        [Route("v1/Rolewebmenu/UpdateWebRoleByRoleMenuInfo")]
        public Basic.Framework.Web.BasicResponse<bool> UpdateWebRoleByRoleMenuInfo(Sys.Safety.Request.Rolewebmenu.RoleWebMenuUpdateByRoleRequest rolewebmenurequest)
        {
            return _rolewebmenuService.UpdateWebRoleByRoleMenuInfo(rolewebmenurequest);
        }

        [HttpPost]
        [Route("v1/Rolewebmenu/GetRolewebmenuInfoByRole")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.RolewebmenuInfo>> GetRolewebmenuInfoByRole(Sys.Safety.Request.Rolewebmenu.RolewebmenuGetByRoleRequest rolewebmenurequest)
        {
            return _rolewebmenuService.GetRolewebmenuInfoByRole(rolewebmenurequest);
        }
    }
}

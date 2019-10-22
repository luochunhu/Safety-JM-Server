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
using Sys.Safety.Request.Role;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 角色管理 WebApi接口
    /// </summary>
    public class RoleController : Basic.Framework.Web.WebApi.BasicApiController, IRoleService
    {
        static RoleController()
        {

        }
        IRoleService _roleService = ServiceFactory.Create<IRoleService>();
        
        [HttpPost]
        [Route("v1/Role/Add")]
        public BasicResponse<RoleInfo> AddRole(RoleAddRequest rolerequest)
        {
            return _roleService.AddRole(rolerequest);
        }
        [HttpPost]
        [Route("v1/Role/Update")]
        public BasicResponse<RoleInfo> UpdateRole(RoleUpdateRequest rolerequest)
        {
            return _roleService.UpdateRole(rolerequest);
        }
        [HttpPost]
        [Route("v1/Role/Delete")]
        public BasicResponse DeleteRole(RoleDeleteRequest rolerequest)
        {
            return _roleService.DeleteRole(rolerequest);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Role/DeleteRoles")]
        public BasicResponse DeleteRoles(RolesDeleteRequest rolerequest)
        {
            return _roleService.DeleteRoles(rolerequest);
        }
        [HttpPost]
        [Route("v1/Role/GetPageList")]
        public BasicResponse<List<RoleInfo>> GetRoleList(RoleGetListRequest rolerequest)
        {
            return _roleService.GetRoleList(rolerequest);
        }
        [HttpPost]
        [Route("v1/Role/GetAllList")]
        public BasicResponse<List<RoleInfo>> GetRoleList()
        {
            return _roleService.GetRoleList();
        }
        [HttpPost]
        [Route("v1/Role/Get")]
        public BasicResponse<RoleInfo> GetRoleById(RoleGetRequest rolerequest)
        {
            return _roleService.GetRoleById(rolerequest);
        }
        /// <summary>
        /// 添加一个全新信息到角色表并返回成功后的角色对象(支持新增和更新)
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Role/AddRoleEx")]
        public BasicResponse<RoleInfo> AddRoleEx(RoleAddRequest rolerequest)
        {
            return _roleService.AddRoleEx(rolerequest);
        }
        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Role/EnableRole")]
        public BasicResponse EnableRole(RolesRequest rolerequest)
        {
            return _roleService.EnableRole(rolerequest);
        }
        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Role/DisableRole")]
        public BasicResponse DisableRole(RolesRequest rolerequest)
        {
            return _roleService.DisableRole(rolerequest);
        }
    }
}

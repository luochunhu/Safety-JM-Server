using Basic.Framework.Common;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Role;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.UserRoleAuthorize
{
    public class RoleControllerProxy : BaseProxy, IRoleService
    {
        public BasicResponse<RoleInfo> AddRole(RoleAddRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/Add?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<RoleInfo>>(responseStr);
        }       
        public BasicResponse<RoleInfo> UpdateRole(RoleUpdateRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/Update?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<RoleInfo>>(responseStr);
        }       
        public BasicResponse DeleteRole(RoleDeleteRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/Delete?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>       
        public BasicResponse DeleteRoles(RolesDeleteRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/DeleteRoles?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }        
        public BasicResponse<List<RoleInfo>> GetRoleList(RoleGetListRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/GetPageList?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RoleInfo>>>(responseStr);
        }        
        public BasicResponse<List<RoleInfo>> GetRoleList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<RoleInfo>>>(responseStr);
        }        
        public BasicResponse<RoleInfo> GetRoleById(RoleGetRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/Get?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<RoleInfo>>(responseStr);
        }
        /// <summary>
        /// 添加一个全新信息到角色表并返回成功后的角色对象(支持新增和更新)
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>        
        public BasicResponse<RoleInfo> AddRoleEx(RoleAddRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/AddRoleEx?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<RoleInfo>>(responseStr);
        }
        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>        
        public BasicResponse EnableRole(RolesRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/EnableRole?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>        
        public BasicResponse DisableRole(RolesRequest rolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Role/DisableRole?token=" + Token, JSONHelper.ToJSONString(rolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}

using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Roleright;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.UserRoleAuthorize
{
    public class RolerightControllerProxy : BaseProxy, IRolerightService
    {

        public BasicResponse<RolerightInfo> AddRoleright(RolerightAddRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/Add?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RolerightInfo>>(responseStr);
        }
        public BasicResponse<RolerightInfo> UpdateRoleright(RolerightUpdateRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/Update?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RolerightInfo>>(responseStr);
        }
        public BasicResponse DeleteRoleright(RolerightDeleteRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/Delete?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据角色ID删除角色权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>        
        public BasicResponse DeleteRolerightByRoleId(RolerightDeleteByRoleIdRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/DeleteRolerightByRoleId?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据角色ID删除角色权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>        
        public BasicResponse DeleteRolerightByRightId(RolerightDeleteByRightIdRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/DeleteRolerightByRightId?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<List<RolerightInfo>> GetRolerightList(RolerightGetListRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/GetPageList?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RolerightInfo>>>(responseStr);
        }
        public BasicResponse<List<RolerightInfo>> GetRolerightList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<RolerightInfo>>>(responseStr);
        }
        public BasicResponse<RolerightInfo> GetRolerightById(RolerightGetRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/Get?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RolerightInfo>>(responseStr);
        }
        /// <summary>
        /// 新增角色权限点
        /// 批量增加权限点，增加前会删除原来角色下的所有权限点
        /// </summary>
        /// <param name="rolerightrequest">角色编码,角色的权限点集合</param>    
        public BasicResponse AddRoleRights(RolerightsAddRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/AddRoleRights?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据角色ID获取角色对应的权限信息
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>        
        public BasicResponse<List<RightInfo>> GetRightsByRoleId(RolerightGetByRoleIdRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/GetRightsByRoleId?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RightInfo>>>(responseStr);
        }
        /// <summary>
        /// 根据角色ID获取角色对应的权限关联信息
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>        
        public BasicResponse<List<RolerightInfo>> GetRolerightByRoleId(RolerightGetByRoleIdRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/GetRolerightByRoleId?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RolerightInfo>>>(responseStr);
        }
        /// <summary>
        /// 判断该角色是否分配权限
        /// </summary>
        /// <param name="rolerightrequest">角色编号</param>
        /// <returns></returns>        
        public BasicResponse CheckRoleIDExist(RolerightCheckExistByRoleIdRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/CheckRoleIDExist?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 为角色分配权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>        
        public BasicResponse ForRoleAssignmentRight(RolerightForRoleAssignmentRightRequest rolerightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Roleright/ForRoleAssignmentRight?token=" + Token, JSONHelper.ToJSONString(rolerightrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}

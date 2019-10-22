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
using Sys.Safety.Request.Roleright;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 角色与权限关联WebApi接口
    /// </summary>
    public class RolerightController : Basic.Framework.Web.WebApi.BasicApiController, IRolerightService
    {
        static RolerightController()
        {

        }
        IRolerightService _rolerightService = ServiceFactory.Create<IRolerightService>();
       
        [HttpPost]
        [Route("v1/Roleright/Add")]
        public BasicResponse<RolerightInfo> AddRoleright(RolerightAddRequest rolerightrequest)
        {
            return _rolerightService.AddRoleright(rolerightrequest);
        }
        [HttpPost]
        [Route("v1/Roleright/Update")]
        public BasicResponse<RolerightInfo> UpdateRoleright(RolerightUpdateRequest rolerightrequest)
        {
            return _rolerightService.UpdateRoleright(rolerightrequest);
        }
        [HttpPost]
        [Route("v1/Roleright/Delete")]
        public BasicResponse DeleteRoleright(RolerightDeleteRequest rolerightrequest)
        {
            return _rolerightService.DeleteRoleright(rolerightrequest);
        }
        /// <summary>
        /// 根据角色ID删除角色权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Roleright/DeleteRolerightByRoleId")]
        public BasicResponse DeleteRolerightByRoleId(RolerightDeleteByRoleIdRequest rolerightrequest)
        {
            return _rolerightService.DeleteRolerightByRoleId(rolerightrequest);
        }
        /// <summary>
        /// 根据角色ID删除角色权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Roleright/DeleteRolerightByRightId")]
        public BasicResponse DeleteRolerightByRightId(RolerightDeleteByRightIdRequest rolerightrequest)
        {
            return _rolerightService.DeleteRolerightByRightId(rolerightrequest);
        }
        [HttpPost]
        [Route("v1/Roleright/GetPageList")]
        public BasicResponse<List<RolerightInfo>> GetRolerightList(RolerightGetListRequest rolerightrequest)
        {
            return _rolerightService.GetRolerightList(rolerightrequest);
        }
        [HttpPost]
        [Route("v1/Roleright/GetAllList")]
        public BasicResponse<List<RolerightInfo>> GetRolerightList()
        {
            return _rolerightService.GetRolerightList();
        }
        [HttpPost]
        [Route("v1/Roleright/Get")]
        public BasicResponse<RolerightInfo> GetRolerightById(RolerightGetRequest rolerightrequest)
        {
            return _rolerightService.GetRolerightById(rolerightrequest);
        }
        /// <summary>
        /// 新增角色权限点
        /// 批量增加权限点，增加前会删除原来角色下的所有权限点
        /// </summary>
        /// <param name="rolerightrequest">角色编码,角色的权限点集合</param>      
        [HttpPost]
        [Route("v1/Roleright/AddRoleRights")]
        public BasicResponse AddRoleRights(RolerightsAddRequest rolerightrequest)
        {
            return _rolerightService.AddRoleRights(rolerightrequest);
        }
        /// <summary>
        /// 根据角色ID获取角色对应的权限信息
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Roleright/GetRightsByRoleId")]
        public BasicResponse<List<RightInfo>> GetRightsByRoleId(RolerightGetByRoleIdRequest rolerightrequest)
        {
            return _rolerightService.GetRightsByRoleId(rolerightrequest);
        }
        /// <summary>
        /// 根据角色ID获取角色对应的权限关联信息
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Roleright/GetRolerightByRoleId")]
        public BasicResponse<List<RolerightInfo>> GetRolerightByRoleId(RolerightGetByRoleIdRequest rolerightrequest)
        {
            return _rolerightService.GetRolerightByRoleId(rolerightrequest);
        }
        /// <summary>
        /// 判断该角色是否分配权限
        /// </summary>
        /// <param name="rolerightrequest">角色编号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Roleright/CheckRoleIDExist")]
        public BasicResponse CheckRoleIDExist(RolerightCheckExistByRoleIdRequest rolerightrequest)
        {
            return _rolerightService.CheckRoleIDExist(rolerightrequest);
        }
        /// <summary>
        /// 为角色分配权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Roleright/ForRoleAssignmentRight")]
        public BasicResponse ForRoleAssignmentRight(RolerightForRoleAssignmentRightRequest rolerightrequest)
        {
            return _rolerightService.ForRoleAssignmentRight(rolerightrequest);
        }
    }
}

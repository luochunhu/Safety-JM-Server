using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Roleright;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class RolerightService : IRolerightService
    {
        private IRolerightRepository _Repository;

        public RolerightService(IRolerightRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<RolerightInfo> AddRoleright(RolerightAddRequest rolerightrequest)
        {
            RolerightInfo roleRightDTO = rolerightrequest.RolerightInfo;
            //判断用户编号和权限编码是否存在
            if (CheckExist(roleRightDTO.RoleID, roleRightDTO.RightID))
            {
                ThrowException("AddRoleRight", new Exception("角色权限关系已存在，请重新绑定！"));
            }
            var _roleright = ObjectConverter.Copy<RolerightInfo, RolerightModel>(rolerightrequest.RolerightInfo);
            var resultroleright = _Repository.AddRoleright(_roleright);
            var rolerightresponse = new BasicResponse<RolerightInfo>();
            rolerightresponse.Data = ObjectConverter.Copy<RolerightModel, RolerightInfo>(resultroleright);
            return rolerightresponse;
        }
        public BasicResponse<RolerightInfo> UpdateRoleright(RolerightUpdateRequest rolerightrequest)
        {
            var _roleright = ObjectConverter.Copy<RolerightInfo, RolerightModel>(rolerightrequest.RolerightInfo);
            _Repository.UpdateRoleright(_roleright);
            var rolerightresponse = new BasicResponse<RolerightInfo>();
            rolerightresponse.Data = ObjectConverter.Copy<RolerightModel, RolerightInfo>(_roleright);
            return rolerightresponse;
        }
        public BasicResponse DeleteRoleright(RolerightDeleteRequest rolerightrequest)
        {
            _Repository.DeleteRoleright(rolerightrequest.Id);
            var rolerightresponse = new BasicResponse();
            return rolerightresponse;
        }
        /// <summary>
        /// 根据角色ID删除角色权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteRolerightByRoleId(RolerightDeleteByRoleIdRequest rolerightrequest)
        {
            _Repository.DeleteRolerightByRoleId(rolerightrequest.RoleId);
            var rolerightresponse = new BasicResponse();
            return rolerightresponse;
        }
        /// <summary>
        /// 根据角色ID删除角色权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteRolerightByRightId(RolerightDeleteByRightIdRequest rolerightrequest)
        {
            _Repository.DeleteRolerightByRightId(rolerightrequest.RightId);
            var rolerightresponse = new BasicResponse();
            return rolerightresponse;
        }
        public BasicResponse<List<RolerightInfo>> GetRolerightList(RolerightGetListRequest rolerightrequest)
        {
            var rolerightresponse = new BasicResponse<List<RolerightInfo>>();
            rolerightrequest.PagerInfo.PageIndex = rolerightrequest.PagerInfo.PageIndex - 1;
            if (rolerightrequest.PagerInfo.PageIndex < 0)
            {
                rolerightrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var rolerightModelLists = _Repository.GetRolerightList(rolerightrequest.PagerInfo.PageIndex, rolerightrequest.PagerInfo.PageSize, out rowcount);
            var rolerightInfoLists = new List<RolerightInfo>();
            foreach (var item in rolerightModelLists)
            {
                var RolerightInfo = ObjectConverter.Copy<RolerightModel, RolerightInfo>(item);
                rolerightInfoLists.Add(RolerightInfo);
            }
            rolerightresponse.Data = rolerightInfoLists;
            return rolerightresponse;
        }
        public BasicResponse<List<RolerightInfo>> GetRolerightList()
        {
            var rolerightresponse = new BasicResponse<List<RolerightInfo>>();
            var rolerightModelLists = _Repository.GetRolerightList();
            var rolerightInfoLists = new List<RolerightInfo>();
            foreach (var item in rolerightModelLists)
            {
                var RolerightInfo = ObjectConverter.Copy<RolerightModel, RolerightInfo>(item);
                rolerightInfoLists.Add(RolerightInfo);
            }
            rolerightresponse.Data = rolerightInfoLists;
            return rolerightresponse;
        }
        public BasicResponse<RolerightInfo> GetRolerightById(RolerightGetRequest rolerightrequest)
        {
            var result = _Repository.GetRolerightById(rolerightrequest.Id);
            var rolerightInfo = ObjectConverter.Copy<RolerightModel, RolerightInfo>(result);
            var rolerightresponse = new BasicResponse<RolerightInfo>();
            rolerightresponse.Data = rolerightInfo;
            return rolerightresponse;
        }

        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("RoleRightService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        /// <summary>
        /// 新增角色权限点
        /// 批量增加权限点，增加前会删除原来角色下的所有权限点
        /// </summary>
        /// <param name="roleId">角色编码</param>
        /// <param name="rightList">角色的权限点集合</param>      
        public BasicResponse AddRoleRights(RolerightsAddRequest rolerightrequest)
        {
            BasicResponse Result = new BasicResponse();
            long roleId = long.Parse(rolerightrequest.roleId);
            List<RolerightInfo> rolerightList = rolerightrequest.RolerightInfo;
            try
            {
                if (rolerightList == null || roleId <= 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数错误";
                    return Result;
                }
                //先删除原有权限点
                _Repository.DeleteRolerightByRoleId(roleId.ToString());

                foreach (RolerightInfo dto in rolerightList)
                {
                    if (long.Parse(dto.RoleID) <= 0 || long.Parse(dto.RightID) <= 0 || long.Parse(dto.RoleID) != roleId)
                    {
                        //业务检查
                        //如果DTO对象的角色ID和权限ID为零，则跳过此条记录
                        //如果DTO对象的角色ID和传入参数的ID不一样，则跳过此条记录
                        continue;
                    }
                    dto.RoleRightID = IdHelper.CreateLongId().ToString();
                    var _request = ObjectConverter.Copy<RolerightInfo, RolerightModel>(dto);
                    var resultmenu = _Repository.AddRoleright(_request);
                }
            }
            catch (System.Exception ex)
            {
                ThrowException("AddRoleRights", ex);
            }
            return Result;
        }
        /// <summary>
        /// 根据角色ID获取角色对应的权限信息
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        public BasicResponse<List<RightInfo>> GetRightsByRoleId(RolerightGetByRoleIdRequest rolerightrequest)
        {
            var result = _Repository.GetRightsByRoleId(rolerightrequest.RoleId);
            var rolerightInfo = ObjectConverter.CopyList<RightModel, RightInfo>(result).ToList();
            var rolerightresponse = new BasicResponse<List<RightInfo>>();
            rolerightresponse.Data = rolerightInfo;
            return rolerightresponse;
        }
        /// <summary>
        /// 根据角色ID获取角色对应的权限关联信息
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        public BasicResponse<List<RolerightInfo>> GetRolerightByRoleId(RolerightGetByRoleIdRequest rolerightrequest)
        {
            var result = _Repository.GetRolerightByRoleId(rolerightrequest.RoleId);
            var rolerightInfo = ObjectConverter.Copy<List<RolerightModel>, List<RolerightInfo>>(result);
            var rolerightresponse = new BasicResponse<List<RolerightInfo>>();
            rolerightresponse.Data = rolerightInfo;
            return rolerightresponse;
        }
        /// <summary>
        /// 判断该角色是否分配权限
        /// </summary>
        /// <param name="roleID">角色编号</param>
        /// <returns></returns>
        public BasicResponse CheckRoleIDExist(RolerightCheckExistByRoleIdRequest rolerightrequest)
        {
            BasicResponse Result = new BasicResponse();
            List<RolerightModel> roleRightDTO = null;
            try
            {
                if (long.Parse(rolerightrequest.RoleId) < 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                List<RolerightModel> RolerightList = _Repository.GetRolerightList();
                roleRightDTO = RolerightList.FindAll(a => a.RoleID == rolerightrequest.RoleId);
                if (roleRightDTO.Count < 1)
                {
                    Result.Code = 2;
                    Result.Message = "未查询到数据";
                }
            }
            catch
            {
                Result.Code = 3;
                Result.Message = "查询异常";
            }
            return Result;
        }
        /// <summary>
        /// 为角色分配权限
        /// </summary>       
        public BasicResponse ForRoleAssignmentRight(RolerightForRoleAssignmentRightRequest rolerightrequest)
        {
            BasicResponse Result = new BasicResponse();

            int i = 0;
            List<RolerightInfo> lstRoleRightDTO = new List<RolerightInfo>();
            RolerightInfo tempRoleRightDTO = new RolerightInfo();
            try
            {
                if (long.Parse(rolerightrequest.RoleId) < 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                if (rolerightrequest.lstRightID == null)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                //首先将角色权限表中已分配的权限删除掉                
                _Repository.DeleteRolerightByRoleId(rolerightrequest.RoleId);
                //根据权限列表将角色-权限进行分配
                if (rolerightrequest.lstRightID.Count > 0)
                {
                    tempRoleRightDTO = new RolerightInfo();
                    for (i = 0; i < rolerightrequest.lstRightID.Count; i++)
                    {
                        tempRoleRightDTO.RoleRightID = IdHelper.CreateLongId().ToString();
                        tempRoleRightDTO.RoleID = rolerightrequest.RoleId;
                        tempRoleRightDTO.RightID = rolerightrequest.lstRightID[i];
                        var _roleright = ObjectConverter.Copy<RolerightInfo, RolerightModel>(tempRoleRightDTO);
                        _Repository.AddRoleright(_roleright);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ThrowException("ForRoleAssignmentRight", ex);
            }
            return Result;
        }


        /// <summary>
        /// 同一个角色编号和权限编号只能插入一次，判断指定的权限编号和角色编号是否存在
        /// </summary>
        /// <param name="userID">角色编号</param>
        /// <param name="roleID">权限编号</param>
        /// <returns></returns>
        private bool CheckExist(string roleID, string rightID)
        {
            bool isExist = false;
            RolerightModel roleRightDTO = null;
            try
            {
                if (long.Parse(roleID) < 0)
                {
                    isExist = false;
                    return isExist;
                }
                if (long.Parse(rightID) < 0)
                {
                    isExist = false;
                    return isExist;
                }
                List<RolerightModel> RolerightList = _Repository.GetRolerightList();
                roleRightDTO = RolerightList.Find(a => a.RoleID == roleID.ToString() && a.RightID == rightID.ToString());
                if (roleRightDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(roleRightDTO.RoleRightID) > 0)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch
            {
                isExist = false;
            }
            return isExist;
        }
        /// <summary>
        /// 根据角色编号得到权限编号列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private List<string> GetRightID(string roleID)
        {
            List<RolerightModel> roleRightDTO = new List<RolerightModel>();
            List<string> lstRightID = new List<string>();
            int i = 0;
            try
            {
                if (string.IsNullOrEmpty(roleID))
                {
                    return lstRightID;
                }
                List<RolerightModel> RolerightList = _Repository.GetRolerightList();
                roleRightDTO = RolerightList.FindAll(a => a.RoleID == roleID.ToString());
                if (roleRightDTO.Count > 0)
                {
                    for (i = 0; i < roleRightDTO.Count; i++)
                    {
                        lstRightID.Add(roleRightDTO[i].RightID.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                ThrowException("GetRightID", ex);
            }
            return lstRightID;
        }
    }
}



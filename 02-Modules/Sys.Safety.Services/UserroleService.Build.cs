using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Userrole;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class UserroleService : IUserroleService
    {
        private IUserroleRepository _Repository;
        private IRoleRepository _RoleRepository;

        public UserroleService(IUserroleRepository _Repository, IRoleRepository _RoleRepository)
        {
            this._Repository = _Repository;
            this._RoleRepository = _RoleRepository;
        }
        public void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("UserRoleService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        public BasicResponse<UserroleInfo> AddUserrole(UserroleAddRequest userrolerequest)
        {
            UserroleInfo userRoleDTO = userrolerequest.UserroleInfo;
            if (userRoleDTO.CreateTime != null)
            {
                userRoleDTO.CreateTime = Convert.ToDateTime(userRoleDTO.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //判断用户编号和角色编码是否存在
            if (CheckExist(userRoleDTO.UserID, userRoleDTO.RoleID))
            {
                //校验权限名是否重复              
                ThrowException("SaveEnumCode", new Exception("用户角色关系已存在，请重新绑定！"));
            }
            var _userrole = ObjectConverter.Copy<UserroleInfo, UserroleModel>(userrolerequest.UserroleInfo);
            var resultuserrole = _Repository.AddUserrole(_userrole);
            var userroleresponse = new BasicResponse<UserroleInfo>();
            userroleresponse.Data = ObjectConverter.Copy<UserroleModel, UserroleInfo>(resultuserrole);
            return userroleresponse;
        }
        public BasicResponse<UserroleInfo> UpdateUserrole(UserroleUpdateRequest userrolerequest)
        {
            UserroleInfo userRoleDTO = userrolerequest.UserroleInfo;
            if (userRoleDTO.CreateTime != null)
            {
                userRoleDTO.CreateTime = Convert.ToDateTime(userRoleDTO.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //判断用户编号和角色编码是否存在
            if (CheckExist(userRoleDTO.UserID, userRoleDTO.RoleID))
            {
                //校验权限名是否重复               
                ThrowException("SaveEnumCode", new Exception("用户角色关系不存在，请绑定后再进行修改！"));
            }
            var _userrole = ObjectConverter.Copy<UserroleInfo, UserroleModel>(userrolerequest.UserroleInfo);
            _Repository.UpdateUserrole(_userrole);
            var userroleresponse = new BasicResponse<UserroleInfo>();
            userroleresponse.Data = ObjectConverter.Copy<UserroleModel, UserroleInfo>(_userrole);
            return userroleresponse;
        }
        public BasicResponse DeleteUserrole(UserroleDeleteRequest userrolerequest)
        {
            UserroleModel userRoleDTO = _Repository.GetUserroleById(userrolerequest.Id);
            //判断用户编号和角色编码是否存在
            if (userRoleDTO == null)
            {
                //校验权限名是否重复               
                ThrowException("DeleteUserRole", new Exception("用户角色关系不存在，请绑定后再进行删除！"));
            }
            _Repository.DeleteUserrole(userrolerequest.Id);
            var userroleresponse = new BasicResponse();
            return userroleresponse;
        }
        /// <summary>
        /// 根据角色ID删除用户与角色的关联关系
        /// </summary>
        /// <param name="userrolerequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteUserroleByRoleId(UserroleDeleteByRoleIdRequest userrolerequest)
        {
            _Repository.DeleteUserroleByRoleId(userrolerequest.RoleId);
            var userroleresponse = new BasicResponse();
            return userroleresponse;
        }
        /// <summary>
        /// 根据用户ID删除用户与角色的关联关系
        /// </summary>
        /// <param name="userrolerequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteUserroleByUserId(UserroleDeleteByUserIdRequest userrolerequest)
        {
            _Repository.DeleteUserroleByUserId(userrolerequest.UserId);
            var userroleresponse = new BasicResponse();
            return userroleresponse;
        }
        public BasicResponse<List<UserroleInfo>> GetUserroleList(UserroleGetListRequest userrolerequest)
        {
            var userroleresponse = new BasicResponse<List<UserroleInfo>>();
            userrolerequest.PagerInfo.PageIndex = userrolerequest.PagerInfo.PageIndex - 1;
            if (userrolerequest.PagerInfo.PageIndex < 0)
            {
                userrolerequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var userroleModelLists = _Repository.GetUserroleList(userrolerequest.PagerInfo.PageIndex, userrolerequest.PagerInfo.PageSize, out rowcount);
            var userroleInfoLists = new List<UserroleInfo>();
            foreach (var item in userroleModelLists)
            {
                var UserroleInfo = ObjectConverter.Copy<UserroleModel, UserroleInfo>(item);
                userroleInfoLists.Add(UserroleInfo);
            }
            userroleresponse.Data = userroleInfoLists;
            return userroleresponse;
        }
        public BasicResponse<List<UserroleInfo>> GetUserroleList()
        {
            var userroleresponse = new BasicResponse<List<UserroleInfo>>();            
            var userroleModelLists = _Repository.GetUserroleList();
            var userroleInfoLists = new List<UserroleInfo>();
            foreach (var item in userroleModelLists)
            {
                var UserroleInfo = ObjectConverter.Copy<UserroleModel, UserroleInfo>(item);
                userroleInfoLists.Add(UserroleInfo);
            }
            userroleresponse.Data = userroleInfoLists;
            return userroleresponse;
        }
        public BasicResponse<UserroleInfo> GetUserroleById(UserroleGetRequest userrolerequest)
        {
            var result = _Repository.GetUserroleById(userrolerequest.Id);
            var userroleInfo = ObjectConverter.Copy<UserroleModel, UserroleInfo>(result);
            var userroleresponse = new BasicResponse<UserroleInfo>();
            userroleresponse.Data = userroleInfo;
            return userroleresponse;
        }
        /// <summary>
        /// 批量设置用户角色
        /// 注意此接口是先删除原有，再做新增操作
        /// </summary>        
        public BasicResponse AddUserRoles(UserrolesAddRequest userrolerequest)
        {
            BasicResponse Result = new BasicResponse();
            string userId = userrolerequest.userId;
            List<UserroleInfo> userRoleList = userrolerequest.userRoleList;
            try
            {
                if (userRoleList == null || string.IsNullOrEmpty(userId))
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                //先删除原有权限点
                _Repository.DeleteUserroleByUserId(userId);
                foreach (UserroleInfo dto in userRoleList)
                {
                    dto.UserRoleID = IdHelper.CreateLongId().ToString();
                    if (string.IsNullOrEmpty(dto.RoleID) || string.IsNullOrEmpty(dto.UserID) || dto.UserID != userId)
                    {
                        //业务检查
                        //如果DTO对象的角色ID和用户ID为空，则跳过此条记录
                        //如果DTO对象的用户ID和传入参数的ID不一样，则跳过此条记录
                        continue;
                    }
                    var _userrole = ObjectConverter.Copy<UserroleInfo, UserroleModel>(dto);
                    _Repository.AddUserrole(_userrole);
                }
            }
            catch (System.Exception ex)
            {
                ThrowException("AddUserRoles", ex);
            }
            return Result;
        }
        /// <summary>
        /// 根据用户ID获取用户角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户角色列表</returns>        
        public BasicResponse<List<UserroleInfo>> GetUserRoleByUserId(UserroleGetByUserIdRequest userrolerequest)
        {
            BasicResponse<List<UserroleInfo>> Result = new BasicResponse<List<UserroleInfo>>();
            List<UserroleModel> Userrole = _Repository.GetUserroleByUserId(userrolerequest.UserId);
            Result.Data = ObjectConverter.CopyList<UserroleModel, UserroleInfo>(Userrole).ToList();
            return Result;
        }
        /// <summary>
        /// 判断该用户是否分配角色
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns></returns>
        public BasicResponse CheckUserIDExist(UserroleGetCheckUserIDExistRequest userrolerequest)
        {
            BasicResponse Result = new BasicResponse();
            try
            {
                if (string.IsNullOrEmpty(userrolerequest.UserId))
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                List<string> UserRoleList = GetRoleID(userrolerequest.UserId);
                if (UserRoleList.Count < 1)
                {
                    Result.Code = 2;
                    Result.Message = "未分配角色";
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
        /// 为用户分配角色
        /// </summary>       
        public BasicResponse ForUserAssignmentRole(UserroleForUserAssignmentRoleRequest userrolerequest)
        {
            BasicResponse Result = new BasicResponse();
            int i = 0;
            List<UserroleInfo> lstUserRoleDTO = new List<UserroleInfo>();
            UserroleInfo tempUserRoleDTO = new UserroleInfo();
            try
            {
                if (string.IsNullOrEmpty(userrolerequest.UserId ))
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                if (userrolerequest.lstRoleID == null)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                //首先将用户角色表中已分配的角色删除掉
                _Repository.DeleteUserroleByUserId(userrolerequest.UserId);
                //根据角色列表将用户-角色进行分配
                if (userrolerequest.lstRoleID.Count > 0)
                {                   
                    for (i = 0; i < userrolerequest.lstRoleID.Count; i++)
                    {
                        tempUserRoleDTO = new UserroleInfo();
                        tempUserRoleDTO.UserRoleID = IdHelper.CreateLongId().ToString();
                        tempUserRoleDTO.UserID = userrolerequest.UserId;
                        tempUserRoleDTO.RoleID = userrolerequest.lstRoleID[i];
                        var _userrole = ObjectConverter.Copy<UserroleInfo, UserroleModel>(tempUserRoleDTO);
                        _Repository.AddUserrole(_userrole);
                    }                  
                }
            }
            catch (System.Exception ex)
            {
                ThrowException("ForUserAssignmentRole", ex);
            }
            return Result;
        }
        /// <summary>
        /// 根据用户编号获取所有的该用户所拥有的角色对象
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns></returns>
        public BasicResponse<List<RoleInfo>> GetRoleListByUserId(UserroleGetByUserIdRequest userrolerequest)
        {
            BasicResponse<List<RoleInfo>> Result = new BasicResponse<List<RoleInfo>>();

            List<UserroleModel> userroleList = _Repository.GetUserroleByUserId(userrolerequest.UserId);
            List<RoleModel> rolemodelList = _RoleRepository.GetRoleList();
            List<RoleInfo> roleinfoList = ObjectConverter.CopyList<RoleModel, RoleInfo>(rolemodelList).ToList();
            foreach (UserroleModel userrole in userroleList) {
                RoleInfo temprole = roleinfoList.Find(a => a.RoleID == userrole.RoleID);
                if (temprole != null)
                {
                    Result.Data.Add(temprole);
                }
            }
            return Result;
        }


        /// <summary>
        /// 同一个用户编号和角色编号只能插入一次，判断指定的用户编号和角色编号是否存在
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <param name="roleID">角色编号</param>
        /// <returns></returns>
        private bool CheckExist(string userID, string roleID)
        {
            bool isExist = false;
            UserroleModel userRoleDTO = null;
            try
            {
                if (long.Parse(userID) < 0)
                {
                    isExist = false;
                    return isExist;
                }
                if (long.Parse(roleID) < 0)
                {
                    isExist = false;
                    return isExist;
                }
                List<UserroleModel> RoleList = _Repository.GetUserroleList();
                userRoleDTO = RoleList.Find(a => a.UserID == userID && a.RoleID == roleID);


                if (userRoleDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(userRoleDTO.UserRoleID) > 0)
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
        /// 根据用户编号得到角色编号列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private List<string> GetRoleID(string userID)
        {
            List<UserroleModel> userRoleDTO = new List<UserroleModel>();
            List<string> lstRoleID = new List<string>();
            int i = 0;
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return lstRoleID;
                }
                List<UserroleModel> RoleList = _Repository.GetUserroleList();
                userRoleDTO = RoleList.FindAll(a => a.UserID == userID);
                if (userRoleDTO.Count > 0)
                {
                    for (i = 0; i < userRoleDTO.Count; i++)
                    {
                        lstRoleID.Add(userRoleDTO[i].RoleID.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                ThrowException("GetRoleID", ex);
            }
            return lstRoleID;
        }
    }
}



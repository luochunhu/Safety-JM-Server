using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Role;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class RoleService : IRoleService
    {
        private IRoleRepository _Repository;
        private IUserroleRepository _UserroleRepository;
        private IRolerightRepository _RolerightRepository;

        public void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("RoleService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        public RoleService(IRoleRepository _Repository, IUserroleRepository _UserroleRepository, IRolerightRepository _RolerightRepository)
        {
            this._Repository = _Repository;
            this._UserroleRepository = _UserroleRepository;
            this._RolerightRepository = _RolerightRepository;
        }
        public BasicResponse<RoleInfo> AddRole(RoleAddRequest rolerequest)
        {
            RoleInfo roleDTO = rolerequest.RoleInfo;
            if (roleDTO.CreateTime == new DateTime(1, 1, 1, 0, 0, 0))
            {
                roleDTO.CreateTime = DateTime.Now;
            }
            if (roleDTO.CreateTime != null)
            {
                roleDTO.CreateTime = Convert.ToDateTime(roleDTO.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (string.IsNullOrEmpty(roleDTO.RoleCode))
            {                
                ThrowException("AddRole", new Exception("角色编码不能为空，请重新输入！"));
            }
            if (string.IsNullOrEmpty(roleDTO.RoleName))
            {                
                ThrowException("AddRole", new Exception("角色名称不能为空，请重新输入！"));
            }
            if (CheckRoleNameExist(roleDTO.RoleName))
            {
                //校验权限名是否重复               
                ThrowException("AddRole", new Exception(String.Format("角色名:{0} 已存在，请重新输入！", roleDTO.RoleName)));
            }
            if (CheckRoleCodeExist(roleDTO.RoleCode))
            {
                //校验权限编码是否重复               
                ThrowException("AddRole", new Exception(String.Format("角色编码:{0} 已存在，请重新输入！", roleDTO.RoleCode)));
            }
            var _role = ObjectConverter.Copy<RoleInfo, RoleModel>(roleDTO);
            var resultrole = _Repository.AddRole(_role);
            var roleresponse = new BasicResponse<RoleInfo>();
            roleresponse.Data = ObjectConverter.Copy<RoleModel, RoleInfo>(resultrole);
            return roleresponse;
        }
        public BasicResponse<RoleInfo> UpdateRole(RoleUpdateRequest rolerequest)
        {
            RoleInfo roleDTO = rolerequest.RoleInfo;
            if (string.IsNullOrEmpty( roleDTO.RoleID))
            {               
                ThrowException("UpdateRole", new Exception("角色编号不能为空！"));
            }
            if (string.IsNullOrEmpty(roleDTO.RoleName))
            {               
                ThrowException("UpdateRole", new Exception("角色名称不能为空！"));
            }
            var _role = ObjectConverter.Copy<RoleInfo, RoleModel>(rolerequest.RoleInfo);
            _Repository.UpdateRole(_role);
            var roleresponse = new BasicResponse<RoleInfo>();
            roleresponse.Data = ObjectConverter.Copy<RoleModel, RoleInfo>(_role);
            return roleresponse;
        }
        public BasicResponse DeleteRole(RoleDeleteRequest rolerequest)
        {
            RoleModel roleDTO = _Repository.GetRoleById(rolerequest.Id);
            if (roleDTO.RoleCode == "SuperMan")
            {               
                ThrowException("DeleteRole", new Exception("SuperMan为超级管理员角色不能进行删除操作！"));
            }
            if (string.IsNullOrEmpty(roleDTO.RoleID.ToString()))
            {               
                ThrowException("DeleteRole", new Exception("角色编号不能为空！"));
            }
            _Repository.DeleteRole(rolerequest.Id);

            //删除角色时将角色关联的用户关系一并删除。
            _UserroleRepository.DeleteUserroleByRoleId(roleDTO.RoleID);
            //删除角色时将角色关联的权限关系一并删除。
            _RolerightRepository.DeleteRolerightByRoleId(roleDTO.RoleID);

            var roleresponse = new BasicResponse();
            return roleresponse;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteRoles(RolesDeleteRequest rolerequest)
        {
            foreach (string Id in rolerequest.IdList)
            {
                RoleModel roleDTO = _Repository.GetRoleById(Id);                                               
                
                if (roleDTO.RoleCode == "SuperMan")
                {
                    ThrowException("DeleteRole", new Exception("SuperMan为超级管理员角色不能进行删除操作！"));
                }
                if (string.IsNullOrEmpty(roleDTO.RoleID.ToString()))
                {
                    ThrowException("DeleteRole", new Exception("角色编号不能为空！"));
                }
                _Repository.DeleteRole(Id);

                //删除角色时将角色关联的用户关系一并删除。
                _UserroleRepository.DeleteUserroleByRoleId(roleDTO.RoleID);
                //删除角色时将角色关联的权限关系一并删除。
                _RolerightRepository.DeleteRolerightByRoleId(roleDTO.RoleID);
            }

            var roleresponse = new BasicResponse();
            return roleresponse;
        }
        public BasicResponse<List<RoleInfo>> GetRoleList(RoleGetListRequest rolerequest)
        {
            var roleresponse = new BasicResponse<List<RoleInfo>>();
            rolerequest.PagerInfo.PageIndex = rolerequest.PagerInfo.PageIndex - 1;
            if (rolerequest.PagerInfo.PageIndex < 0)
            {
                rolerequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var roleModelLists = _Repository.GetRoleList(rolerequest.PagerInfo.PageIndex, rolerequest.PagerInfo.PageSize, out rowcount);
            var roleInfoLists = new List<RoleInfo>();
            foreach (var item in roleModelLists)
            {
                var RoleInfo = ObjectConverter.Copy<RoleModel, RoleInfo>(item);
                roleInfoLists.Add(RoleInfo);
            }
            roleresponse.Data = roleInfoLists;
            return roleresponse;
        }
        public BasicResponse<List<RoleInfo>> GetRoleList()
        {
            var roleresponse = new BasicResponse<List<RoleInfo>>();            
            var roleModelLists = _Repository.GetRoleList();
            var roleInfoLists = new List<RoleInfo>();
            foreach (var item in roleModelLists)
            {
                var RoleInfo = ObjectConverter.Copy<RoleModel, RoleInfo>(item);
                roleInfoLists.Add(RoleInfo);
            }
            roleresponse.Data = roleInfoLists;
            return roleresponse;
        }
        public BasicResponse<RoleInfo> GetRoleById(RoleGetRequest rolerequest)
        {
            var result = _Repository.GetRoleById(rolerequest.Id);
            var roleInfo = ObjectConverter.Copy<RoleModel, RoleInfo>(result);
            var roleresponse = new BasicResponse<RoleInfo>();
            roleresponse.Data = roleInfo;
            return roleresponse;
        }

        /// <summary>
        /// 添加一个全新信息到角色表并返回成功后的角色对象(支持新增和更新)
        /// </summary>
        /// <param name="roleDTO"></param>
        /// <returns></returns>
        public BasicResponse<RoleInfo> AddRoleEx(RoleAddRequest rolerequest)
        {
            BasicResponse<RoleInfo> Result = new BasicResponse<RoleInfo>();
            RoleInfo roleDTO = rolerequest.RoleInfo;
            try
            {
                long ID = 0;
                if (roleDTO.CreateTime == new DateTime(1, 1, 1, 0, 0, 0))
                {
                    roleDTO.CreateTime = DateTime.Now;
                }
                if (roleDTO.CreateTime != null)
                {
                    roleDTO.CreateTime = Convert.ToDateTime(roleDTO.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (string.IsNullOrEmpty(roleDTO.RoleCode))
                {
                    ThrowException("AddRoleEx", new Exception("角色编码不能为空，请重新输入！"));
                }
                if (string.IsNullOrEmpty(roleDTO.RoleName))
                {
                    ThrowException("AddRoleEx", new Exception("角色名称不能为空，请重新输入！"));
                }
                if (roleDTO.InfoState == InfoState.AddNew)
                {
                    if (CheckRoleNameExist(roleDTO.RoleName))
                    {
                        //校验权限名是否重复                       
                        ThrowException("AddRoleEx", new Exception(String.Format("角色名:{0} 已存在，请重新输入！", roleDTO.RoleName)));
                    }
                    if (CheckRoleCodeExist(roleDTO.RoleCode))
                    {
                        //校验权限编码是否重复                       
                        ThrowException("AddRoleEx", new Exception(String.Format("角色编码:{0} 已存在，请重新输入！", roleDTO.RoleCode)));
                    }

                    ID = IdHelper.CreateLongId();
                    roleDTO.RoleID = ID.ToString();
                    var _request = ObjectConverter.Copy<RoleInfo, RoleModel>(roleDTO);
                    var resultmenu = _Repository.AddRole(_request);
                    Result.Data= ObjectConverter.Copy<RoleModel, RoleInfo>(resultmenu);
                }
                else
                {
                    var _request = ObjectConverter.Copy<RoleInfo, RoleModel>(roleDTO);
                    _Repository.Update(_request);
                    Result.Data = roleDTO;
                }               
            }
            catch (System.Exception ex)
            {
                Result.Code = 1;
                Result.Message = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        public BasicResponse EnableRole(RolesRequest rolerequest)
        {
            BasicResponse Result = new BasicResponse();
            List<RoleInfo> lstRoleDTO = rolerequest.RoleInfo;
            try
            {
                if (lstRoleDTO.Count <= 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                foreach (RoleInfo tempRoleDTO in lstRoleDTO)
                {
                    tempRoleDTO.RoleFlag = 1;
                    var _request = ObjectConverter.Copy<RoleInfo, RoleModel>(tempRoleDTO);
                    _Repository.Update(_request);
                }               
            }
            catch
            {
                Result.Code = 2;
                Result.Message = "操作失败";
            }
            return Result;
        }
        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        public BasicResponse DisableRole(RolesRequest rolerequest)
        {
            BasicResponse Result = new BasicResponse();
            List<RoleInfo> lstRoleDTO = rolerequest.RoleInfo;
            try
            {
                if (lstRoleDTO.Count <= 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数异常";
                    return Result;
                }
                foreach (RoleInfo tempRoleDTO in lstRoleDTO)
                {
                    tempRoleDTO.RoleFlag = 0;
                    var _request = ObjectConverter.Copy<RoleInfo, RoleModel>(tempRoleDTO);
                    _Repository.Update(_request);
                }
            }
            catch
            {
                Result.Code = 2;
                Result.Message = "操作失败";
            }
            return Result;
        }

        /// <summary>
        /// 根据传入的角色名判断当前角色是否已存在
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns>true:存在  false:不存在</returns>
        private bool CheckRoleNameExist(string roleName)
        {
            bool isExist = false;           
            RoleModel roleDTO = null;
            try
            {
                List<RoleModel> RoleList = _Repository.GetRoleList();
                roleDTO = RoleList.Find(a => a.RoleName == roleName);
                if (roleDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(roleDTO.RoleID) > 0)
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
        /// 判断角色编码是否存在
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns></returns>
        private bool CheckRoleCodeExist(string roleCode)
        {
            bool isExist = false;          
            RoleModel roleDTO = null;
            try
            {
                List<RoleModel> RoleList = _Repository.GetRoleList();
                roleDTO = RoleList.Find(a => a.RoleCode == roleCode);
                if (roleDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(roleDTO.RoleID) > 0)
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
    }
}



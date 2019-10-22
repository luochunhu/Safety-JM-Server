using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Rolewebmenu;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class RolewebmenuService : IRolewebmenuService
    {
        private IRolewebmenuRepository _Repository;

        public RolewebmenuService(IRolewebmenuRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<RolewebmenuInfo> AddRolewebmenu(RolewebmenuAddRequest rolewebmenurequest)
        {
            var _rolewebmenu = ObjectConverter.Copy<RolewebmenuInfo, RolewebmenuModel>(rolewebmenurequest.RolewebmenuInfo);
            var resultrolewebmenu = _Repository.AddRolewebmenu(_rolewebmenu);
            var rolewebmenuresponse = new BasicResponse<RolewebmenuInfo>();
            rolewebmenuresponse.Data = ObjectConverter.Copy<RolewebmenuModel, RolewebmenuInfo>(resultrolewebmenu);
            return rolewebmenuresponse;
        }
        public BasicResponse<RolewebmenuInfo> UpdateRolewebmenu(RolewebmenuUpdateRequest rolewebmenurequest)
        {
            var _rolewebmenu = ObjectConverter.Copy<RolewebmenuInfo, RolewebmenuModel>(rolewebmenurequest.RolewebmenuInfo);
            _Repository.UpdateRolewebmenu(_rolewebmenu);
            var rolewebmenuresponse = new BasicResponse<RolewebmenuInfo>();
            rolewebmenuresponse.Data = ObjectConverter.Copy<RolewebmenuModel, RolewebmenuInfo>(_rolewebmenu);
            return rolewebmenuresponse;
        }
        public BasicResponse DeleteRolewebmenu(RolewebmenuDeleteRequest rolewebmenurequest)
        {
            _Repository.DeleteRolewebmenu(rolewebmenurequest.Id);
            var rolewebmenuresponse = new BasicResponse();
            return rolewebmenuresponse;
        }
        public BasicResponse<List<RolewebmenuInfo>> GetRolewebmenuList(RolewebmenuGetListRequest rolewebmenurequest)
        {
            var rolewebmenuresponse = new BasicResponse<List<RolewebmenuInfo>>();
            rolewebmenurequest.PagerInfo.PageIndex = rolewebmenurequest.PagerInfo.PageIndex - 1;
            if (rolewebmenurequest.PagerInfo.PageIndex < 0)
            {
                rolewebmenurequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var rolewebmenuModelLists = _Repository.GetRolewebmenuList(rolewebmenurequest.PagerInfo.PageIndex, rolewebmenurequest.PagerInfo.PageSize, out rowcount);
            var rolewebmenuInfoLists = new List<RolewebmenuInfo>();
            foreach (var item in rolewebmenuModelLists)
            {
                var RolewebmenuInfo = ObjectConverter.Copy<RolewebmenuModel, RolewebmenuInfo>(item);
                rolewebmenuInfoLists.Add(RolewebmenuInfo);
            }
            rolewebmenuresponse.Data = rolewebmenuInfoLists;
            return rolewebmenuresponse;
        }
        public BasicResponse<RolewebmenuInfo> GetRolewebmenuById(RolewebmenuGetRequest rolewebmenurequest)
        {
            var result = _Repository.GetRolewebmenuById(rolewebmenurequest.Id);
            var rolewebmenuInfo = ObjectConverter.Copy<RolewebmenuModel, RolewebmenuInfo>(result);
            var rolewebmenuresponse = new BasicResponse<RolewebmenuInfo>();
            rolewebmenuresponse.Data = rolewebmenuInfo;
            return rolewebmenuresponse;
        }


        public BasicResponse<bool> UpdateWebRoleByRoleMenuInfo(RoleWebMenuUpdateByRoleRequest rolewebmenurequest)
        {
            BasicResponse<bool> response = new BasicResponse<bool>();

            try
            {
                var items = _Repository.Datas.Where(o => o.RoleID == rolewebmenurequest.RoleId);
                _Repository.Delete(items);

                var webmenumodels = ObjectConverter.CopyList<RolewebmenuInfo, RolewebmenuModel>(rolewebmenurequest.RolewebMenuInfos);

                _Repository.Insert(webmenumodels);

                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                LogHelper.Error("角色菜单更新失败！" + ex.Message);
                response.Data = false;
                return response;
            }
        }


        public BasicResponse<List<RolewebmenuInfo>> GetRolewebmenuInfoByRole(RolewebmenuGetByRoleRequest rolewebmenurequest)
        {
            BasicResponse<List<RolewebmenuInfo>> response = new BasicResponse<List<RolewebmenuInfo>>();

            try
            {
                var items = _Repository.Datas.Where(o => o.RoleID == rolewebmenurequest.RoleId).ToList();
                var webmenuinfos = ObjectConverter.CopyList<RolewebmenuModel, RolewebmenuInfo>(items).ToList();
                response.Data = webmenuinfos;
                return response;
            }
            catch (Exception ex)
            {
                LogHelper.Error("根据角色获取角色菜单！" + ex.Message);
                return response;
            }
        }
    }
}



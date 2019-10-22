using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Rolewebmenuauth;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class RolewebmenuauthService:IRolewebmenuauthService
    {
		private IRolewebmenuauthRepository _Repository;

		public RolewebmenuauthService(IRolewebmenuauthRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<RolewebmenuauthInfo> AddRolewebmenuauth(RolewebmenuauthAddRequest rolewebmenuauthrequest)
        {
            var _rolewebmenuauth = ObjectConverter.Copy<RolewebmenuauthInfo, RolewebmenuauthModel>(rolewebmenuauthrequest.RolewebmenuauthInfo);
            var resultrolewebmenuauth = _Repository.AddRolewebmenuauth(_rolewebmenuauth);
            var rolewebmenuauthresponse = new BasicResponse<RolewebmenuauthInfo>();
            rolewebmenuauthresponse.Data = ObjectConverter.Copy<RolewebmenuauthModel, RolewebmenuauthInfo>(resultrolewebmenuauth);
            return rolewebmenuauthresponse;
        }
				public BasicResponse<RolewebmenuauthInfo> UpdateRolewebmenuauth(RolewebmenuauthUpdateRequest rolewebmenuauthrequest)
        {
            var _rolewebmenuauth = ObjectConverter.Copy<RolewebmenuauthInfo, RolewebmenuauthModel>(rolewebmenuauthrequest.RolewebmenuauthInfo);
            _Repository.UpdateRolewebmenuauth(_rolewebmenuauth);
            var rolewebmenuauthresponse = new BasicResponse<RolewebmenuauthInfo>();
            rolewebmenuauthresponse.Data = ObjectConverter.Copy<RolewebmenuauthModel, RolewebmenuauthInfo>(_rolewebmenuauth);  
            return rolewebmenuauthresponse;
        }
				public BasicResponse DeleteRolewebmenuauth(RolewebmenuauthDeleteRequest rolewebmenuauthrequest)
        {
            _Repository.DeleteRolewebmenuauth(rolewebmenuauthrequest.Id);
            var rolewebmenuauthresponse = new BasicResponse();            
            return rolewebmenuauthresponse;
        }
				public BasicResponse<List<RolewebmenuauthInfo>> GetRolewebmenuauthList(RolewebmenuauthGetListRequest rolewebmenuauthrequest)
        {
            var rolewebmenuauthresponse = new BasicResponse<List<RolewebmenuauthInfo>>();
            rolewebmenuauthrequest.PagerInfo.PageIndex = rolewebmenuauthrequest.PagerInfo.PageIndex - 1;
            if (rolewebmenuauthrequest.PagerInfo.PageIndex < 0)
            {
                rolewebmenuauthrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var rolewebmenuauthModelLists = _Repository.GetRolewebmenuauthList(rolewebmenuauthrequest.PagerInfo.PageIndex, rolewebmenuauthrequest.PagerInfo.PageSize, out rowcount);
            var rolewebmenuauthInfoLists = new List<RolewebmenuauthInfo>();
            foreach (var item in rolewebmenuauthModelLists)
            {
                var RolewebmenuauthInfo = ObjectConverter.Copy<RolewebmenuauthModel, RolewebmenuauthInfo>(item);
                rolewebmenuauthInfoLists.Add(RolewebmenuauthInfo);
            }
            rolewebmenuauthresponse.Data = rolewebmenuauthInfoLists;
            return rolewebmenuauthresponse;
        }
				public BasicResponse<RolewebmenuauthInfo> GetRolewebmenuauthById(RolewebmenuauthGetRequest rolewebmenuauthrequest)
        {
            var result = _Repository.GetRolewebmenuauthById(rolewebmenuauthrequest.Id);
            var rolewebmenuauthInfo = ObjectConverter.Copy<RolewebmenuauthModel, RolewebmenuauthInfo>(result);
            var rolewebmenuauthresponse = new BasicResponse<RolewebmenuauthInfo>();
            rolewebmenuauthresponse.Data = rolewebmenuauthInfo;
            return rolewebmenuauthresponse;
        }
	}
}



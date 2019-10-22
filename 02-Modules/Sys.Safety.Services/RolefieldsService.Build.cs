using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Rolefields;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class RolefieldsService:IRolefieldsService
    {
		private IRolefieldsRepository _Repository;

		public RolefieldsService(IRolefieldsRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<RolefieldsInfo> AddRolefields(RolefieldsAddRequest rolefieldsrequest)
        {
            var _rolefields = ObjectConverter.Copy<RolefieldsInfo, RolefieldsModel>(rolefieldsrequest.RolefieldsInfo);
            var resultrolefields = _Repository.AddRolefields(_rolefields);
            var rolefieldsresponse = new BasicResponse<RolefieldsInfo>();
            rolefieldsresponse.Data = ObjectConverter.Copy<RolefieldsModel, RolefieldsInfo>(resultrolefields);
            return rolefieldsresponse;
        }
				public BasicResponse<RolefieldsInfo> UpdateRolefields(RolefieldsUpdateRequest rolefieldsrequest)
        {
            var _rolefields = ObjectConverter.Copy<RolefieldsInfo, RolefieldsModel>(rolefieldsrequest.RolefieldsInfo);
            _Repository.UpdateRolefields(_rolefields);
            var rolefieldsresponse = new BasicResponse<RolefieldsInfo>();
            rolefieldsresponse.Data = ObjectConverter.Copy<RolefieldsModel, RolefieldsInfo>(_rolefields);  
            return rolefieldsresponse;
        }
				public BasicResponse DeleteRolefields(RolefieldsDeleteRequest rolefieldsrequest)
        {
            _Repository.DeleteRolefields(rolefieldsrequest.Id);
            var rolefieldsresponse = new BasicResponse();            
            return rolefieldsresponse;
        }
				public BasicResponse<List<RolefieldsInfo>> GetRolefieldsList(RolefieldsGetListRequest rolefieldsrequest)
        {
            var rolefieldsresponse = new BasicResponse<List<RolefieldsInfo>>();
            rolefieldsrequest.PagerInfo.PageIndex = rolefieldsrequest.PagerInfo.PageIndex - 1;
            if (rolefieldsrequest.PagerInfo.PageIndex < 0)
            {
                rolefieldsrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var rolefieldsModelLists = _Repository.GetRolefieldsList(rolefieldsrequest.PagerInfo.PageIndex, rolefieldsrequest.PagerInfo.PageSize, out rowcount);
            var rolefieldsInfoLists = new List<RolefieldsInfo>();
            foreach (var item in rolefieldsModelLists)
            {
                var RolefieldsInfo = ObjectConverter.Copy<RolefieldsModel, RolefieldsInfo>(item);
                rolefieldsInfoLists.Add(RolefieldsInfo);
            }
            rolefieldsresponse.Data = rolefieldsInfoLists;
            return rolefieldsresponse;
        }
				public BasicResponse<RolefieldsInfo> GetRolefieldsById(RolefieldsGetRequest rolefieldsrequest)
        {
            var result = _Repository.GetRolefieldsById(rolefieldsrequest.Id);
            var rolefieldsInfo = ObjectConverter.Copy<RolefieldsModel, RolefieldsInfo>(result);
            var rolefieldsresponse = new BasicResponse<RolefieldsInfo>();
            rolefieldsresponse.Data = rolefieldsInfo;
            return rolefieldsresponse;
        }
	}
}



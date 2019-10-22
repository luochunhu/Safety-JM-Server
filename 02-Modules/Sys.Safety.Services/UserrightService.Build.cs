using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Userright;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class UserrightService:IUserrightService
    {
		private IUserrightRepository _Repository;

		public UserrightService(IUserrightRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<UserrightInfo> AddUserright(UserrightAddRequest userrightrequest)
        {
            var _userright = ObjectConverter.Copy<UserrightInfo, UserrightModel>(userrightrequest.UserrightInfo);
            var resultuserright = _Repository.AddUserright(_userright);
            var userrightresponse = new BasicResponse<UserrightInfo>();
            userrightresponse.Data = ObjectConverter.Copy<UserrightModel, UserrightInfo>(resultuserright);
            return userrightresponse;
        }
				public BasicResponse<UserrightInfo> UpdateUserright(UserrightUpdateRequest userrightrequest)
        {
            var _userright = ObjectConverter.Copy<UserrightInfo, UserrightModel>(userrightrequest.UserrightInfo);
            _Repository.UpdateUserright(_userright);
            var userrightresponse = new BasicResponse<UserrightInfo>();
            userrightresponse.Data = ObjectConverter.Copy<UserrightModel, UserrightInfo>(_userright);  
            return userrightresponse;
        }
				public BasicResponse DeleteUserright(UserrightDeleteRequest userrightrequest)
        {
            _Repository.DeleteUserright(userrightrequest.Id);
            var userrightresponse = new BasicResponse();            
            return userrightresponse;
        }
				public BasicResponse<List<UserrightInfo>> GetUserrightList(UserrightGetListRequest userrightrequest)
        {
            var userrightresponse = new BasicResponse<List<UserrightInfo>>();
            userrightrequest.PagerInfo.PageIndex = userrightrequest.PagerInfo.PageIndex - 1;
            if (userrightrequest.PagerInfo.PageIndex < 0)
            {
                userrightrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var userrightModelLists = _Repository.GetUserrightList(userrightrequest.PagerInfo.PageIndex, userrightrequest.PagerInfo.PageSize, out rowcount);
            var userrightInfoLists = new List<UserrightInfo>();
            foreach (var item in userrightModelLists)
            {
                var UserrightInfo = ObjectConverter.Copy<UserrightModel, UserrightInfo>(item);
                userrightInfoLists.Add(UserrightInfo);
            }
            userrightresponse.Data = userrightInfoLists;
            return userrightresponse;
        }
				public BasicResponse<UserrightInfo> GetUserrightById(UserrightGetRequest userrightrequest)
        {
            var result = _Repository.GetUserrightById(userrightrequest.Id);
            var userrightInfo = ObjectConverter.Copy<UserrightModel, UserrightInfo>(result);
            var userrightresponse = new BasicResponse<UserrightInfo>();
            userrightresponse.Data = userrightInfo;
            return userrightresponse;
        }
	}
}



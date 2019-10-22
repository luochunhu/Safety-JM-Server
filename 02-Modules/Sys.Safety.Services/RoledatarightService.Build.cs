using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Roledataright;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class RoledatarightService:IRoledatarightService
    {
		private IRoledatarightRepository _Repository;

		public RoledatarightService(IRoledatarightRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<RoledatarightInfo> AddRoledataright(RoledatarightAddRequest roledatarightrequest)
        {
            var _roledataright = ObjectConverter.Copy<RoledatarightInfo, RoledatarightModel>(roledatarightrequest.RoledatarightInfo);
            var resultroledataright = _Repository.AddRoledataright(_roledataright);
            var roledatarightresponse = new BasicResponse<RoledatarightInfo>();
            roledatarightresponse.Data = ObjectConverter.Copy<RoledatarightModel, RoledatarightInfo>(resultroledataright);
            return roledatarightresponse;
        }
				public BasicResponse<RoledatarightInfo> UpdateRoledataright(RoledatarightUpdateRequest roledatarightrequest)
        {
            var _roledataright = ObjectConverter.Copy<RoledatarightInfo, RoledatarightModel>(roledatarightrequest.RoledatarightInfo);
            _Repository.UpdateRoledataright(_roledataright);
            var roledatarightresponse = new BasicResponse<RoledatarightInfo>();
            roledatarightresponse.Data = ObjectConverter.Copy<RoledatarightModel, RoledatarightInfo>(_roledataright);  
            return roledatarightresponse;
        }
				public BasicResponse DeleteRoledataright(RoledatarightDeleteRequest roledatarightrequest)
        {
            _Repository.DeleteRoledataright(roledatarightrequest.Id);
            var roledatarightresponse = new BasicResponse();            
            return roledatarightresponse;
        }
				public BasicResponse<List<RoledatarightInfo>> GetRoledatarightList(RoledatarightGetListRequest roledatarightrequest)
        {
            var roledatarightresponse = new BasicResponse<List<RoledatarightInfo>>();
            roledatarightrequest.PagerInfo.PageIndex = roledatarightrequest.PagerInfo.PageIndex - 1;
            if (roledatarightrequest.PagerInfo.PageIndex < 0)
            {
                roledatarightrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var roledatarightModelLists = _Repository.GetRoledatarightList(roledatarightrequest.PagerInfo.PageIndex, roledatarightrequest.PagerInfo.PageSize, out rowcount);
            var roledatarightInfoLists = new List<RoledatarightInfo>();
            foreach (var item in roledatarightModelLists)
            {
                var RoledatarightInfo = ObjectConverter.Copy<RoledatarightModel, RoledatarightInfo>(item);
                roledatarightInfoLists.Add(RoledatarightInfo);
            }
            roledatarightresponse.Data = roledatarightInfoLists;
            return roledatarightresponse;
        }
				public BasicResponse<RoledatarightInfo> GetRoledatarightById(RoledatarightGetRequest roledatarightrequest)
        {
            var result = _Repository.GetRoledatarightById(roledatarightrequest.Id);
            var roledatarightInfo = ObjectConverter.Copy<RoledatarightModel, RoledatarightInfo>(result);
            var roledatarightresponse = new BasicResponse<RoledatarightInfo>();
            roledatarightresponse.Data = roledatarightInfo;
            return roledatarightresponse;
        }
	}
}



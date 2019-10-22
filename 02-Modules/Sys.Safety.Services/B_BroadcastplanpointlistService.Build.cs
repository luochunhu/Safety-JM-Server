using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Broadcastplanpointlist;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class B_BroadcastplanpointlistService:IB_BroadcastplanpointlistService
    {
		private IB_BroadcastplanpointlistRepository _Repository;

		public B_BroadcastplanpointlistService(IB_BroadcastplanpointlistRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<B_BroadcastplanpointlistInfo> AddB_Broadcastplanpointlist(B_BroadcastplanpointlistAddRequest b_BroadcastplanpointlistRequest)
        {
            var _b_Broadcastplanpointlist = ObjectConverter.Copy<B_BroadcastplanpointlistInfo, B_BroadcastplanpointlistModel>(b_BroadcastplanpointlistRequest.B_BroadcastplanpointlistInfo);
            var resultb_Broadcastplanpointlist = _Repository.AddB_Broadcastplanpointlist(_b_Broadcastplanpointlist);
            var b_Broadcastplanpointlistresponse = new BasicResponse<B_BroadcastplanpointlistInfo>();
            b_Broadcastplanpointlistresponse.Data = ObjectConverter.Copy<B_BroadcastplanpointlistModel, B_BroadcastplanpointlistInfo>(resultb_Broadcastplanpointlist);
            return b_Broadcastplanpointlistresponse;
        }
				public BasicResponse<B_BroadcastplanpointlistInfo> UpdateB_Broadcastplanpointlist(B_BroadcastplanpointlistUpdateRequest b_BroadcastplanpointlistRequest)
        {
            var _b_Broadcastplanpointlist = ObjectConverter.Copy<B_BroadcastplanpointlistInfo, B_BroadcastplanpointlistModel>(b_BroadcastplanpointlistRequest.B_BroadcastplanpointlistInfo);
            _Repository.UpdateB_Broadcastplanpointlist(_b_Broadcastplanpointlist);
            var b_Broadcastplanpointlistresponse = new BasicResponse<B_BroadcastplanpointlistInfo>();
            b_Broadcastplanpointlistresponse.Data = ObjectConverter.Copy<B_BroadcastplanpointlistModel, B_BroadcastplanpointlistInfo>(_b_Broadcastplanpointlist);  
            return b_Broadcastplanpointlistresponse;
        }
				public BasicResponse DeleteB_Broadcastplanpointlist(B_BroadcastplanpointlistDeleteRequest b_BroadcastplanpointlistRequest)
        {
            _Repository.DeleteB_Broadcastplanpointlist(b_BroadcastplanpointlistRequest.Id);
            var b_Broadcastplanpointlistresponse = new BasicResponse();            
            return b_Broadcastplanpointlistresponse;
        }
				public BasicResponse<List<B_BroadcastplanpointlistInfo>> GetB_BroadcastplanpointlistList(B_BroadcastplanpointlistGetListRequest b_BroadcastplanpointlistRequest)
        {
            var b_Broadcastplanpointlistresponse = new BasicResponse<List<B_BroadcastplanpointlistInfo>>();
            b_BroadcastplanpointlistRequest.PagerInfo.PageIndex = b_BroadcastplanpointlistRequest.PagerInfo.PageIndex - 1;
            if (b_BroadcastplanpointlistRequest.PagerInfo.PageIndex < 0)
            {
                b_BroadcastplanpointlistRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var b_BroadcastplanpointlistModelLists = _Repository.GetB_BroadcastplanpointlistList(b_BroadcastplanpointlistRequest.PagerInfo.PageIndex, b_BroadcastplanpointlistRequest.PagerInfo.PageSize, out rowcount);
            var b_BroadcastplanpointlistInfoLists = new List<B_BroadcastplanpointlistInfo>();
            foreach (var item in b_BroadcastplanpointlistModelLists)
            {
                var B_BroadcastplanpointlistInfo = ObjectConverter.Copy<B_BroadcastplanpointlistModel, B_BroadcastplanpointlistInfo>(item);
                b_BroadcastplanpointlistInfoLists.Add(B_BroadcastplanpointlistInfo);
            }
            b_Broadcastplanpointlistresponse.Data = b_BroadcastplanpointlistInfoLists;
            return b_Broadcastplanpointlistresponse;
        }
				public BasicResponse<B_BroadcastplanpointlistInfo> GetB_BroadcastplanpointlistById(B_BroadcastplanpointlistGetRequest b_BroadcastplanpointlistRequest)
        {
            var result = _Repository.GetB_BroadcastplanpointlistById(b_BroadcastplanpointlistRequest.Id);
            var b_BroadcastplanpointlistInfo = ObjectConverter.Copy<B_BroadcastplanpointlistModel, B_BroadcastplanpointlistInfo>(result);
            var b_Broadcastplanpointlistresponse = new BasicResponse<B_BroadcastplanpointlistInfo>();
            b_Broadcastplanpointlistresponse.Data = b_BroadcastplanpointlistInfo;
            return b_Broadcastplanpointlistresponse;
        }
	}
}



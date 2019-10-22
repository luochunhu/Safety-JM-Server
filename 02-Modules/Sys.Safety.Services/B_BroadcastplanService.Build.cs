using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Broadcastplan;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class B_BroadcastplanService:IB_BroadcastplanService
    {
		private IB_BroadcastplanRepository _Repository;

		public B_BroadcastplanService(IB_BroadcastplanRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<B_BroadcastplanInfo> AddB_Broadcastplan(B_BroadcastplanAddRequest b_BroadcastplanRequest)
        {
            var _b_Broadcastplan = ObjectConverter.Copy<B_BroadcastplanInfo, B_BroadcastplanModel>(b_BroadcastplanRequest.B_BroadcastplanInfo);
            var resultb_Broadcastplan = _Repository.AddB_Broadcastplan(_b_Broadcastplan);
            var b_Broadcastplanresponse = new BasicResponse<B_BroadcastplanInfo>();
            b_Broadcastplanresponse.Data = ObjectConverter.Copy<B_BroadcastplanModel, B_BroadcastplanInfo>(resultb_Broadcastplan);
            return b_Broadcastplanresponse;
        }
				public BasicResponse<B_BroadcastplanInfo> UpdateB_Broadcastplan(B_BroadcastplanUpdateRequest b_BroadcastplanRequest)
        {
            var _b_Broadcastplan = ObjectConverter.Copy<B_BroadcastplanInfo, B_BroadcastplanModel>(b_BroadcastplanRequest.B_BroadcastplanInfo);
            _Repository.UpdateB_Broadcastplan(_b_Broadcastplan);
            var b_Broadcastplanresponse = new BasicResponse<B_BroadcastplanInfo>();
            b_Broadcastplanresponse.Data = ObjectConverter.Copy<B_BroadcastplanModel, B_BroadcastplanInfo>(_b_Broadcastplan);  
            return b_Broadcastplanresponse;
        }
				public BasicResponse DeleteB_Broadcastplan(B_BroadcastplanDeleteRequest b_BroadcastplanRequest)
        {
            _Repository.DeleteB_Broadcastplan(b_BroadcastplanRequest.Id);
            var b_Broadcastplanresponse = new BasicResponse();            
            return b_Broadcastplanresponse;
        }
				public BasicResponse<List<B_BroadcastplanInfo>> GetB_BroadcastplanList(B_BroadcastplanGetListRequest b_BroadcastplanRequest)
        {
            var b_Broadcastplanresponse = new BasicResponse<List<B_BroadcastplanInfo>>();
            b_BroadcastplanRequest.PagerInfo.PageIndex = b_BroadcastplanRequest.PagerInfo.PageIndex - 1;
            if (b_BroadcastplanRequest.PagerInfo.PageIndex < 0)
            {
                b_BroadcastplanRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var b_BroadcastplanModelLists = _Repository.GetB_BroadcastplanList(b_BroadcastplanRequest.PagerInfo.PageIndex, b_BroadcastplanRequest.PagerInfo.PageSize, out rowcount);
            var b_BroadcastplanInfoLists = new List<B_BroadcastplanInfo>();
            foreach (var item in b_BroadcastplanModelLists)
            {
                var B_BroadcastplanInfo = ObjectConverter.Copy<B_BroadcastplanModel, B_BroadcastplanInfo>(item);
                b_BroadcastplanInfoLists.Add(B_BroadcastplanInfo);
            }
            b_Broadcastplanresponse.Data = b_BroadcastplanInfoLists;
            return b_Broadcastplanresponse;
        }
				public BasicResponse<B_BroadcastplanInfo> GetB_BroadcastplanById(B_BroadcastplanGetRequest b_BroadcastplanRequest)
        {
            var result = _Repository.GetB_BroadcastplanById(b_BroadcastplanRequest.Id);
            var b_BroadcastplanInfo = ObjectConverter.Copy<B_BroadcastplanModel, B_BroadcastplanInfo>(result);
            var b_Broadcastplanresponse = new BasicResponse<B_BroadcastplanInfo>();
            b_Broadcastplanresponse.Data = b_BroadcastplanInfo;
            return b_Broadcastplanresponse;
        }
	}
}



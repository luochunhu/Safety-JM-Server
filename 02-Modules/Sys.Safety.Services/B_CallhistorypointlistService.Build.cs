using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Callhistorypointlist;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class B_CallhistorypointlistService:IB_CallhistorypointlistService
    {
		private IB_CallhistorypointlistRepository _Repository;

		public B_CallhistorypointlistService(IB_CallhistorypointlistRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<B_CallhistorypointlistInfo> AddB_Callhistorypointlist(B_CallhistorypointlistAddRequest b_CallhistorypointlistRequest)
        {
            var _b_Callhistorypointlist = ObjectConverter.Copy<B_CallhistorypointlistInfo, B_CallhistorypointlistModel>(b_CallhistorypointlistRequest.B_CallhistorypointlistInfo);
            var resultb_Callhistorypointlist = _Repository.AddB_Callhistorypointlist(_b_Callhistorypointlist);
            var b_Callhistorypointlistresponse = new BasicResponse<B_CallhistorypointlistInfo>();
            b_Callhistorypointlistresponse.Data = ObjectConverter.Copy<B_CallhistorypointlistModel, B_CallhistorypointlistInfo>(resultb_Callhistorypointlist);
            return b_Callhistorypointlistresponse;
        }
				public BasicResponse<B_CallhistorypointlistInfo> UpdateB_Callhistorypointlist(B_CallhistorypointlistUpdateRequest b_CallhistorypointlistRequest)
        {
            var _b_Callhistorypointlist = ObjectConverter.Copy<B_CallhistorypointlistInfo, B_CallhistorypointlistModel>(b_CallhistorypointlistRequest.B_CallhistorypointlistInfo);
            _Repository.UpdateB_Callhistorypointlist(_b_Callhistorypointlist);
            var b_Callhistorypointlistresponse = new BasicResponse<B_CallhistorypointlistInfo>();
            b_Callhistorypointlistresponse.Data = ObjectConverter.Copy<B_CallhistorypointlistModel, B_CallhistorypointlistInfo>(_b_Callhistorypointlist);  
            return b_Callhistorypointlistresponse;
        }
				public BasicResponse DeleteB_Callhistorypointlist(B_CallhistorypointlistDeleteRequest b_CallhistorypointlistRequest)
        {
            _Repository.DeleteB_Callhistorypointlist(b_CallhistorypointlistRequest.Id);
            var b_Callhistorypointlistresponse = new BasicResponse();            
            return b_Callhistorypointlistresponse;
        }
				public BasicResponse<List<B_CallhistorypointlistInfo>> GetB_CallhistorypointlistList(B_CallhistorypointlistGetListRequest b_CallhistorypointlistRequest)
        {
            var b_Callhistorypointlistresponse = new BasicResponse<List<B_CallhistorypointlistInfo>>();
            b_CallhistorypointlistRequest.PagerInfo.PageIndex = b_CallhistorypointlistRequest.PagerInfo.PageIndex - 1;
            if (b_CallhistorypointlistRequest.PagerInfo.PageIndex < 0)
            {
                b_CallhistorypointlistRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var b_CallhistorypointlistModelLists = _Repository.GetB_CallhistorypointlistList(b_CallhistorypointlistRequest.PagerInfo.PageIndex, b_CallhistorypointlistRequest.PagerInfo.PageSize, out rowcount);
            var b_CallhistorypointlistInfoLists = new List<B_CallhistorypointlistInfo>();
            foreach (var item in b_CallhistorypointlistModelLists)
            {
                var B_CallhistorypointlistInfo = ObjectConverter.Copy<B_CallhistorypointlistModel, B_CallhistorypointlistInfo>(item);
                b_CallhistorypointlistInfoLists.Add(B_CallhistorypointlistInfo);
            }
            b_Callhistorypointlistresponse.Data = b_CallhistorypointlistInfoLists;
            return b_Callhistorypointlistresponse;
        }
				public BasicResponse<B_CallhistorypointlistInfo> GetB_CallhistorypointlistById(B_CallhistorypointlistGetRequest b_CallhistorypointlistRequest)
        {
            var result = _Repository.GetB_CallhistorypointlistById(b_CallhistorypointlistRequest.Id);
            var b_CallhistorypointlistInfo = ObjectConverter.Copy<B_CallhistorypointlistModel, B_CallhistorypointlistInfo>(result);
            var b_Callhistorypointlistresponse = new BasicResponse<B_CallhistorypointlistInfo>();
            b_Callhistorypointlistresponse.Data = b_CallhistorypointlistInfo;
            return b_Callhistorypointlistresponse;
        }
	}
}



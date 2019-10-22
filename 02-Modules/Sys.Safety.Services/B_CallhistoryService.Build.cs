using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Callhistory;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class B_CallhistoryService:IB_CallhistoryService
    {
		private IB_CallhistoryRepository _Repository;

		public B_CallhistoryService(IB_CallhistoryRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<B_CallhistoryInfo> AddB_Callhistory(B_CallhistoryAddRequest b_CallhistoryRequest)
        {
            var _b_Callhistory = ObjectConverter.Copy<B_CallhistoryInfo, B_CallhistoryModel>(b_CallhistoryRequest.B_CallhistoryInfo);
            var resultb_Callhistory = _Repository.AddB_Callhistory(_b_Callhistory);
            var b_Callhistoryresponse = new BasicResponse<B_CallhistoryInfo>();
            b_Callhistoryresponse.Data = ObjectConverter.Copy<B_CallhistoryModel, B_CallhistoryInfo>(resultb_Callhistory);
            return b_Callhistoryresponse;
        }
				public BasicResponse<B_CallhistoryInfo> UpdateB_Callhistory(B_CallhistoryUpdateRequest b_CallhistoryRequest)
        {
            var _b_Callhistory = ObjectConverter.Copy<B_CallhistoryInfo, B_CallhistoryModel>(b_CallhistoryRequest.B_CallhistoryInfo);
            _Repository.UpdateB_Callhistory(_b_Callhistory);
            var b_Callhistoryresponse = new BasicResponse<B_CallhistoryInfo>();
            b_Callhistoryresponse.Data = ObjectConverter.Copy<B_CallhistoryModel, B_CallhistoryInfo>(_b_Callhistory);  
            return b_Callhistoryresponse;
        }
				public BasicResponse DeleteB_Callhistory(B_CallhistoryDeleteRequest b_CallhistoryRequest)
        {
            _Repository.DeleteB_Callhistory(b_CallhistoryRequest.Id);
            var b_Callhistoryresponse = new BasicResponse();            
            return b_Callhistoryresponse;
        }
				public BasicResponse<List<B_CallhistoryInfo>> GetB_CallhistoryList(B_CallhistoryGetListRequest b_CallhistoryRequest)
        {
            var b_Callhistoryresponse = new BasicResponse<List<B_CallhistoryInfo>>();
            b_CallhistoryRequest.PagerInfo.PageIndex = b_CallhistoryRequest.PagerInfo.PageIndex - 1;
            if (b_CallhistoryRequest.PagerInfo.PageIndex < 0)
            {
                b_CallhistoryRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var b_CallhistoryModelLists = _Repository.GetB_CallhistoryList(b_CallhistoryRequest.PagerInfo.PageIndex, b_CallhistoryRequest.PagerInfo.PageSize, out rowcount);
            var b_CallhistoryInfoLists = new List<B_CallhistoryInfo>();
            foreach (var item in b_CallhistoryModelLists)
            {
                var B_CallhistoryInfo = ObjectConverter.Copy<B_CallhistoryModel, B_CallhistoryInfo>(item);
                b_CallhistoryInfoLists.Add(B_CallhistoryInfo);
            }
            b_Callhistoryresponse.Data = b_CallhistoryInfoLists;
            return b_Callhistoryresponse;
        }
				public BasicResponse<B_CallhistoryInfo> GetB_CallhistoryById(B_CallhistoryGetRequest b_CallhistoryRequest)
        {
            var result = _Repository.GetB_CallhistoryById(b_CallhistoryRequest.Id);
            var b_CallhistoryInfo = ObjectConverter.Copy<B_CallhistoryModel, B_CallhistoryInfo>(result);
            var b_Callhistoryresponse = new BasicResponse<B_CallhistoryInfo>();
            b_Callhistoryresponse.Data = b_CallhistoryInfo;
            return b_Callhistoryresponse;
        }
	}
}



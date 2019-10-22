using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Musicfiles;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class B_MusicfilesService:IB_MusicfilesService
    {
		private IB_MusicfilesRepository _Repository;

		public B_MusicfilesService(IB_MusicfilesRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<B_MusicfilesInfo> AddB_Musicfiles(B_MusicfilesAddRequest b_MusicfilesRequest)
        {
            var _b_Musicfiles = ObjectConverter.Copy<B_MusicfilesInfo, B_MusicfilesModel>(b_MusicfilesRequest.B_MusicfilesInfo);
            var resultb_Musicfiles = _Repository.AddB_Musicfiles(_b_Musicfiles);
            var b_Musicfilesresponse = new BasicResponse<B_MusicfilesInfo>();
            b_Musicfilesresponse.Data = ObjectConverter.Copy<B_MusicfilesModel, B_MusicfilesInfo>(resultb_Musicfiles);
            return b_Musicfilesresponse;
        }
				public BasicResponse<B_MusicfilesInfo> UpdateB_Musicfiles(B_MusicfilesUpdateRequest b_MusicfilesRequest)
        {
            var _b_Musicfiles = ObjectConverter.Copy<B_MusicfilesInfo, B_MusicfilesModel>(b_MusicfilesRequest.B_MusicfilesInfo);
            _Repository.UpdateB_Musicfiles(_b_Musicfiles);
            var b_Musicfilesresponse = new BasicResponse<B_MusicfilesInfo>();
            b_Musicfilesresponse.Data = ObjectConverter.Copy<B_MusicfilesModel, B_MusicfilesInfo>(_b_Musicfiles);  
            return b_Musicfilesresponse;
        }
				public BasicResponse DeleteB_Musicfiles(B_MusicfilesDeleteRequest b_MusicfilesRequest)
        {
            _Repository.DeleteB_Musicfiles(b_MusicfilesRequest.Id);
            var b_Musicfilesresponse = new BasicResponse();            
            return b_Musicfilesresponse;
        }
				public BasicResponse<List<B_MusicfilesInfo>> GetB_MusicfilesList(B_MusicfilesGetListRequest b_MusicfilesRequest)
        {
            var b_Musicfilesresponse = new BasicResponse<List<B_MusicfilesInfo>>();
            b_MusicfilesRequest.PagerInfo.PageIndex = b_MusicfilesRequest.PagerInfo.PageIndex - 1;
            if (b_MusicfilesRequest.PagerInfo.PageIndex < 0)
            {
                b_MusicfilesRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var b_MusicfilesModelLists = _Repository.GetB_MusicfilesList(b_MusicfilesRequest.PagerInfo.PageIndex, b_MusicfilesRequest.PagerInfo.PageSize, out rowcount);
            var b_MusicfilesInfoLists = new List<B_MusicfilesInfo>();
            foreach (var item in b_MusicfilesModelLists)
            {
                var B_MusicfilesInfo = ObjectConverter.Copy<B_MusicfilesModel, B_MusicfilesInfo>(item);
                b_MusicfilesInfoLists.Add(B_MusicfilesInfo);
            }
            b_Musicfilesresponse.Data = b_MusicfilesInfoLists;
            return b_Musicfilesresponse;
        }
				public BasicResponse<B_MusicfilesInfo> GetB_MusicfilesById(B_MusicfilesGetRequest b_MusicfilesRequest)
        {
            var result = _Repository.GetB_MusicfilesById(b_MusicfilesRequest.Id);
            var b_MusicfilesInfo = ObjectConverter.Copy<B_MusicfilesModel, B_MusicfilesInfo>(result);
            var b_Musicfilesresponse = new BasicResponse<B_MusicfilesInfo>();
            b_Musicfilesresponse.Data = b_MusicfilesInfo;
            return b_Musicfilesresponse;
        }
	}
}



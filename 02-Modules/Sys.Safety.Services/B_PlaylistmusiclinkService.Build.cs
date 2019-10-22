using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Playlistmusiclink;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class B_PlaylistmusiclinkService:IB_PlaylistmusiclinkService
    {
		private IB_PlaylistmusiclinkRepository _Repository;

		public B_PlaylistmusiclinkService(IB_PlaylistmusiclinkRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<B_PlaylistmusiclinkInfo> AddB_Playlistmusiclink(B_PlaylistmusiclinkAddRequest b_PlaylistmusiclinkRequest)
        {
            var _b_Playlistmusiclink = ObjectConverter.Copy<B_PlaylistmusiclinkInfo, B_PlaylistmusiclinkModel>(b_PlaylistmusiclinkRequest.B_PlaylistmusiclinkInfo);
            var resultb_Playlistmusiclink = _Repository.AddB_Playlistmusiclink(_b_Playlistmusiclink);
            var b_Playlistmusiclinkresponse = new BasicResponse<B_PlaylistmusiclinkInfo>();
            b_Playlistmusiclinkresponse.Data = ObjectConverter.Copy<B_PlaylistmusiclinkModel, B_PlaylistmusiclinkInfo>(resultb_Playlistmusiclink);
            return b_Playlistmusiclinkresponse;
        }
				public BasicResponse<B_PlaylistmusiclinkInfo> UpdateB_Playlistmusiclink(B_PlaylistmusiclinkUpdateRequest b_PlaylistmusiclinkRequest)
        {
            var _b_Playlistmusiclink = ObjectConverter.Copy<B_PlaylistmusiclinkInfo, B_PlaylistmusiclinkModel>(b_PlaylistmusiclinkRequest.B_PlaylistmusiclinkInfo);
            _Repository.UpdateB_Playlistmusiclink(_b_Playlistmusiclink);
            var b_Playlistmusiclinkresponse = new BasicResponse<B_PlaylistmusiclinkInfo>();
            b_Playlistmusiclinkresponse.Data = ObjectConverter.Copy<B_PlaylistmusiclinkModel, B_PlaylistmusiclinkInfo>(_b_Playlistmusiclink);  
            return b_Playlistmusiclinkresponse;
        }
				public BasicResponse DeleteB_Playlistmusiclink(B_PlaylistmusiclinkDeleteRequest b_PlaylistmusiclinkRequest)
        {
            _Repository.DeleteB_Playlistmusiclink(b_PlaylistmusiclinkRequest.Id);
            var b_Playlistmusiclinkresponse = new BasicResponse();            
            return b_Playlistmusiclinkresponse;
        }
				public BasicResponse<List<B_PlaylistmusiclinkInfo>> GetB_PlaylistmusiclinkList(B_PlaylistmusiclinkGetListRequest b_PlaylistmusiclinkRequest)
        {
            var b_Playlistmusiclinkresponse = new BasicResponse<List<B_PlaylistmusiclinkInfo>>();
            b_PlaylistmusiclinkRequest.PagerInfo.PageIndex = b_PlaylistmusiclinkRequest.PagerInfo.PageIndex - 1;
            if (b_PlaylistmusiclinkRequest.PagerInfo.PageIndex < 0)
            {
                b_PlaylistmusiclinkRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var b_PlaylistmusiclinkModelLists = _Repository.GetB_PlaylistmusiclinkList(b_PlaylistmusiclinkRequest.PagerInfo.PageIndex, b_PlaylistmusiclinkRequest.PagerInfo.PageSize, out rowcount);
            var b_PlaylistmusiclinkInfoLists = new List<B_PlaylistmusiclinkInfo>();
            foreach (var item in b_PlaylistmusiclinkModelLists)
            {
                var B_PlaylistmusiclinkInfo = ObjectConverter.Copy<B_PlaylistmusiclinkModel, B_PlaylistmusiclinkInfo>(item);
                b_PlaylistmusiclinkInfoLists.Add(B_PlaylistmusiclinkInfo);
            }
            b_Playlistmusiclinkresponse.Data = b_PlaylistmusiclinkInfoLists;
            return b_Playlistmusiclinkresponse;
        }
				public BasicResponse<B_PlaylistmusiclinkInfo> GetB_PlaylistmusiclinkById(B_PlaylistmusiclinkGetRequest b_PlaylistmusiclinkRequest)
        {
            var result = _Repository.GetB_PlaylistmusiclinkById(b_PlaylistmusiclinkRequest.Id);
            var b_PlaylistmusiclinkInfo = ObjectConverter.Copy<B_PlaylistmusiclinkModel, B_PlaylistmusiclinkInfo>(result);
            var b_Playlistmusiclinkresponse = new BasicResponse<B_PlaylistmusiclinkInfo>();
            b_Playlistmusiclinkresponse.Data = b_PlaylistmusiclinkInfo;
            return b_Playlistmusiclinkresponse;
        }
	}
}



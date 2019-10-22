using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Playlist;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class B_PlaylistService:IB_PlaylistService
    {
		private IB_PlaylistRepository _Repository;

		public B_PlaylistService(IB_PlaylistRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<B_PlaylistInfo> AddB_Playlist(B_PlaylistAddRequest b_PlaylistRequest)
        {
            var _b_Playlist = ObjectConverter.Copy<B_PlaylistInfo, B_PlaylistModel>(b_PlaylistRequest.B_PlaylistInfo);
            var resultb_Playlist = _Repository.AddB_Playlist(_b_Playlist);
            var b_Playlistresponse = new BasicResponse<B_PlaylistInfo>();
            b_Playlistresponse.Data = ObjectConverter.Copy<B_PlaylistModel, B_PlaylistInfo>(resultb_Playlist);
            return b_Playlistresponse;
        }
				public BasicResponse<B_PlaylistInfo> UpdateB_Playlist(B_PlaylistUpdateRequest b_PlaylistRequest)
        {
            var _b_Playlist = ObjectConverter.Copy<B_PlaylistInfo, B_PlaylistModel>(b_PlaylistRequest.B_PlaylistInfo);
            _Repository.UpdateB_Playlist(_b_Playlist);
            var b_Playlistresponse = new BasicResponse<B_PlaylistInfo>();
            b_Playlistresponse.Data = ObjectConverter.Copy<B_PlaylistModel, B_PlaylistInfo>(_b_Playlist);  
            return b_Playlistresponse;
        }
				public BasicResponse DeleteB_Playlist(B_PlaylistDeleteRequest b_PlaylistRequest)
        {
            _Repository.DeleteB_Playlist(b_PlaylistRequest.Id);
            var b_Playlistresponse = new BasicResponse();            
            return b_Playlistresponse;
        }
				public BasicResponse<List<B_PlaylistInfo>> GetB_PlaylistList(B_PlaylistGetListRequest b_PlaylistRequest)
        {
            var b_Playlistresponse = new BasicResponse<List<B_PlaylistInfo>>();
            b_PlaylistRequest.PagerInfo.PageIndex = b_PlaylistRequest.PagerInfo.PageIndex - 1;
            if (b_PlaylistRequest.PagerInfo.PageIndex < 0)
            {
                b_PlaylistRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var b_PlaylistModelLists = _Repository.GetB_PlaylistList(b_PlaylistRequest.PagerInfo.PageIndex, b_PlaylistRequest.PagerInfo.PageSize, out rowcount);
            var b_PlaylistInfoLists = new List<B_PlaylistInfo>();
            foreach (var item in b_PlaylistModelLists)
            {
                var B_PlaylistInfo = ObjectConverter.Copy<B_PlaylistModel, B_PlaylistInfo>(item);
                b_PlaylistInfoLists.Add(B_PlaylistInfo);
            }
            b_Playlistresponse.Data = b_PlaylistInfoLists;
            return b_Playlistresponse;
        }
				public BasicResponse<B_PlaylistInfo> GetB_PlaylistById(B_PlaylistGetRequest b_PlaylistRequest)
        {
            var result = _Repository.GetB_PlaylistById(b_PlaylistRequest.Id);
            var b_PlaylistInfo = ObjectConverter.Copy<B_PlaylistModel, B_PlaylistInfo>(result);
            var b_Playlistresponse = new BasicResponse<B_PlaylistInfo>();
            b_Playlistresponse.Data = b_PlaylistInfo;
            return b_Playlistresponse;
        }
	}
}



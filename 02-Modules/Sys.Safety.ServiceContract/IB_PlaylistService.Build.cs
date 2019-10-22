using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Playlist;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_PlaylistService
    {  
	            BasicResponse<B_PlaylistInfo> AddB_Playlist(B_PlaylistAddRequest b_PlaylistRequest);		
		        BasicResponse<B_PlaylistInfo> UpdateB_Playlist(B_PlaylistUpdateRequest b_PlaylistRequest);	 
		        BasicResponse DeleteB_Playlist(B_PlaylistDeleteRequest b_PlaylistRequest);
		        BasicResponse<List<B_PlaylistInfo>> GetB_PlaylistList(B_PlaylistGetListRequest b_PlaylistRequest);
		         BasicResponse<B_PlaylistInfo> GetB_PlaylistById(B_PlaylistGetRequest b_PlaylistRequest);	
    }
}


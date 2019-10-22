using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Playlistmusiclink;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_PlaylistmusiclinkService
    {  
	            BasicResponse<B_PlaylistmusiclinkInfo> AddB_Playlistmusiclink(B_PlaylistmusiclinkAddRequest b_PlaylistmusiclinkRequest);		
		        BasicResponse<B_PlaylistmusiclinkInfo> UpdateB_Playlistmusiclink(B_PlaylistmusiclinkUpdateRequest b_PlaylistmusiclinkRequest);	 
		        BasicResponse DeleteB_Playlistmusiclink(B_PlaylistmusiclinkDeleteRequest b_PlaylistmusiclinkRequest);
		        BasicResponse<List<B_PlaylistmusiclinkInfo>> GetB_PlaylistmusiclinkList(B_PlaylistmusiclinkGetListRequest b_PlaylistmusiclinkRequest);
		         BasicResponse<B_PlaylistmusiclinkInfo> GetB_PlaylistmusiclinkById(B_PlaylistmusiclinkGetRequest b_PlaylistmusiclinkRequest);	
    }
}


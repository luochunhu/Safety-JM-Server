using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Musicfiles;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_MusicfilesService
    {  
	            BasicResponse<B_MusicfilesInfo> AddB_Musicfiles(B_MusicfilesAddRequest b_MusicfilesRequest);		
		        BasicResponse<B_MusicfilesInfo> UpdateB_Musicfiles(B_MusicfilesUpdateRequest b_MusicfilesRequest);	 
		        BasicResponse DeleteB_Musicfiles(B_MusicfilesDeleteRequest b_MusicfilesRequest);
		        BasicResponse<List<B_MusicfilesInfo>> GetB_MusicfilesList(B_MusicfilesGetListRequest b_MusicfilesRequest);
		         BasicResponse<B_MusicfilesInfo> GetB_MusicfilesById(B_MusicfilesGetRequest b_MusicfilesRequest);	
    }
}


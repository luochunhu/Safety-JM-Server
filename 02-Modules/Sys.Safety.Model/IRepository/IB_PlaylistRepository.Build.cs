using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_PlaylistRepository : IRepository<B_PlaylistModel>
    {
                B_PlaylistModel AddB_Playlist(B_PlaylistModel b_PlaylistModel);
		        void UpdateB_Playlist(B_PlaylistModel b_PlaylistModel);
	            void DeleteB_Playlist(string id);
		        IList<B_PlaylistModel> GetB_PlaylistList(int pageIndex, int pageSize, out int rowCount);
				B_PlaylistModel GetB_PlaylistById(string id);
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_PlaylistmusiclinkRepository : IRepository<B_PlaylistmusiclinkModel>
    {
                B_PlaylistmusiclinkModel AddB_Playlistmusiclink(B_PlaylistmusiclinkModel b_PlaylistmusiclinkModel);
		        void UpdateB_Playlistmusiclink(B_PlaylistmusiclinkModel b_PlaylistmusiclinkModel);
	            void DeleteB_Playlistmusiclink(string id);
		        IList<B_PlaylistmusiclinkModel> GetB_PlaylistmusiclinkList(int pageIndex, int pageSize, out int rowCount);
				B_PlaylistmusiclinkModel GetB_PlaylistmusiclinkById(string id);
    }
}

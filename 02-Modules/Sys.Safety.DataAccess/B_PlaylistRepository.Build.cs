using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_PlaylistRepository:RepositoryBase<B_PlaylistModel>,IB_PlaylistRepository
    {

                public B_PlaylistModel AddB_Playlist(B_PlaylistModel b_PlaylistModel)
		{
		   return base.Insert(b_PlaylistModel);
		}
		        public void UpdateB_Playlist(B_PlaylistModel b_PlaylistModel)
		{
		   base.Update(b_PlaylistModel);
		}
	            public void DeleteB_Playlist(string id)
		{
		   base.Delete(id);
		}
		        public IList<B_PlaylistModel> GetB_PlaylistList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  b_PlaylistModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return b_PlaylistModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public B_PlaylistModel GetB_PlaylistById(string id)
		{
		    B_PlaylistModel b_PlaylistModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return b_PlaylistModel;
		}
    }
}

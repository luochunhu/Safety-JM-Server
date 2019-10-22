using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_PlaylistmusiclinkRepository:RepositoryBase<B_PlaylistmusiclinkModel>,IB_PlaylistmusiclinkRepository
    {

                public B_PlaylistmusiclinkModel AddB_Playlistmusiclink(B_PlaylistmusiclinkModel b_PlaylistmusiclinkModel)
		{
		   return base.Insert(b_PlaylistmusiclinkModel);
		}
		        public void UpdateB_Playlistmusiclink(B_PlaylistmusiclinkModel b_PlaylistmusiclinkModel)
		{
		   base.Update(b_PlaylistmusiclinkModel);
		}
	            public void DeleteB_Playlistmusiclink(string id)
		{
		   base.Delete(id);
		}
		        public IList<B_PlaylistmusiclinkModel> GetB_PlaylistmusiclinkList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  b_PlaylistmusiclinkModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return b_PlaylistmusiclinkModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public B_PlaylistmusiclinkModel GetB_PlaylistmusiclinkById(string id)
		{
		    B_PlaylistmusiclinkModel b_PlaylistmusiclinkModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return b_PlaylistmusiclinkModel;
		}
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_MusicfilesRepository : IRepository<B_MusicfilesModel>
    {
                B_MusicfilesModel AddB_Musicfiles(B_MusicfilesModel b_MusicfilesModel);
		        void UpdateB_Musicfiles(B_MusicfilesModel b_MusicfilesModel);
	            void DeleteB_Musicfiles(string id);
		        IList<B_MusicfilesModel> GetB_MusicfilesList(int pageIndex, int pageSize, out int rowCount);
				B_MusicfilesModel GetB_MusicfilesById(string id);
    }
}

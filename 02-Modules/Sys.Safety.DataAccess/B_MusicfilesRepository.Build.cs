using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_MusicfilesRepository:RepositoryBase<B_MusicfilesModel>,IB_MusicfilesRepository
    {

                public B_MusicfilesModel AddB_Musicfiles(B_MusicfilesModel b_MusicfilesModel)
		{
		   return base.Insert(b_MusicfilesModel);
		}
		        public void UpdateB_Musicfiles(B_MusicfilesModel b_MusicfilesModel)
		{
		   base.Update(b_MusicfilesModel);
		}
	            public void DeleteB_Musicfiles(string id)
		{
		   base.Delete(id);
		}
		        public IList<B_MusicfilesModel> GetB_MusicfilesList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  b_MusicfilesModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return b_MusicfilesModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public B_MusicfilesModel GetB_MusicfilesById(string id)
		{
		    B_MusicfilesModel b_MusicfilesModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return b_MusicfilesModel;
		}
    }
}

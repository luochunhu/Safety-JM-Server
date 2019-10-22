using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_PointhisRepository:RepositoryBase<Jc_PointhisModel>,IJc_PointhisRepository
    {

                public Jc_PointhisModel AddJc_Pointhis(Jc_PointhisModel jc_PointhisModel)
		{
		   return base.Insert(jc_PointhisModel);
		}
		        public void UpdateJc_Pointhis(Jc_PointhisModel jc_PointhisModel)
		{
		   base.Update(jc_PointhisModel);
		}
	            public void DeleteJc_Pointhis(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_PointhisModel> GetJc_PointhisList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_PointhisModelLists = base.Datas.ToList();
		   rowCount = jc_PointhisModelLists.Count();
           return jc_PointhisModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_PointhisModel GetJc_PointhisById(string id)
		{
		    Jc_PointhisModel jc_PointhisModel = base.Datas.FirstOrDefault(c => c.PointID == id);
            return jc_PointhisModel;
		}
    }
}

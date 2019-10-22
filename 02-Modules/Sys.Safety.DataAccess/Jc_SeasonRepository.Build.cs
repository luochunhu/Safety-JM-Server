using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_SeasonRepository:RepositoryBase<Jc_SeasonModel>,IJc_SeasonRepository
    {

                public Jc_SeasonModel AddJc_Season(Jc_SeasonModel jc_SeasonModel)
		{
		   return base.Insert(jc_SeasonModel);
		}
		        public void UpdateJc_Season(Jc_SeasonModel jc_SeasonModel)
		{
		   base.Update(jc_SeasonModel);
		}
	            public void DeleteJc_Season(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_SeasonModel> GetJc_SeasonList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_SeasonModelLists = base.Datas.ToList();
		   rowCount = jc_SeasonModelLists.Count();
           return jc_SeasonModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_SeasonModel GetJc_SeasonById(string id)
		{
		    Jc_SeasonModel jc_SeasonModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_SeasonModel;
		}
    }
}

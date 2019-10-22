using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_RemarkRepository:RepositoryBase<Jc_RemarkModel>,IJc_RemarkRepository
    {

                public Jc_RemarkModel AddJc_Remark(Jc_RemarkModel jc_RemarkModel)
		{
		   return base.Insert(jc_RemarkModel);
		}
		        public void UpdateJc_Remark(Jc_RemarkModel jc_RemarkModel)
		{
		   base.Update(jc_RemarkModel);
		}
	            public void DeleteJc_Remark(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_RemarkModel> GetJc_RemarkList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_RemarkModelLists = base.Datas.ToList();
		   rowCount = jc_RemarkModelLists.Count();
           return jc_RemarkModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_RemarkModel GetJc_RemarkById(string id)
		{
		    Jc_RemarkModel jc_RemarkModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_RemarkModel;
		}
    }
}

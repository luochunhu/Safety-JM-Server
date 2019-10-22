using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_CallhistoryRepository:RepositoryBase<B_CallhistoryModel>,IB_CallhistoryRepository
    {

                public B_CallhistoryModel AddB_Callhistory(B_CallhistoryModel b_CallhistoryModel)
		{
		   return base.Insert(b_CallhistoryModel);
		}
		        public void UpdateB_Callhistory(B_CallhistoryModel b_CallhistoryModel)
		{
		   base.Update(b_CallhistoryModel);
		}
	            public void DeleteB_Callhistory(string id)
		{
		   base.Delete(id);
		}
		        public IList<B_CallhistoryModel> GetB_CallhistoryList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  b_CallhistoryModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return b_CallhistoryModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public B_CallhistoryModel GetB_CallhistoryById(string id)
		{
		    B_CallhistoryModel b_CallhistoryModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return b_CallhistoryModel;
		}
    }
}

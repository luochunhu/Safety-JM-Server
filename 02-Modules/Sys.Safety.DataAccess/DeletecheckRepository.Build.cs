using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class DeletecheckRepository:RepositoryBase<DeletecheckModel>,IDeletecheckRepository
    {

                public DeletecheckModel AddDeletecheck(DeletecheckModel deletecheckModel)
		{
		   return base.Insert(deletecheckModel);
		}
		        public void UpdateDeletecheck(DeletecheckModel deletecheckModel)
		{
		   base.Update(deletecheckModel);
		}
	            public void DeleteDeletecheck(string id)
		{
		   base.Delete(id);
		}
		        public IList<DeletecheckModel> GetDeletecheckList(int pageIndex, int pageSize, out int rowCount)
		{
            var deletecheckModelLists = base.Datas;
		   rowCount = deletecheckModelLists.Count();
           return deletecheckModelLists.OrderBy(p => p.DeleteCheckID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public DeletecheckModel GetDeletecheckById(string id)
		{
		    DeletecheckModel deletecheckModel = base.Datas.FirstOrDefault(c => c.DeleteCheckID == id);
            return deletecheckModel;
		}
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IDeletecheckRepository : IRepository<DeletecheckModel>
    {
                DeletecheckModel AddDeletecheck(DeletecheckModel deletecheckModel);
		        void UpdateDeletecheck(DeletecheckModel deletecheckModel);
	            void DeleteDeletecheck(string id);
		        IList<DeletecheckModel> GetDeletecheckList(int pageIndex, int pageSize, out int rowCount);
				DeletecheckModel GetDeletecheckById(string id);
    }
}

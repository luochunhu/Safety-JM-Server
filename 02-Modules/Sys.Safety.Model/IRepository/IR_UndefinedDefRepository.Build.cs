using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_UndefinedDefRepository : IRepository<R_UndefinedDefModel>
    {
                R_UndefinedDefModel AddUndefinedDef(R_UndefinedDefModel undefinedDefModel);
		        void UpdateUndefinedDef(R_UndefinedDefModel undefinedDefModel);
	            void DeleteUndefinedDef(string id);
		        IList<R_UndefinedDefModel> GetUndefinedDefList(int pageIndex, int pageSize, out int rowCount);
				R_UndefinedDefModel GetUndefinedDefById(string id);
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_KqbcRepository : IRepository<R_KqbcModel>
    {
                R_KqbcModel AddKqbc(R_KqbcModel kqbcModel);
		        void UpdateKqbc(R_KqbcModel kqbcModel);
	            void DeleteKqbc(string id);
		        IList<R_KqbcModel> GetKqbcList(int pageIndex, int pageSize, out int rowCount);
                IList<R_KqbcModel> GetAllKqbcList();
				R_KqbcModel GetKqbcById(string id);
    }
}

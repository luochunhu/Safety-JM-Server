using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IFlagRepository : IRepository<FlagModel>
    {
                FlagModel AddFlag(FlagModel flagModel);
		        void UpdateFlag(FlagModel flagModel);
	            void DeleteFlag(string id);
		        IList<FlagModel> GetFlagList(int pageIndex, int pageSize, out int rowCount);
				FlagModel GetFlagById(string id);
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IUserrightRepository : IRepository<UserrightModel>
    {
                UserrightModel AddUserright(UserrightModel userrightModel);
		        void UpdateUserright(UserrightModel userrightModel);
	            void DeleteUserright(string id);
		        IList<UserrightModel> GetUserrightList(int pageIndex, int pageSize, out int rowCount);
				UserrightModel GetUserrightById(string id);
    }
}

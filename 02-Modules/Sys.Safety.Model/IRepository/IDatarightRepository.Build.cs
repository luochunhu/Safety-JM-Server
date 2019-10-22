using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IDatarightRepository : IRepository<DatarightModel>
    {
                DatarightModel AddDataright(DatarightModel datarightModel);
		        void UpdateDataright(DatarightModel datarightModel);
	            void DeleteDataright(string id);
		        IList<DatarightModel> GetDatarightList(int pageIndex, int pageSize, out int rowCount);
				DatarightModel GetDatarightById(string id);
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IDataexchangesettingRepository : IRepository<DataexchangesettingModel>
    {
                DataexchangesettingModel AddDataexchangesetting(DataexchangesettingModel dataexchangesettingModel);
		        void UpdateDataexchangesetting(DataexchangesettingModel dataexchangesettingModel);
	            void DeleteDataexchangesetting(string id);
		        IList<DataexchangesettingModel> GetDataexchangesettingList(int pageIndex, int pageSize, out int rowCount);
				DataexchangesettingModel GetDataexchangesettingById(string id);
    }
}

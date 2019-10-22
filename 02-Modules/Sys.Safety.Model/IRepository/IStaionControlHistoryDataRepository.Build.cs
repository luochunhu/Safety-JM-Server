using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IStaionControlHistoryDataRepository : IRepository<StaionControlHistoryDataModel>
    {
                StaionControlHistoryDataModel AddStaionControlHistoryData(StaionControlHistoryDataModel staionControlHistoryDataModel);
		        void UpdateStaionControlHistoryData(StaionControlHistoryDataModel staionControlHistoryDataModel);
	            void DeleteStaionControlHistoryData(string id);
		        IList<StaionControlHistoryDataModel> GetStaionControlHistoryDataList(int pageIndex, int pageSize, out int rowCount);
				StaionControlHistoryDataModel GetStaionControlHistoryDataById(string id);

                IList<StaionControlHistoryDataModel> GetStaionControlHistoryDataByFzh(ushort fzh);
    }
}

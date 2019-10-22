using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IStaionHistoryDataRepository : IRepository<StaionHistoryDataModel>
    {
        StaionHistoryDataModel AddStaionHistoryData(StaionHistoryDataModel staionHistoryDataModel);
        void UpdateStaionHistoryData(StaionHistoryDataModel staionHistoryDataModel);
        void DeleteStaionHistoryData(string id);
        IList<StaionHistoryDataModel> GetStaionHistoryDataList(int pageIndex, int pageSize, out int rowCount);
        StaionHistoryDataModel GetStaionHistoryDataById(string id);

        IList<StaionHistoryDataModel> GetAllStaionHistoryData();
    }
}

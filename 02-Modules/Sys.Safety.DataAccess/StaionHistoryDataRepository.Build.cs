using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class StaionHistoryDataRepository : RepositoryBase<StaionHistoryDataModel>, IStaionHistoryDataRepository
    {

        public StaionHistoryDataModel AddStaionHistoryData(StaionHistoryDataModel staionHistoryDataModel)
        {
            return base.Insert(staionHistoryDataModel);
        }
        public void UpdateStaionHistoryData(StaionHistoryDataModel staionHistoryDataModel)
        {
            base.Update(staionHistoryDataModel);
        }
        public void DeleteStaionHistoryData(string id)
        {
            base.Delete(id);
        }
        public IList<StaionHistoryDataModel> GetStaionHistoryDataList(int pageIndex, int pageSize, out int rowCount)
        {
            var staionHistoryDataModelLists = base.Datas.ToList();
            rowCount = staionHistoryDataModelLists.Count();
            return staionHistoryDataModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public StaionHistoryDataModel GetStaionHistoryDataById(string id)
        {
            StaionHistoryDataModel staionHistoryDataModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return staionHistoryDataModel;
        }

        public IList<StaionHistoryDataModel> GetAllStaionHistoryData()
        {
            return base.Datas.ToList();
        }
    }
}

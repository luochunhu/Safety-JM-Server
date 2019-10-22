using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class StaionControlHistoryDataRepository : RepositoryBase<StaionControlHistoryDataModel>, IStaionControlHistoryDataRepository
    {

        public StaionControlHistoryDataModel AddStaionControlHistoryData(StaionControlHistoryDataModel staionControlHistoryDataModel)
        {
            return base.Insert(staionControlHistoryDataModel);
        }
        public void UpdateStaionControlHistoryData(StaionControlHistoryDataModel staionControlHistoryDataModel)
        {
            base.Update(staionControlHistoryDataModel);
        }
        public void DeleteStaionControlHistoryData(string id)
        {
            base.Delete(id);
        }
        public IList<StaionControlHistoryDataModel> GetStaionControlHistoryDataList(int pageIndex, int pageSize, out int rowCount)
        {
            var staionControlHistoryDataModelLists = base.Datas.ToList();
            rowCount = staionControlHistoryDataModelLists.Count();
            return staionControlHistoryDataModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public StaionControlHistoryDataModel GetStaionControlHistoryDataById(string id)
        {
            StaionControlHistoryDataModel staionControlHistoryDataModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return staionControlHistoryDataModel;
        }


        public IList<StaionControlHistoryDataModel> GetStaionControlHistoryDataByFzh(ushort fzh)
        {
            return base.Datas.Where(a => a.Fzh == fzh.ToString()).ToList();
        }
    }
}

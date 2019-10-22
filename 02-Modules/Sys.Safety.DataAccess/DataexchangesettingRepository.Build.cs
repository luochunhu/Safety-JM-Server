using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class DataexchangesettingRepository:RepositoryBase<DataexchangesettingModel>,IDataexchangesettingRepository
    {

                public DataexchangesettingModel AddDataexchangesetting(DataexchangesettingModel dataexchangesettingModel)
		{
		   return base.Insert(dataexchangesettingModel);
		}
		        public void UpdateDataexchangesetting(DataexchangesettingModel dataexchangesettingModel)
		{
		   base.Update(dataexchangesettingModel);
		}
	            public void DeleteDataexchangesetting(string id)
		{
		   base.Delete(id);
		}
		        public IList<DataexchangesettingModel> GetDataexchangesettingList(int pageIndex, int pageSize, out int rowCount)
		{
            var dataexchangesettingModelLists = base.Datas.ToList();
		   rowCount = dataexchangesettingModelLists.Count();
           return dataexchangesettingModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public DataexchangesettingModel GetDataexchangesettingById(string id)
		{
		    DataexchangesettingModel dataexchangesettingModel = base.Datas.FirstOrDefault(c => c.DataExchangeSettingID == id);
            return dataexchangesettingModel;
		}
    }
}

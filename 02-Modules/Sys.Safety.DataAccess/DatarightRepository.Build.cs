using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class DatarightRepository:RepositoryBase<DatarightModel>,IDatarightRepository
    {

                public DatarightModel AddDataright(DatarightModel datarightModel)
		{
		   return base.Insert(datarightModel);
		}
		        public void UpdateDataright(DatarightModel datarightModel)
		{
		   base.Update(datarightModel);
		}
	            public void DeleteDataright(string id)
		{
		   base.Delete(id);
		}
		        public IList<DatarightModel> GetDatarightList(int pageIndex, int pageSize, out int rowCount)
		{
            var datarightModelLists = base.Datas;
		   rowCount = datarightModelLists.Count();
           return datarightModelLists.OrderBy(p => p.DataRightID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public DatarightModel GetDatarightById(string id)
		{
		    DatarightModel datarightModel = base.Datas.FirstOrDefault(c => c.DataRightID == id);
            return datarightModel;
		}
    }
}

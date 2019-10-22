using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class UserrightRepository:RepositoryBase<UserrightModel>,IUserrightRepository
    {

                public UserrightModel AddUserright(UserrightModel userrightModel)
		{
		   return base.Insert(userrightModel);
		}
		        public void UpdateUserright(UserrightModel userrightModel)
		{
		   base.Update(userrightModel);
		}
	            public void DeleteUserright(string id)
		{
		   base.Delete(id);
		}
		        public IList<UserrightModel> GetUserrightList(int pageIndex, int pageSize, out int rowCount)
		{
            var userrightModelLists = base.Datas;
		   rowCount = userrightModelLists.Count();
           return userrightModelLists.OrderBy(p => p.UserRightID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public UserrightModel GetUserrightById(string id)
		{
		    UserrightModel userrightModel = base.Datas.FirstOrDefault(c => c.UserRightID == id);
            return userrightModel;
		}
    }
}

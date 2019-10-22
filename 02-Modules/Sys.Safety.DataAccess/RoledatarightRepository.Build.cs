using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class RoledatarightRepository:RepositoryBase<RoledatarightModel>,IRoledatarightRepository
    {

                public RoledatarightModel AddRoledataright(RoledatarightModel roledatarightModel)
		{
		   return base.Insert(roledatarightModel);
		}
		        public void UpdateRoledataright(RoledatarightModel roledatarightModel)
		{
		   base.Update(roledatarightModel);
		}
	            public void DeleteRoledataright(string id)
		{
		   base.Delete(id);
		}
		        public IList<RoledatarightModel> GetRoledatarightList(int pageIndex, int pageSize, out int rowCount)
		{
            var roledatarightModelLists = base.Datas;
		   rowCount = roledatarightModelLists.Count();
           return roledatarightModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public RoledatarightModel GetRoledatarightById(string id)
		{
		    RoledatarightModel roledatarightModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return roledatarightModel;
		}
    }
}

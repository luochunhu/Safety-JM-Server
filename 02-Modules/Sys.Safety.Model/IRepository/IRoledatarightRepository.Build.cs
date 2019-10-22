using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRoledatarightRepository : IRepository<RoledatarightModel>
    {
                RoledatarightModel AddRoledataright(RoledatarightModel roledatarightModel);
		        void UpdateRoledataright(RoledatarightModel roledatarightModel);
	            void DeleteRoledataright(string id);
		        IList<RoledatarightModel> GetRoledatarightList(int pageIndex, int pageSize, out int rowCount);
				RoledatarightModel GetRoledatarightById(string id);
    }
}

using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IWebauthorityRepository : IRepository<WebauthorityModel>
    {
                WebauthorityModel AddWebauthority(WebauthorityModel webauthorityModel);
		        void UpdateWebauthority(WebauthorityModel webauthorityModel);
	            void DeleteWebauthority(string id);
		        IList<WebauthorityModel> GetWebauthorityList(int pageIndex, int pageSize, out int rowCount);
				WebauthorityModel GetWebauthorityById(string id);
    }
}

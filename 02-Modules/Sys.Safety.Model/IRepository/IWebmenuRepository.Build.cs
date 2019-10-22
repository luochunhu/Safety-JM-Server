using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IWebmenuRepository : IRepository<WebmenuModel>
    {
                WebmenuModel AddWebmenu(WebmenuModel webmenuModel);
		        void UpdateWebmenu(WebmenuModel webmenuModel);
	            void DeleteWebmenu(string id);
		        IList<WebmenuModel> GetWebmenuList(int pageIndex, int pageSize, out int rowCount);
				WebmenuModel GetWebmenuById(string id);
    }
}

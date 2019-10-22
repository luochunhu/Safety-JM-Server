using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class WebmenuRepository:RepositoryBase<WebmenuModel>,IWebmenuRepository
    {

                public WebmenuModel AddWebmenu(WebmenuModel webmenuModel)
		{
		   return base.Insert(webmenuModel);
		}
		        public void UpdateWebmenu(WebmenuModel webmenuModel)
		{
		   base.Update(webmenuModel);
		}
	            public void DeleteWebmenu(string id)
		{
		   base.Delete(id);
		}
		        public IList<WebmenuModel> GetWebmenuList(int pageIndex, int pageSize, out int rowCount)
		{
            var webmenuModelLists = base.Datas;
		   rowCount = webmenuModelLists.Count();
           return webmenuModelLists.OrderBy(p => p.ModuleID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public WebmenuModel GetWebmenuById(string id)
		{
		    WebmenuModel webmenuModel = base.Datas.FirstOrDefault(c => c.ModuleID == id);
            return webmenuModel;
		}
    }
}

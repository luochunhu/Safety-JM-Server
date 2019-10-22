using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class WebauthorityRepository:RepositoryBase<WebauthorityModel>,IWebauthorityRepository
    {

                public WebauthorityModel AddWebauthority(WebauthorityModel webauthorityModel)
		{
		   return base.Insert(webauthorityModel);
		}
		        public void UpdateWebauthority(WebauthorityModel webauthorityModel)
		{
		   base.Update(webauthorityModel);
		}
	            public void DeleteWebauthority(string id)
		{
		   base.Delete(id);
		}
		        public IList<WebauthorityModel> GetWebauthorityList(int pageIndex, int pageSize, out int rowCount)
		{
            var webauthorityModelLists = base.Datas;
		   rowCount = webauthorityModelLists.Count();
           return webauthorityModelLists.OrderBy(p => p.AuthID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public WebauthorityModel GetWebauthorityById(string id)
		{
		    WebauthorityModel webauthorityModel = base.Datas.FirstOrDefault(c => c.AuthID == id);
            return webauthorityModel;
		}
    }
}

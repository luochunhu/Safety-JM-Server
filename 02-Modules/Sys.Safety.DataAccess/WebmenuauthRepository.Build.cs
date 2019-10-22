using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class WebmenuauthRepository:RepositoryBase<WebmenuauthModel>,IWebmenuauthRepository
    {

                public WebmenuauthModel AddWebmenuauth(WebmenuauthModel webmenuauthModel)
		{
		   return base.Insert(webmenuauthModel);
		}
		        public void UpdateWebmenuauth(WebmenuauthModel webmenuauthModel)
		{
		   base.Update(webmenuauthModel);
		}
	            public void DeleteWebmenuauth(string id)
		{
		   base.Delete(id);
		}
		        public IList<WebmenuauthModel> GetWebmenuauthList(int pageIndex, int pageSize, out int rowCount)
		{
            var webmenuauthModelLists = base.Datas;
		   rowCount = webmenuauthModelLists.Count();
           return webmenuauthModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public WebmenuauthModel GetWebmenuauthById(string id)
		{
		    WebmenuauthModel webmenuauthModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return webmenuauthModel;
		}
    }
}

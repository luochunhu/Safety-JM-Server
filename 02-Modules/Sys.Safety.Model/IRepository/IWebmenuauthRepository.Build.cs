using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IWebmenuauthRepository : IRepository<WebmenuauthModel>
    {
                WebmenuauthModel AddWebmenuauth(WebmenuauthModel webmenuauthModel);
		        void UpdateWebmenuauth(WebmenuauthModel webmenuauthModel);
	            void DeleteWebmenuauth(string id);
		        IList<WebmenuauthModel> GetWebmenuauthList(int pageIndex, int pageSize, out int rowCount);
				WebmenuauthModel GetWebmenuauthById(string id);
    }
}

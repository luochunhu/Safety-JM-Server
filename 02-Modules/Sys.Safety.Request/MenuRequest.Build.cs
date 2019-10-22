using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Menu
{
    public partial class MenuAddRequest : Basic.Framework.Web.BasicRequest
    {
        public MenuInfo MenuInfo { get; set; }      
    }

	public partial class MenuUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public MenuInfo MenuInfo { get; set; }      
    }

    public partial class MenusUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public List<MenuInfo> MenuInfo { get; set; }
    }

    public partial class MenusDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public List<string> IdList { get; set; }
    }
    public partial class MenuDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }


    public partial class MenuGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class MenuGetByCOdeRequest : Basic.Framework.Web.BasicRequest
    {
        public string MenuCode { get; set; }
    }

    public partial class MenuGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

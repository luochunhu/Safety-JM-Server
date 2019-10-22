using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Roledataright
{
    public partial class RoledatarightAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RoledatarightInfo RoledatarightInfo { get; set; }      
    }

	public partial class RoledatarightUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RoledatarightInfo RoledatarightInfo { get; set; }      
    }

	public partial class RoledatarightDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RoledatarightGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RoledatarightGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Rolefields
{
    public partial class RolefieldsAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RolefieldsInfo RolefieldsInfo { get; set; }      
    }

	public partial class RolefieldsUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RolefieldsInfo RolefieldsInfo { get; set; }      
    }

	public partial class RolefieldsDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RolefieldsGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RolefieldsGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

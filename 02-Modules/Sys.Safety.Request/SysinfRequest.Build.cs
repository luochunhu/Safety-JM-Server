using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Sysinf
{
    public partial class SysinfAddRequest : Basic.Framework.Web.BasicRequest
    {
        public SysinfInfo SysinfInfo { get; set; }      
    }

	public partial class SysinfUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public SysinfInfo SysinfInfo { get; set; }      
    }

	public partial class SysinfDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class SysinfGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class SysinfGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

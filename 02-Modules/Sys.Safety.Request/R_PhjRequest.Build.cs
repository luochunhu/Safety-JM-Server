using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Phj
{
    public partial class PhjAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PhjInfo PhjInfo { get; set; }      
    }

	public partial class PhjUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PhjInfo PhjInfo { get; set; }      
    }

	public partial class PhjDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class PhjGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class PhjGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

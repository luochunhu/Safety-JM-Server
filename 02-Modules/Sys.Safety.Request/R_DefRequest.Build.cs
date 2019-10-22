using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;

namespace Sys.Safety.Request.R_Def
{
    public partial class R_DefAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefInfo DefInfo { get; set; }      
    }

	public partial class R_DefUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefInfo DefInfo { get; set; }      
    }

	public partial class R_DefDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_DefGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_DefGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

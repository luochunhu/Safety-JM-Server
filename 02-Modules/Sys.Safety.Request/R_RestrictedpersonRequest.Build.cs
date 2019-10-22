using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.R_Restrictedperson
{
    public partial class R_RestrictedpersonAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_RestrictedpersonInfo RestrictedpersonInfo { get; set; }      
    }

	public partial class R_RestrictedpersonUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_RestrictedpersonInfo RestrictedpersonInfo { get; set; }      
    }

	public partial class R_RestrictedpersonDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class R_RestrictedpersonDeleteByPointIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string PointId { get; set; }
    }

	public partial class R_RestrictedpersonGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_RestrictedpersonGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

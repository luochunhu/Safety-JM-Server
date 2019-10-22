using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Arearestrictedperson
{
    public partial class R_ArearestrictedpersonAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_ArearestrictedpersonInfo ArearestrictedpersonInfo { get; set; }      
    }

	public partial class R_ArearestrictedpersonUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_ArearestrictedpersonInfo ArearestrictedpersonInfo { get; set; }      
    }

	public partial class R_ArearestrictedpersonDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_ArearestrictedpersonGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class R_ArearestrictedpersonDeleteByAreaIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AreaId { get; set; }
    }

	public partial class R_ArearestrictedpersonGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

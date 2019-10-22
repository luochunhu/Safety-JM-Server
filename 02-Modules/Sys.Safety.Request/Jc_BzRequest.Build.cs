using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Bz
{
    public partial class Jc_BzAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BzInfo Jc_BzInfo { get; set; }      
    }

	public partial class Jc_BzUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BzInfo Jc_BzInfo { get; set; }      
    }

	public partial class Jc_BzDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_BzGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_BzGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

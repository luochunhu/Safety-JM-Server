using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Bx
{
    public partial class Jc_BxAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BxInfo Jc_BxInfo { get; set; }      
    }

	public partial class Jc_BxUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BxInfo Jc_BxInfo { get; set; }      
    }

	public partial class Jc_BxDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_BxGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_BxGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

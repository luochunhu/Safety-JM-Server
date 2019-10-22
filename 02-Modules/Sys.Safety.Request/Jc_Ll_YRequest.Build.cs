using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Ll_Y
{
    public partial class Jc_Ll_YAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_Ll_YInfo Jc_Ll_YInfo { get; set; }      
    }

	public partial class Jc_Ll_YUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_Ll_YInfo Jc_Ll_YInfo { get; set; }      
    }

	public partial class Jc_Ll_YDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_Ll_YGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_Ll_YGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

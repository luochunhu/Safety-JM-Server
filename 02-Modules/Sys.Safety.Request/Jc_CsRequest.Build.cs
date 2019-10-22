using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Cs
{
    public partial class Jc_CsAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_CsInfo Jc_CsInfo { get; set; }      
    }

	public partial class Jc_CsUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_CsInfo Jc_CsInfo { get; set; }      
    }

	public partial class Jc_CsDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_CsGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_CsGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Ll_M
{
    public partial class Jc_Ll_MAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_Ll_MInfo Jc_Ll_MInfo { get; set; }      
    }

	public partial class Jc_Ll_MUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_Ll_MInfo Jc_Ll_MInfo { get; set; }      
    }

	public partial class Jc_Ll_MDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_Ll_MGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_Ll_MGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

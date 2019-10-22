using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_M
{
    public partial class Jc_MAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_MInfo Jc_MInfo { get; set; }      
    }

	public partial class Jc_MUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_MInfo Jc_MInfo { get; set; }      
    }

	public partial class Jc_MDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_MGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_MGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

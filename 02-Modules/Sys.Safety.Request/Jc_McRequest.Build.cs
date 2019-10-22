using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Mc
{
    public partial class Jc_McAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_McInfo Jc_McInfo { get; set; }      
    }

	public partial class Jc_McUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_McInfo Jc_McInfo { get; set; }      
    }

	public partial class Jc_McDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_McGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_McGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

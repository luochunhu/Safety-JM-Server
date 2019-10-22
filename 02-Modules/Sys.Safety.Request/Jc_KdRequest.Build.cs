using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Kd
{
    public partial class Jc_KdAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_KdInfo Jc_KdInfo { get; set; }      
    }

	public partial class Jc_KdUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_KdInfo Jc_KdInfo { get; set; }      
    }

	public partial class Jc_KdDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_KdGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_KdGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

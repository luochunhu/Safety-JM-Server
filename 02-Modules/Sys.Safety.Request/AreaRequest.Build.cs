using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Area
{
    public partial class AreaAddRequest : Basic.Framework.Web.BasicRequest
    {
        public AreaInfo AreaInfo { get; set; }      
    }

	public partial class AreaUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public AreaInfo AreaInfo { get; set; }      
    }

	public partial class AreaDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AreaGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AreaGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

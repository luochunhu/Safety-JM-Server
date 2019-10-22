using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Right
{
    public partial class RightAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RightInfo RightInfo { get; set; }      
    }

	public partial class RightUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RightInfo RightInfo { get; set; }      
    }

	public partial class RightDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class RightsDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public List<string> IdList { get; set; }
    }

    public partial class RightGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RightGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

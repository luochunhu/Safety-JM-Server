using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Userright
{
    public partial class UserrightAddRequest : Basic.Framework.Web.BasicRequest
    {
        public UserrightInfo UserrightInfo { get; set; }      
    }

	public partial class UserrightUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public UserrightInfo UserrightInfo { get; set; }      
    }

	public partial class UserrightDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class UserrightGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class UserrightGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

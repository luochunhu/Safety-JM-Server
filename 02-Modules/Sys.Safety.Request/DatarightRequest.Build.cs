using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Dataright
{
    public partial class DatarightAddRequest : Basic.Framework.Web.BasicRequest
    {
        public DatarightInfo DatarightInfo { get; set; }      
    }

	public partial class DatarightUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public DatarightInfo DatarightInfo { get; set; }      
    }

	public partial class DatarightDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class DatarightGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class DatarightGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

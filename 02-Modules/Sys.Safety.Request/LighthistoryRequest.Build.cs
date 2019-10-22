using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Lighthistory
{
    public partial class LighthistoryAddRequest : Basic.Framework.Web.BasicRequest
    {
        public LighthistoryInfo LighthistoryInfo { get; set; }      
    }

	public partial class LighthistoryUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public LighthistoryInfo LighthistoryInfo { get; set; }      
    }

	public partial class LighthistoryDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class LighthistoryGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class LighthistoryGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

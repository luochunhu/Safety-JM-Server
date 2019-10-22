using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Dataexchangesetting
{
    public partial class DataexchangesettingAddRequest : Basic.Framework.Web.BasicRequest
    {
        public DataexchangesettingInfo DataexchangesettingInfo { get; set; }      
    }

	public partial class DataexchangesettingUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public DataexchangesettingInfo DataexchangesettingInfo { get; set; }      
    }

	public partial class DataexchangesettingDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class DataexchangesettingGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class DataexchangesettingGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

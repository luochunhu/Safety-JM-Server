using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Callhistory
{
    public partial class B_CallhistoryAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallhistoryInfo B_CallhistoryInfo { get; set; }      
    }

	public partial class B_CallhistoryUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallhistoryInfo B_CallhistoryInfo { get; set; }      
    }

	public partial class B_CallhistoryDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_CallhistoryGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_CallhistoryGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

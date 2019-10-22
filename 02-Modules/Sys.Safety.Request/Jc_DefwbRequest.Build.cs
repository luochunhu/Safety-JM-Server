using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Defwb
{
    public partial class DeviceKoriyasuAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefwbInfo Jc_DefwbInfo { get; set; }      
    }

	public partial class Jc_DefwbUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefwbInfo Jc_DefwbInfo { get; set; }      
    }

	public partial class Jc_DefwbDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_DefwbGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_DefwbGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

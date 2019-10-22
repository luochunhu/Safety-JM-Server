using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Ll
{
    public partial class Jc_LlAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_LlInfo Jc_LlInfo { get; set; }      
    }

	public partial class Jc_LlUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_LlInfo Jc_LlInfo { get; set; }      
    }

	public partial class Jc_LlDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_LlGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_LlGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

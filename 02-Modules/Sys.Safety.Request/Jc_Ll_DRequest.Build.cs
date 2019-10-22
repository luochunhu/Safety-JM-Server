using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Ll_D
{
    public partial class Jc_Ll_DAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_Ll_DInfo Jc_Ll_DInfo { get; set; }      
    }

	public partial class Jc_Ll_DUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_Ll_DInfo Jc_Ll_DInfo { get; set; }      
    }

	public partial class Jc_Ll_DDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_Ll_DGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_Ll_DGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

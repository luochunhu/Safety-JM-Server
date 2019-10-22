using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Bxex
{
    public partial class Jc_BxexAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BxexInfo Jc_BxexInfo { get; set; }      
    }

	public partial class Jc_BxexUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BxexInfo Jc_BxexInfo { get; set; }      
    }

	public partial class Jc_BxexDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_BxexGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_BxexGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

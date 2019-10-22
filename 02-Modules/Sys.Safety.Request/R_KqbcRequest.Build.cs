using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Kqbc
{
    public partial class R_KqbcAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_KqbcInfo KqbcInfo { get; set; }      
    }

	public partial class R_KqbcUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_KqbcInfo KqbcInfo { get; set; }      
    }

	public partial class R_KqbcDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_KqbcGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_KqbcGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

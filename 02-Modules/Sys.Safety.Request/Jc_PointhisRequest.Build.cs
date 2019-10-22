using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Pointhis
{
    public partial class Jc_PointhisAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_PointhisInfo Jc_PointhisInfo { get; set; }      
    }

	public partial class Jc_PointhisUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_PointhisInfo Jc_PointhisInfo { get; set; }      
    }

	public partial class Jc_PointhisDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_PointhisGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_PointhisGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_R
{
    public partial class Jc_RAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_RInfo Jc_RInfo { get; set; }      
    }

	public partial class Jc_RUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_RInfo Jc_RInfo { get; set; }      
    }

	public partial class Jc_RDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_RGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_RGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
    public partial class Jc_RGetByDateAndIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }

        public string Data { get; set; }
    }
}

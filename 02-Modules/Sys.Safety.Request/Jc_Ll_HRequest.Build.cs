using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Ll_H
{
    public partial class Jc_Ll_HAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_Ll_HInfo Jc_Ll_HInfo { get; set; }      
    }

	public partial class Jc_Ll_HUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_Ll_HInfo Jc_Ll_HInfo { get; set; }      
    }

	public partial class Jc_Ll_HDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_Ll_HGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_Ll_HGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

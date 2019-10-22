using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Remark
{
    public partial class Jc_RemarkAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_RemarkInfo Jc_RemarkInfo { get; set; }      
    }

	public partial class Jc_RemarkUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_RemarkInfo Jc_RemarkInfo { get; set; }      
    }

	public partial class Jc_RemarkDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_RemarkGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_RemarkGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

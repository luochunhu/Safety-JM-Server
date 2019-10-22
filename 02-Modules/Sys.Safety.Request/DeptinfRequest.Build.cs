using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Deptinf
{
    public partial class DeptinfAddRequest : Basic.Framework.Web.BasicRequest
    {
        public DeptinfInfo DeptinfInfo { get; set; }      
    }

	public partial class DeptinfUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public DeptinfInfo DeptinfInfo { get; set; }      
    }

	public partial class DeptinfDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class DeptinfGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class DeptinfGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

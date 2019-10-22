using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Month
{
    public partial class Jc_MonthAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_MonthInfo Jc_MonthInfo { get; set; }      
    }

	public partial class Jc_MonthUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_MonthInfo Jc_MonthInfo { get; set; }      
    }

	public partial class Jc_MonthDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_MonthGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_MonthGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

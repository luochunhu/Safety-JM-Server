using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Year
{
    public partial class Jc_YearAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_YearInfo Jc_YearInfo { get; set; }      
    }

	public partial class Jc_YearUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_YearInfo Jc_YearInfo { get; set; }      
    }

	public partial class Jc_YearDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_YearGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_YearGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

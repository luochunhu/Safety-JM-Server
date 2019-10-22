using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Day
{
    public partial class Jc_DayAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DayInfo Jc_DayInfo { get; set; }      
    }

	public partial class Jc_DayUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DayInfo Jc_DayInfo { get; set; }      
    }

	public partial class Jc_DayDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_DayGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_DayGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

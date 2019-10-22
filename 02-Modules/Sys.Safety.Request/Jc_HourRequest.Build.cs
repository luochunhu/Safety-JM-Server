using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Hour
{
    public partial class Jc_HourAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_HourInfo Jc_HourInfo { get; set; }      
    }

	public partial class Jc_HourUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_HourInfo Jc_HourInfo { get; set; }      
    }

	public partial class Jc_HourDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_HourGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
        public string PointId { get; set; }
    }

	public partial class Jc_HourGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

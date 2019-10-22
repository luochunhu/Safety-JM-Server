using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Season
{
    public partial class Jc_SeasonAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_SeasonInfo Jc_SeasonInfo { get; set; }      
    }

	public partial class Jc_SeasonUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_SeasonInfo Jc_SeasonInfo { get; set; }      
    }

	public partial class Jc_SeasonDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_SeasonGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class Jc_SeasonGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Gascontentanalyzeconfig
{
    public partial class GascontentanalyzeconfigAddRequest : Basic.Framework.Web.BasicRequest
    {
        public GascontentanalyzeconfigInfo GascontentanalyzeconfigInfo { get; set; }      
    }

	public partial class GascontentanalyzeconfigUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public GascontentanalyzeconfigInfo GascontentanalyzeconfigInfo { get; set; }      
    }

    public partial class UpdateRealTimeValueRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }

        public string RealTimeValue { get; set; }

        public string State { get; set; }
    }

	public partial class GascontentanalyzeconfigDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class GascontentanalyzeconfigGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class GascontentanalyzeconfigGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

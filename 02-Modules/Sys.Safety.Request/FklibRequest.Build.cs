using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Fklib
{
    public partial class FklibAddRequest : Basic.Framework.Web.BasicRequest
    {
        public FklibInfo FklibInfo { get; set; }      
    }

	public partial class FklibUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public FklibInfo FklibInfo { get; set; }      
    }

	public partial class FklibDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class FklibGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class FklibGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

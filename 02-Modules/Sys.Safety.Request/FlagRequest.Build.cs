using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Flag
{
    public partial class FlagAddRequest : Basic.Framework.Web.BasicRequest
    {
        public FlagInfo FlagInfo { get; set; }      
    }

	public partial class FlagUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public FlagInfo FlagInfo { get; set; }      
    }

	public partial class FlagDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class FlagGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class FlagGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

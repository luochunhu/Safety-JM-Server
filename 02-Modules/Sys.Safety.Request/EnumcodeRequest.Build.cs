using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Enumcode
{
    public partial class EnumcodeAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EnumcodeInfo EnumcodeInfo { get; set; }      
    }    

    public partial class EnumcodeUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EnumcodeInfo EnumcodeInfo { get; set; }      
    }

	public partial class EnumcodeDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EnumcodeGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class EnumcodeGetByEnumTypeIDRequest : Basic.Framework.Web.BasicRequest
    {
        public string EnumTypeId { get; set; }
    }

    public partial class EnumcodeGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

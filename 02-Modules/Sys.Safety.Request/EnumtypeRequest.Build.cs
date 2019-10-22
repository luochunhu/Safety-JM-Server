using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Enumtype
{
    public partial class EnumtypeAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EnumtypeInfo EnumtypeInfo { get; set; }      
    }

	public partial class EnumtypeUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EnumtypeInfo EnumtypeInfo { get; set; }      
    }

	public partial class EnumtypeDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EnumtypeGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class EnumtypeGetByStrCodeRequest : Basic.Framework.Web.BasicRequest
    {
        public string StrCode { get; set; }
    }

    public partial class EnumtypeGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

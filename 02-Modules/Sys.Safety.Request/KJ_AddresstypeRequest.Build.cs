using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.KJ_Addresstype
{
    public partial class KJ_AddresstypeAddRequest : Basic.Framework.Web.BasicRequest
    {
        public KJ_AddresstypeInfo KJ_AddresstypeInfo { get; set; }      
    }

	public partial class KJ_AddresstypeUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public KJ_AddresstypeInfo KJ_AddresstypeInfo { get; set; }      
    }

	public partial class KJ_AddresstypeDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class KJ_AddresstypeGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class KJ_AddresstypeGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

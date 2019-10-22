using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.KJ_Addresstyperule
{
    public partial class KJ_AddresstyperuleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public KJ_AddresstyperuleInfo KJ_AddresstyperuleInfo { get; set; }      
    }

	public partial class KJ_AddresstyperuleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public KJ_AddresstyperuleInfo KJ_AddresstyperuleInfo { get; set; }      
    }

	public partial class KJ_AddresstyperuleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class KJ_AddresstyperuleDeleteByAddressTypeIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AddressTypeId { get; set; }
    }

	public partial class KJ_AddresstyperuleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class KJ_AddresstyperuleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

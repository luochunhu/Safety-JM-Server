using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Listtemple
{
    public partial class ListtempleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ListtempleInfo ListtempleInfo { get; set; }      
    }

	public partial class ListtempleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ListtempleInfo ListtempleInfo { get; set; }      
    }

	public partial class ListtempleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListtempleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListtempleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class SaveListTempleInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public ListtempleInfo Info { get; set; }
    }
}

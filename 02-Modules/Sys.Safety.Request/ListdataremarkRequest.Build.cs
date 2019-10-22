using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Listdataremark
{
    public partial class ListdataremarkAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdataremarkInfo ListdataremarkInfo { get; set; }      
    }

	public partial class ListdataremarkUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdataremarkInfo ListdataremarkInfo { get; set; }      
    }

	public partial class ListdataremarkDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListdataremarkGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListdataremarkGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class GetListdataremarkByTimeListDataIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string Time { get; set; }
        public string ListDataId { get; set; }
    }
}

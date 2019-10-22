using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Listdisplayex
{
    public partial class ListdisplayexAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdisplayexInfo ListdisplayexInfo { get; set; }      
    }

	public partial class ListdisplayexUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdisplayexInfo ListdisplayexInfo { get; set; }      
    }

	public partial class ListdisplayexDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListdisplayexGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListdisplayexGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class SaveListDisplayExInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdisplayexInfo Info { get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Listdataex
{
    public partial class ListdataexAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdataexInfo ListdataexInfo { get; set; }      
    }

	public partial class ListdataexUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdataexInfo ListdataexInfo { get; set; }      
    }

	public partial class ListdataexDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListdataexGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListdataexGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class ListdataexGetBySqlRequest : Basic.Framework.Web.BasicRequest
    {
        public string Sql { get; set; }
    }

    public partial class SaveListDataExInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdataexInfo Info { get; set; }
    }
}

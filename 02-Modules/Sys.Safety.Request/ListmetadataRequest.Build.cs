using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Listmetadata
{
    public partial class ListmetadataAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ListmetadataInfo ListmetadataInfo { get; set; }      
    }

	public partial class ListmetadataUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ListmetadataInfo ListmetadataInfo { get; set; }      
    }

	public partial class ListmetadataDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListmetadataGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListmetadataGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class SaveListMetaDataExInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public ListmetadataInfo Info { get; set; }
    }
}

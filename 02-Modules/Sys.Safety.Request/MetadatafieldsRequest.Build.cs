using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Metadatafields
{
    public partial class MetadatafieldsAddRequest : Basic.Framework.Web.BasicRequest
    {
        public MetadatafieldsInfo MetadatafieldsInfo { get; set; }      
    }

	public partial class MetadatafieldsUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public MetadatafieldsInfo MetadatafieldsInfo { get; set; }      
    }

	public partial class MetadatafieldsDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class MetadatafieldsGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class MetadatafieldsGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

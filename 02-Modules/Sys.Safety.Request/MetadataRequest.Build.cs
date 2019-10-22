using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Metadata
{
    public partial class MetadataAddRequest : Basic.Framework.Web.BasicRequest
    {
        public MetadataInfo MetadataInfo { get; set; }      
    }

	public partial class MetadataUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public MetadataInfo MetadataInfo { get; set; }      
    }

	public partial class MetadataDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class MetadataGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class MetadataGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class ImportMetadataRequest : Basic.Framework.Web.BasicRequest
    {
        public object Obj { get; set; }
    }
}

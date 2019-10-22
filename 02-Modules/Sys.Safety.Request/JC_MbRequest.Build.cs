using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Mb
{
    public partial class JC_MbAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_MbInfo MbInfo { get; set; }      
    }

    public partial class JC_MbUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_MbInfo MbInfo { get; set; }      
    }

    public partial class JC_MbDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class JC_MbGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class JC_MbGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

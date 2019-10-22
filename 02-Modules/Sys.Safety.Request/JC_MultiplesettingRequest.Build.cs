using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Multiplesetting
{
    public partial class JC_MultiplesettingAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_MultiplesettingInfo MultiplesettingInfo { get; set; }      
    }

    public partial class JC_MultiplesettingUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_MultiplesettingInfo MultiplesettingInfo { get; set; }      
    }

    public partial class JC_MultiplesettingDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class JC_MultiplesettingGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class JC_MultiplesettingGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

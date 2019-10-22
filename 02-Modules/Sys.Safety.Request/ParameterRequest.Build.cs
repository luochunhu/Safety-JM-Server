using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Parameter
{
    public partial class ParameterAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_ParameterInfo JC_ParameterInfo { get; set; }      
    }

	public partial class ParameterUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_ParameterInfo JC_ParameterInfo { get; set; }      
    }

	public partial class ParameterDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ParameterGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ParameterGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Factor
{
    public partial class FactorAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_FactorInfo JC_FactorInfo { get; set; }      
    }

	public partial class FactorUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_FactorInfo JC_FactorInfo { get; set; }      
    }

	public partial class FactorDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class FactorGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class FactorGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Deletecheck
{
    public partial class DeletecheckAddRequest : Basic.Framework.Web.BasicRequest
    {
        public DeletecheckInfo DeletecheckInfo { get; set; }      
    }

	public partial class DeletecheckUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public DeletecheckInfo DeletecheckInfo { get; set; }      
    }

	public partial class DeletecheckDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class DeletecheckGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class DeletecheckGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

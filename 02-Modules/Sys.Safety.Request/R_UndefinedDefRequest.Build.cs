using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.UndefinedDef
{
    public partial class R_UndefinedDefAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_UndefinedDefInfo UndefinedDefInfo { get; set; }      
    }

    public partial class R_UndefinedDefUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_UndefinedDefInfo UndefinedDefInfo { get; set; }      
    }

    public partial class R_UndefinedDefDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_UndefinedDefGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_UndefinedDefGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

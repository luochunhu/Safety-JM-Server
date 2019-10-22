using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.MsgLog
{
    public partial class MsgLogAddRequest : Basic.Framework.Web.BasicRequest
    {
        public MsgLogInfo MsgLogInfo { get; set; }      
    }

	public partial class MsgLogUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public MsgLogInfo MsgLogInfo { get; set; }      
    }

	public partial class MsgLogDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class MsgLogGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class MsgLogGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

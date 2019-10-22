using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Powerboxchargehistory
{
    public partial class PowerboxchargehistoryAddRequest : Basic.Framework.Web.BasicRequest
    {
        public PowerboxchargehistoryInfo PowerboxchargehistoryInfo { get; set; }      
    }

	public partial class PowerboxchargehistoryUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public PowerboxchargehistoryInfo PowerboxchargehistoryInfo { get; set; }      
    }

	public partial class PowerboxchargehistoryDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class PowerboxchargehistoryGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class PowerboxchargehistoryGetByFzhOrMacRequest : Basic.Framework.Web.BasicRequest
    {
        public string Fzh { get; set; }
        public string Mac { get; set; }
    }

    public partial class PowerboxchargehistoryGetByStimeRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime Stime { get; set; }       
    }

	public partial class PowerboxchargehistoryGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}

using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.RemoteState
{
    public partial class RemoteStateRequest : Basic.Framework.Web.BasicRequest
    {
        public bool State { get; set; }
        public DateTime LastReviceTime { get; set; }
    }


    public partial class UpdateInspectionTimeRequest : Basic.Framework.Web.BasicRequest
    {
        public long InspectionTime;
    }

    public partial class GetInspectionTimeRequest : Basic.Framework.Web.BasicRequest
    {

    }
}

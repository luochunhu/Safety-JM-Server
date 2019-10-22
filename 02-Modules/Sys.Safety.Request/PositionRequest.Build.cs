using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;

namespace Sys.Safety.Request.Position
{

    public class PositionRequest : BaseRequest
    {
        public PositionRequest(int requestType) : base(requestType)
        {

        }
        public Jc_WzInfo PositionInfo { get; set; }
    }

    public class PositionResponse
    {
        public List<Jc_WzInfo> JcWzList { get; set; } 
    }


    public partial class PositionAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_WzInfo PositionInfo { get; set; }      
    }
    public partial class PositionsRequest : Basic.Framework.Web.BasicRequest
    {
        public List<Jc_WzInfo> PositionsInfo { get; set; }
    }

    public partial class PositionUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_WzInfo PositionInfo { get; set; }      
    }

    public partial class PositionDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class PositionGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class PositionGetByWzIDRequest : Basic.Framework.Web.BasicRequest
    {
        public string WzID { get; set; }
    }
    public partial class PositionGetByWzRequest : Basic.Framework.Web.BasicRequest
    {
        public string Wz { get; set; }
    }

    public partial class PositionGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class AddPositionBySqlRequest : BasicRequest
    {
        public string Wz { get; set; }
    }
}

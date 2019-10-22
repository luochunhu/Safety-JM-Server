using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.ManualCrossControl
{
    public partial class ManualCrossControlAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_JcsdkzInfo ManualCrossControlInfo { get; set; }      
    }

    public partial class ManualCrossControlUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_JcsdkzInfo ManualCrossControlInfo { get; set; }      
    }

    public partial class ManualCrossControlDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class ManualCrossControlsRequest : Basic.Framework.Web.BasicRequest
    {
        public List<Jc_JcsdkzInfo> ManualCrossControlInfos { get; set; }
    }

    public partial class ManualCrossControlGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class ManualCrossControlGetByStationIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int StationID { get; set; }
    }    
    public partial class ManualCrossControlGetByTypeBkPointRequest : Basic.Framework.Web.BasicRequest
    {
        public int Type { get; set; }
        public string BkPoint { get; set; }
    }
    public partial class ManualCrossControlGetByTypeZkPointBkPointRequest : Basic.Framework.Web.BasicRequest
    {
        public int Type { get; set; }
        public string ZkPoint { get; set; }
        public string BkPoint { get; set; }
    }
    public partial class ManualCrossControlGetByTypeZkPointRequest : Basic.Framework.Web.BasicRequest
    {
        public int Type { get; set; }
        public string ZkPoint { get; set; }        
    }
    public partial class ManualCrossControlGetByBkPointRequest : Basic.Framework.Web.BasicRequest
    {        
        public string BkPoint { get; set; }
    }

    public partial class ManualCrossControlGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public class GetAllManualCrossControlDetailResponse : Jc_JcsdkzInfo
    {
        /// <summary>
        /// 主控测点显示值
        /// </summary>
        public string ZkPointText { get; set; }

        /// <summary>
        /// 被控测点显示值
        /// </summary>
        public string BkPointText { get; set; }
    }
}

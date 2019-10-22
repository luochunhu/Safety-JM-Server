using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.StaionControlHistoryData
{
    public partial class StaionControlHistoryDataAddRequest : Basic.Framework.Web.BasicRequest
    {
        public StaionControlHistoryDataInfo StaionControlHistoryDataInfo { get; set; }      
    }

	public partial class StaionControlHistoryDataUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public StaionControlHistoryDataInfo StaionControlHistoryDataInfo { get; set; }      
    }

	public partial class StaionControlHistoryDataDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class StaionControlHistoryDataGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class StaionControlHistoryDataGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
    public partial class StaionControlHistoryDataGetByFzhRequest : Basic.Framework.Web.BasicRequest
    {
        public ushort Fzh;
    }

    public partial class StationControlHistoryDataToDBRequest : Basic.Framework.Web.BasicRequest
    {
        public List<StaionControlHistoryDataInfo> StaionControlHistoryDataItems;
    }

    public partial class GetStaionControlHistoryDataByByFzhTimeRequest : Basic.Framework.Web.BasicRequest
    {
        public string Fzh { get; set; }

        public string Time { get; set; }
    }
    public partial class DeleteByPointAndTimeStaionControlHistoryDataRequest : Basic.Framework.Web.BasicRequest
    {
        public string Point { get; set; }
        public DateTime Time { get; set; }
    }

    public partial class GetStaionControlHistoryDataByByFzhTimeResponse : StaionControlHistoryDataInfo
    {
        /// <summary>
        /// 设备性质名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 设备状态名称
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 控制量执行情况(2进制字符串)
        /// </summary>
        public string ControlDeviceConvert { get; set; }
    }
}

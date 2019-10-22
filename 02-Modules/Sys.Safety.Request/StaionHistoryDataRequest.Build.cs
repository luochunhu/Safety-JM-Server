using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.StaionHistoryData
{
    public partial class StaionHistoryDataAddRequest : Basic.Framework.Web.BasicRequest
    {
        public StaionHistoryDataInfo StaionHistoryDataInfo { get; set; }      
    }

	public partial class StaionHistoryDataUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public StaionHistoryDataInfo StaionHistoryDataInfo { get; set; }      
    }

	public partial class StaionHistoryDataDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class StaionHistoryDataGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class StaionHistoryDataGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
    public partial class StaionHistoryDataGetByFzhRequest : Basic.Framework.Web.BasicRequest
    {
        public ushort Fzh;
    }
    public partial class InsertStationHistoryDataRequest : Basic.Framework.Web.BasicRequest
    {
        public List<StaionHistoryDataInfo> StationHistoryDataItems;
    }
    public partial class DeleteByPointAndTimeStationHistoryDataRequest : Basic.Framework.Web.BasicRequest
    {
        public string Point { get; set; }
        public DateTime Time { get; set; }
    }
    public partial class GetSubstationHistoryRealDataByFzhTimeRequest : Basic.Framework.Web.BasicRequest
    {
        public string Fzh { get; set; }

        public string Time { get; set; }
    }

    public partial class GetSubstationHistoryRealDataByFzhTimeResponse : StaionHistoryDataInfo
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
        /// 设备型号名称
        /// </summary>
        public string DeviceTypeName { get; set; }
    }
}

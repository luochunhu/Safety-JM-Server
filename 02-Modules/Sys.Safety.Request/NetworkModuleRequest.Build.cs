using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.DataContract.CommunicateExtend;

namespace Sys.Safety.Request.NetworkModule
{
    public partial class NetworkModuleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_MacInfo NetworkModuleInfo { get; set; }      
    }
    public partial class NetworkModulesRequest : Basic.Framework.Web.BasicRequest
    {
        public List<Jc_MacInfo> NetworkModulesInfo { get; set; }
    }

    public partial class NetworkModuleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_MacInfo NetworkModuleInfo { get; set; }      
    }

    public partial class NetworkModuleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class NetworkModuleDeleteByMacRequest : Basic.Framework.Web.BasicRequest
    {
        public string Mac { get; set; }
    }

    public partial class NetworkModuleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class NetworkModuleGetByWzRequest : Basic.Framework.Web.BasicRequest
    {
        public string Wz { get; set; }
    }
    public partial class NetworkModuleGetBySwitchesMacRequest : Basic.Framework.Web.BasicRequest
    {
        public string SwitchesMac { get; set; }
    }
    public partial class NetworkModuleGetByMacRequest : Basic.Framework.Web.BasicRequest
    {
        public string Mac { get; set; }
    }
    public partial class SearchNetworkModuleRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 用于是否指定获取分站的网络信息20181101  =1表示搜索分站，=0表示搜索交换机
        /// </summary>
        public int StationFind { get; set; }
    }
    
    public partial class NetworkModuletParametersSetRequest : Basic.Framework.Web.BasicRequest
    {
        public string MAC { get; set; }

        public string StationFind { get; set; }
        public NetDeviceSettingInfo Parameters { get; set; }
    }
    public partial class NetworkModuletCommParametersSetRequest : Basic.Framework.Web.BasicRequest
    {
        public string MAC { get; set; }
        public NetDeviceSettingInfo Parameters { get; set; }
        public int CommPort { get; set; }
    }
    public partial class NetworkModuletParametersGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Mac { get; set; }
        public uint WaitTime { get; set; }
    }

    public partial class NetworkModuleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class GetAllPowerBoxAddressByMacRequest : Basic.Framework.Web.BasicRequest
    {
        public string Mac { get; set; }
    }

    public partial class GetSwitchBatteryInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public string Mac { get; set; }
        public string Address { get; set; }
    }

    public partial class GetSwitchAllPowerBoxInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public string Mac { get; set; }
    }

    public partial class GetSwitchAllPowerBoxInfoResponse : Basic.Framework.Web.BasicRequest
    {
        public List<BatteryItem> PowerBoxInfo { get; set; }

        public DateTime PowerDateTime { get; set; }
    }
    /// <summary>
    /// 部分更新MAC信息
    /// </summary>
    public partial class NetworkModuleCacheUpdatePropertiesRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 更新信息
        /// </summary>
        public Dictionary<string, object> UpdateItems { get; set; }
    }

    public partial class TestAlarmRequest : Basic.Framework.Web.BasicRequest
    {
        public List<Jc_MacInfo> macItems;
        public int testAlarmFlag;
    }
}

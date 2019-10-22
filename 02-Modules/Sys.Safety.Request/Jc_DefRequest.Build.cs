using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.DataCollection.Common.Protocols.Devices;

namespace Sys.Safety.Request.PointDefine
{
    public partial class PointDefineAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefInfo PointDefineInfo { get; set; }
    }
    public partial class PointDefinesAddRequest : Basic.Framework.Web.BasicRequest
    {
        public List<Jc_DefInfo> PointDefinesInfo { get; set; }
    }

    public partial class PointDefineUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefInfo PointDefineInfo { get; set; }
    }
    public partial class PointDefinesUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public List<Jc_DefInfo> PointDefinesInfo { get; set; }
    }

    public partial class PointDefineAddNetworkModuleAddUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefInfo PointDefineInfo { get; set; }
        public Jc_MacInfo NetworkModuleInfo { get; set; }

        public Jc_MacInfo NetworkModuleInfoOld { get; set; }

        public List<Jc_DefInfo> UpdateSonPointList { get; set; }

        public Jc_MacInfo SwitchesInfo { get; set; }

        public Jc_MacInfo SwitchesInfoOld { get; set; }
    }

    public partial class PointDefineDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class PointDefineGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class PointDefineGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }
    public partial class PointDefineGetByPointRequest : Basic.Framework.Web.BasicRequest
    {
        public string Point { get; set; }
    }
    public partial class PointDefineGetByWzRequest : Basic.Framework.Web.BasicRequest
    {
        public string Wz { get; set; }
    }
    public partial class PointDefineGetByMacRequest : Basic.Framework.Web.BasicRequest
    {
        public string Mac { get; set; }
    }
    public partial class PointDefineGetByCOMRequest : Basic.Framework.Web.BasicRequest
    {
        public string COM { get; set; }
    }
    public partial class PointDefineGetByDevpropertIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevpropertID { get; set; }
    }

    public partial class PointDefineGetByAreaIDRequest : Basic.Framework.Web.BasicRequest
    {
        public string AreaId { get; set; }
    }

    public partial class PointDefineGetByDevClassIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevClassID { get; set; }
    }
    public partial class PointDefineGetByDevModelIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevModelID { get; set; }
    }
    public partial class PointDefineGetByDevIDRequest : Basic.Framework.Web.BasicRequest
    {
        public string DevID { get; set; }
    }
    public partial class PointDefineGetByStationIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int StationID { get; set; }
    }
    public partial class PointDefineGetByStationPointRequest : Basic.Framework.Web.BasicRequest
    {
        public string StationPoint { get; set; }
    }
    public partial class PointDefineGetByStationIDChannelIDDevPropertIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int StationID { get; set; }
        public int ChannelID { get; set; }
        public int DevPropertID { get; set; }
    }
    public partial class PointDefineGetByStationIDDevPropertIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int StationID { get; set; }
        public int DevPropertID { get; set; }
    }
    public partial class PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int StationID { get; set; }
        public int ChannelID { get; set; }
        public int AddressID { get; set; }
        public int DevPropertID { get; set; }
    }
    public partial class PointDefineGetByStationIDChannelIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int StationID { get; set; }
        public int ChannelID { get; set; }
    }
    public partial class PointDefineGetByAreaCodeRequest : Basic.Framework.Web.BasicRequest
    {
        public string AreaCode { get; set; }
    }
    public partial class PointDefineGetByAreaIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AreaId { get; set; }
    }
    public partial class PointDefineGetByAddressTypeIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AddressTypeId { get; set; }
    }
    public partial class PointDefineGetByAreaCodeDevPropertIDRequest : Basic.Framework.Web.BasicRequest
    {
        public string AreaCode { get; set; }
        public int DevPropertID { get; set; }
    }
    public partial class PointDefineGetByStrKeywordsRequest : Basic.Framework.Web.BasicRequest
    {
        public string StrKeywords { get; set; }
    }
    public partial class PointDefineGetByPointIDRequest : Basic.Framework.Web.BasicRequest
    {
        public string PointID { get; set; }
    }
    public partial class PointDefineGetBySensorPowerAlarmValueRequest : Basic.Framework.Web.BasicRequest
    {
        public float SensorPowerAlarmValue { get; set; }
    }
    public partial class PointDefineGetByUnderVoltageAlarmValueRequest : Basic.Framework.Web.BasicRequest
    {
        
    }
    public partial class PointDefineGetByGradingAlarmLevelRequest : Basic.Framework.Web.BasicRequest
    {
       
    }

    public partial class PointDefineGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class SendDComReqest : Basic.Framework.Web.BasicRequest
    {
        public List<BatteryControlItem> queryBatteryRealDataItems;
    }

    public class BatteryControlItem
    {
        public int DevProID { get; set; }
        public string FzhOrMac { get; set; }
        /// <summary>
        /// 0不进行操作，1取消维护性放电，2维护性放电 
        /// </summary>
        public int controlType { get; set; }
    }

    public partial class GetAllPowerBoxAddressRequest : Basic.Framework.Web.BasicRequest
    {
        public string Fzh { get; set; }
    }

    public partial class GetSubstationBatteryInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public string Fzh { get; set; }
        public string Address { get; set; }
    }

    public partial class GetSubstationAllPowerBoxInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public string Fzh { get; set; }
    }

    public partial class GetSubstationAllPowerBoxInfoResponse : Basic.Framework.Web.BasicRequest
    {
        public List<BatteryItem> PowerBoxInfo { get; set; }

        public DateTime PowerDateTime { get; set; }
    }

    public partial class SwitchesDControlRequest : Basic.Framework.Web.BasicRequest
    {
        public List<SwichControlItem> swichControlItems;
    }

    public partial class StationDControlRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 控制链表
        /// </summary>
        public List<StationControlItem> controlItems;
    }

    public class SwichControlItem
    {
        public string mac;
        /// <summary>
        /// =0表示无动作，=1表示远程放电。
        /// </summary>
        public byte controlType;
    }

    public class StationControlItem
    {
        public ushort fzh;
        /// <summary>
        /// 0不进行操作，1取消维护性放电，2维护性放电
        /// </summary>
        public byte controlType;
    }


    public partial class GetHistoryControlRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 控制链表
        /// </summary>
        public List<StationControlItem> controlItems;
    }

    public partial class HistoryRealDataRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 控制链表
        /// </summary>
        public List<StationControlItem> controlItems;
    }
    public partial class DeviceInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public List<DeviceInfoRequestItem> deviceInfoRequestItems;
    }

    public partial class GetRealLinkageInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public int recordId { get; set; }
    }

    public class DeviceInfoRequestItem
    {
        /// <summary>
        /// 分站号
        /// </summary>
        public ushort Fzh { get; set; }
        ///// <summary>
        ///// 是否需要获取软件版本号=1表示要获取  =0表示不获取
        ///// </summary>
        //public byte GetSoftwareVersions { get; set; }
        ///// <summary>
        ///// 是否需要获取硬件版本号 =1表示要获取  =0表示不获取
        ///// </summary>
        //public byte GetHardwareVersions { get; set; }
        ///// <summary>
        ///// 获取唯一信息编码标记=0表法不获取，=1表示仅获取分站的唯一编码（含电量箱），=2表示获取分站及下级设备全部的唯一编码
        ///// </summary>
        //public byte GetDeviceSoleCoding { get; set; }
        /// <summary>
        /// =0表示无动作（取消获取），=1表示开始获取。
        /// </summary>
        public byte controlType;
        /// <summary>
        /// 表示需要获取设备的地址列表=0表示1号地址的传感器，分站默认都需要回发
        /// </summary>
        public List<int> GetAddressLst = new List<int>();
    }

    public partial class DeviceAddressModificationRequest : Basic.Framework.Web.BasicRequest
    {
        public List<DeviceAddressModificationItem> DeviceAddressModificationItems;
    }

    public class DeviceAddressModificationItem
    {
        public ushort fzh;
        /// <summary>
        /// 修改传感器地址列表，一次性仅能修改1个设备，链表是便于后续扩展
        /// </summary>
        public List<DeviceAddressItem> DeviceAddressItem { get; set; }
    }

    public partial class GetSubstationBasicInfoResponse
    {
        /// <summary>
        /// 设备型号
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string Location { get; set; }

        ///// <summary>
        ///// 软件版本号
        ///// </summary>
        //public string SoftVersion { get; set; }

        ///// <summary>
        ///// 硬件版本号
        ///// </summary>
        //public string HardwareVersion { get; set; }

        /// <summary>
        /// 唯一编码
        /// </summary>
        public string OnlyCoding { get; set; }
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime TimeNow { get; set; }
        /// <summary>
        /// 生产时间
        /// </summary>
        public DateTime ProductionTime { get; set; }
        /// <summary>
        /// 入口电压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 重启次数
        /// </summary>
        public int RestartNum { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }

        /// <summary>
        /// 下级设备基础信息
        /// </summary>
        public List<InferiorBasicInfo> InferiorInfo { get; set; }
    }

    /// <summary>
    /// 基础信息
    /// </summary>
    public class InferiorBasicInfo
    {
        /// <summary>
        /// 测点号或地址号
        /// </summary>
        public string PointOrAddr { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 唯一编码
        /// </summary>
        public string OnlyCoding { get; set; }
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime TimeNow { get; set; }
        /// <summary>
        /// 生产时间
        /// </summary>
        public DateTime ProductionTime { get; set; }
        /// <summary>
        /// 入口电压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 重启次数
        /// </summary>
        public int RestartNum { get; set; }
        /// <summary>
        /// 报警次数
        /// </summary>
        public int AlarmNum { get; set; }
    }

    public class GetSubstationBasicInfoRequest
    {
        public string Fzh { get; set; }
    }
    public class OldPlsPointSyncRequest : BasicRequest
    {
        public DataTable PointInfo { get; set; }
    }

    public class PlsPointSyncRequest : BasicRequest
    {
        public List<PersonPointInfo> PointInfo { get; set; }
    }

    public partial class SynchronousPointRequest : BasicRequest
    {
        public DataTable Points { get; set; }
    }
    public class PersonPointInfo
    {
        public string point { get; set; }

        public string fzh { get; set; }

        public string kh { get; set; }

        public string type { get; set; }

        public string wz { get; set; }

        public string state { get; set; }

        public string alarm { get; set; }

        public string zts { get; set; }

        public string jckz1 { get; set; }

        public string jckz2 { get; set; }

        public string k1 { get; set; }
    }

    public partial class BroadcastSysPointSyncRequest : BasicRequest
    {
        public List<BroadcastPointInfo> Points { get; set; }
    }

    public class BroadcastPointInfo
    {
        public string AddNum { get; set; }

        public string Wz { get; set; }

        public string Groupname { get; set; }

        public string ZoneId { get; set; }

        public string State { get; set; }

        public string Mac { get; set; }

        public string IP { get; set; }

        public string Type { get; set; }
    }
}

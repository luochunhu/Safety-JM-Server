using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    public class EditDeviceAddressItem
    {
        /// <summary>
        /// 表示当前下发的随机码，设备收到回发时，也按此码进行应答
        /// </summary>
        public byte RandomCode;
        /// <summary>
        /// 待修改设备信息链表
        /// </summary>
        public List<DeviceAddressItem> DeviceAddressItems = new List<DeviceAddressItem>();
    }

    public class DeviceAddressItem
    {
        /// <summary>
        /// 表示设备的唯一编码；
        /// </summary>
        public string SoleCoding { get; set; }
        /// <summary>
        /// 修改前的地址号
        /// </summary>
        public byte BeforeModification { get; set; }
        /// <summary>
        /// 修改后的地址号
        /// </summary>
        public byte AfterModification { get; set; }
        /// <summary>
        /// 设备型号2018.4.2 by
        /// </summary>
        public byte DeviceType { get; set; }
    }
}

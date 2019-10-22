using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Driver
{
    public class CommunicationCommandValue
    {
        /// <summary>
        /// 初始化命令 0x0001
        /// </summary>
        public static ushort Comm_InitializeRequest = 0x0001;
        /// <summary>
        /// 查询电源箱数据命令 0x0002
        /// </summary>
        public static ushort Comm_QueryBatteryRealDataRequest = 0x0002;
        /// <summary>
        /// 通讯测试命令 0x0004
        /// </summary>
        public static ushort Comm_CommunicationTestRequest = 0x0004;
        /// <summary>
        /// 复位命令 0x0008
        /// </summary>
        public static ushort Comm_ResetDeviceCommandRequest = 0x0008;
        //public ushort Comm_InitializeRequest = 0x0010;
        /// <summary>
        /// 下发分级报警控制
        /// </summary>
        public static ushort Comm_SetSensorGradingAlarmRequest = 0x0010;

        //public ushort Comm_InitializeRequest = 0x0020;
        /// <summary>
        /// 获取设备实时数据命令 F命令 0x0040
        /// </summary>
        public static ushort Comm_QueryRealDataRequest = 0x0040;
        /// <summary>
        /// 设备控制命令 0x0080
        /// </summary>
        public static ushort Comm_DeviceControlRequest = 0x0080;
        /// <summary>
        /// 获取设备信息命令 0x0100
        /// </summary>
        public static ushort Comm_QueryDeviceInfoRequest = 0x0100;
        /// <summary>
        /// 查询分站历史控制数据命令 0x0200
        /// </summary>
        public static ushort Comm_QueryHistoryControlRequest = 0x0200;
        /// <summary>
        /// 查询分站历史五分钟数据命令 0x0400
        /// </summary>
        public static ushort Comm_QueryHistoryRealDataRequest = 0x0400;
        /// <summary>
        /// 修改设备地址号命令 0x0800
        /// </summary>
        public static ushort Comm_ModificationDeviceAddressRequest = 0x0800;



    }
}

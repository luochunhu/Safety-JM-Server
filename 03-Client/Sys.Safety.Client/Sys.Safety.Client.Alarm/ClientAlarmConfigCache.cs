using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sys.Safety.DataContract;

namespace Sys.Safety.Client.Alarm
{
    /// <summary>
    /// 报警配置文件缓存
    /// </summary>
    public static class ClientAlarmConfigCache
    {
        private static string _soundLightPortName = "COM1";
        private static int _soundLightBaudRate = 9600;
        private static bool _isUseAlarmConfig = true;
        private static bool _isUsePopupAlarm = true;

        /// <summary>
        /// 是否自动从服务器下载报警配置并应用到本地
        /// </summary>
        public static bool IsAutoLoadAlarmConfigFromServer { get; set; }
        /// <summary>
        /// 是否启用本地报警配置，不启用则本地不处理报警记录（即不再进行运行记录的解析和相应的报警）
        /// </summary>
        public static bool IsUseAlarmConfig
        {
            set { _isUseAlarmConfig = value; }
            get { return _isUseAlarmConfig; }
        }

        /// <summary>
        /// 右下角报表提示
        /// </summary>
        public static bool IsUsePopupAlarm
        {
            set { _isUsePopupAlarm = value; }
            get { return _isUsePopupAlarm; }
        }
        /// <summary>
        /// 性质配置列表
        /// </summary>
        public static List<ClientAlarmItems> listProperty { get; set; }
        /// <summary>
        /// 种类配置列表
        /// </summary>
        public static List<ClientAlarmItems> listClass { get; set; }
        /// <summary>
        /// 类型配置列表
        /// </summary>
        public static List<ClientAlarmItems> listDev { get; set; }
        /// <summary>
        /// 测点配置列表
        /// </summary>
        public static List<ClientAlarmItems> listPoint { get; set; }
        /// <summary>
        /// 声光报警通信端口，默认COM1
        /// </summary>
        public static string soundLightPortName
        {
            set { _soundLightPortName = value; }
            get { return _soundLightPortName; }
        }
        /// <summary>
        /// 声光报警串行波特率，默认9600
        /// </summary>
        public static int soundLightBaudRate
        {
            set { _soundLightBaudRate = value; }
            get { return _soundLightBaudRate; }
        }
        /// <summary>
        /// 语音列表
        /// </summary>
        public static List<ShowDataInfo> showDataSound { get; set; }
        /// <summary>
        /// 蜂鸣器列表
        /// </summary>
        public static List<ShowDataInfo> showDataBuzzer { get; set; }
        /// <summary>
        /// 声光列表
        /// </summary>
        public static List<ShowDataInfo> showDataLight { get; set; }
        /// <summary>
        /// 图文列表
        /// </summary>
        public static List<ShowDataInfo> showDataGraph { get; set; }
    }
}

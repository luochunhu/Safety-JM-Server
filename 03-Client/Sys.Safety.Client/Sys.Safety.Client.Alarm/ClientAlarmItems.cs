using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Safety.Client.Alarm
{
    /// <summary>
    /// 报警设置对象
    /// </summary>
    [Serializable]
    public class ClientAlarmItems
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 报警类型
        /// </summary>
        public string alarmType { get; set; }

        /// <summary>
        /// 报警类型编码
        /// </summary>
        public string alarmCode { get; set; }

        /// <summary>
        /// 显示报警方式
        /// 1.声音报警；2.声光报警；3.居中弹出报警
        /// </summary>
        public string alarmShow { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    public class ControlRemote
    {
        /// <summary>
        /// 控制测点设备号
        /// </summary>
        public decimal m_nCtrlStationID;

        /// <summary>
        /// 标签
        /// </summary>
        public string m_strLabel;

        /// <summary>
        /// 控制测点通道号
        /// </summary>
        public decimal m_nCtrlChannelID;

        /// <summary>
        /// 控制测点地址号
        /// </summary>
        public decimal m_nCtrlAdrID;

        /// <summary>
        /// 控制类型
        /// </summary>
        public int ControlType;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    /// <summary>
    /// 分站电源箱5分钟耗电量
    /// </summary>
    public  class BatteryPowerConsumption
    {

        protected string channel;
        /// <summary>
        /// 地址号
        /// </summary>
        public string Channel
        {
            get { return channel; }
            set
            {
                channel = value;
            }
        }

        protected int powerConsumption;
        /// <summary>
        /// 5分钟耗电量情况
        /// </summary>
        public int PowerConsumption
        {
            get { return powerConsumption; }
            set
            {
                powerConsumption = value;
            }
        }
        
    }
}

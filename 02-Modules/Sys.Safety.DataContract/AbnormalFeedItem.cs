using Sys.Safety.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Safety.DataContract
{
    public class AbnormalFeedItem
    {
        /// <summary>
        /// 主控异常数据记录id号
        /// </summary>
        public string zkId;
        /// <summary>
        /// 主控点  模拟量 开关量
        /// </summary>
        public string arrPoint;
        /// <summary>
        /// 被控点 控制量
        /// </summary>
        public string ControlPoint;
        /// <summary>
        /// 馈电点 馈电开关量
        /// </summary>
        public string FeekBackPoint;
        /// <summary>
        /// 控制引发时间 如报警控制 报警的开始时间
        /// </summary>
        public DateTime TriggerTime;
        /// <summary>
        /// 引发控制类型 上限报警 下限报警 上限断电 下限断电 断线 上溢 负漂 
        /// </summary>
        public DeviceDataState TriggerState;
        /// <summary>
        /// 记录类型 1-本地控制 2-交叉控制 3-本地馈电异常 4-交叉馈电异常
        /// </summary>
        public int TriggerType;
        /// <summary>
        /// 是否已写记录
        /// </summary>
        public bool HaveWrite = false;
    }
}

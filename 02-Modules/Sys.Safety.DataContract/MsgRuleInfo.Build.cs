using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class MsgRuleInfo
    {
        /// <summary>
        /// 唯一编码
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 规则类型
        /// </summary>
        public int Type
        {
            get;
            set;
        }
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public long DevId
        {
            get;
            set;
        }
        /// <summary>
        /// 设备编号Id
        /// </summary>
        public long PointId
        {
            get;
            set;
        }
        /// <summary>
        /// 设备类型名称或设备编号
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 报警类型或报警值下限
        /// </summary>
        public double Value1
        {
            get;
            set;
        }
        /// <summary>
        /// 报警值上限
        /// </summary>
        public double Value2
        {
            get;
            set;
        }
        /// <summary>
        /// 备用1
        /// </summary>
        public string B1
        {
            get;
            set;
        }
        /// <summary>
        /// 备用2
        /// </summary>
        public string B2
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        {
            get;
            set;
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    /// <summary>
    /// 报警处理
    /// </summary>
    public class AlarmProcessInfo
    {
        /// <summary>
        /// 报警Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Id String
        /// </summary>
        public string AlarmId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 设备位置
        /// </summary>
        public string Wz { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 报警类型
        /// </summary>
        public string StrEnumDisplay { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Stime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Etime { get; set; }
        /// <summary>
        /// 实时值
        /// </summary>
        public string Ssz { get; set; }
        /// <summary>
        /// 处理措施
        /// </summary>
        public string Cs { get; set; }
        /// <summary>
        /// 报警原因
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        public string Bz1 { get; set; }
        /// <summary>
        /// 处理意见
        /// </summary>
        public string Bz2 { get; set; }
        /// <summary>
        /// 结束时间显示格式（19990-01-01 00:00:00 显示为-） 
        /// </summary>
        public string EtimeDisplay { get; set; }
    }
}

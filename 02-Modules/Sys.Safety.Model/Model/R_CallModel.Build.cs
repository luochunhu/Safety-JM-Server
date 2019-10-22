using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_Call")]
    public partial class R_CallModel
    {
        /// <summary>
        /// 唯一编码
        /// </summary>
        [Key]
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 主控ID(如果是手动控制存0，如果是大数据分析存模型ID，如果是应急联动存应急联动ID)
        /// </summary>
        public string MasterId
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫类型（0-人员呼叫，1-设备呼叫）
        /// </summary>
        public int Type
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫类型（0-一般呼叫，1-紧急呼叫，2-解除呼叫）
        /// </summary>
        public int CallType
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫人员及设备类型（0-所有人员呼叫，1-呼叫指定卡号段，2-呼叫指定人员，3-呼叫所有设备，4-呼叫指定设备）
        /// </summary>
        public int CallPersonDefType
        {
            get;
            set;
        }
        /// <summary>
        /// 人员卡号
        /// 卡号段存 开始卡号-结束卡号
        /// 多个人员用","分隔
        /// </summary>
        public string BhContent
        {
            get;
            set;
        }
        /// <summary>
        /// 设备Point，多个用","分隔
        /// </summary>
        public string PointList
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫时间
        /// </summary>
        public DateTime CallTime
        {
            get;
            set;
        }
        
    }
}


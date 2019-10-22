using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("RA_Call")]
    public partial class B_CallModel
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
        /// 主控Id
        /// </summary>
        public string MasterId
        {
            get;
            set;
        }
        
        /// <summary>呼叫名称
        /// 
        /// </summary>
        public string CallName { get; set; }

        /// <summary>
        ///呼叫类型（ 0-一般呼叫（通话），1-广播呼叫，2-解除呼叫）
        /// </summary>
        public int CallType
        {
            get;
            set;
        }

        /// <summary>广播/呼叫类型
        /// 
        /// </summary>
        public int RadioType { get; set; }

        /// <summary>通话状态
        /// 
        /// </summary>
        public int CallState { get; set; }

        /// <summary>
        /// 广播内容
        /// </summary>
        public string Message
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

        public string Bz1 { get; set; }

        public string Bz2 { get; set; }

        public string Bz3 { get; set; }
    }
}


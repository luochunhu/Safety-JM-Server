using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class B_CallInfo
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
        /// 主控ID(如果是手动控制存0，如果是大数据分析存模型ID，如果是应急联动存应急联动ID)
        /// </summary>
        public string MasterId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CallName
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
        /// 
        /// </summary>
        public int RadioType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int CallState
        {
            get;
            set;
        }
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

        /// <summary>
        /// 
        /// </summary>
        public string Bz1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz3
        {
            get;
            set;
        }
        /// <summary>
        /// 下发状态（ 0待下发，1下发成功，2下发失败）
        /// </summary>
        public int SendState
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫详细信息表
        /// </summary>
        public List<B_CallpointlistInfo> CallPointList
        {
            get;
            set;
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_CallHistory")]
    public partial class R_PhjModel
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
        /// 呼叫类型（0-井下呼叫地面，1-地面呼叫井下）
        /// </summary>
        public int Hjlx
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫卡号
        /// </summary>
        public string Bh
        {
            get;
            set;
        }
        /// <summary>
        /// 人员编号
        /// </summary>
        public string Yid
        {
            get;
            set;
        }
        /// <summary>
        /// 设备Id
        /// </summary>
        public string PointId
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
        /// 呼叫提示音响应次数
        /// </summary>
        public int Tsycs
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫状态（0-解除呼叫 1-一般呼叫 2-紧急呼叫）
        /// </summary>
        public int State
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫范围类型（0-全员呼叫 1-区段呼叫 2-指定卡号呼叫）
        /// </summary>
        public int Type
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫的卡号
        /// </summary>
        public string Card
        {
            get;
            set;
        }
        /// <summary>
        /// 登陆用户名
        /// </summary>
        public string Username
        {
            get;
            set;
        }
        /// <summary>
        /// 登陆用户Ip
        /// </summary>
        public string IP
        {
            get;
            set;
        }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime Timer
        {
            get;
            set;
        }
        /// <summary>
        /// 呼叫标志
        /// </summary>
        public int Flag
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类型标志:0—人员,1—机车
        /// </summary>
        public int SysFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string upflag
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By3
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By4
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By5
        {
            get;
            set;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_PersonAlarm")]
    public partial class R_PbModel
    {
        	    /// <summary>
        /// 编号
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 区域ID
        /// </summary>
                public string Areaid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 人员Id
        /// </summary>
                public string Yid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 测点Id
        /// </summary>
                public string Pointid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 报警前的最后一次采集时间
        /// </summary>
                public DateTime Zdzs
        {
           get;
           set;
        }
        	    /// <summary>
        /// 报警开始时间，等同于写记录时间
        /// </summary>
                public DateTime Starttime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 报警结束时间
        /// </summary>
                public DateTime Endtime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 报警类型(关联枚举表)
        /// </summary>
                public int Type
        {
           get;
           set;
        }
        	    /// <summary>
        /// 未使用，保留
        /// </summary>
                public string Z1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 未使用，保留
        /// </summary>
                public string Z2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 经办人id
        /// </summary>
                public string Z3
        {
           get;
           set;
        }
        	    /// <summary>
        /// 系统类型标志:0—人员,1—机车
        /// </summary>
                public string Z4
        {
           get;
           set;
        }
        	    /// <summary>
        /// 处理标志，0—未处理，1—已处理
        /// </summary>
                public string Z5
        {
           get;
           set;
        }
        	    /// <summary>
        /// 报警提示音响应次数
        /// </summary>
                public string Z6
        {
           get;
           set;
        }
        	    /// <summary>
        /// 标记
        /// </summary>
                public string Upflag
        {
           get;
           set;
        }
            }
}


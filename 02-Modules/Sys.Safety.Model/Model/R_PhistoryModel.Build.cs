using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_Trajectory")]
    public partial class R_PhistoryModel
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
        /// 标志卡号
        /// </summary>
                public string Bh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 内部编号
        /// </summary>
                public string Yid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 识别点号
        /// </summary>
                public string Pointid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 采集时间
        /// </summary>
                public DateTime Rtime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 写数据库时间
        /// </summary>
                public DateTime Timer
        {
           get;
           set;
        }
        	    /// <summary>
        /// 记录标志
        /// </summary>
                public string Flag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 系统类型标志
        /// </summary>
                public string Sysflag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 标注该轨迹是否可用
        /// </summary>
                public string Cwflag
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
        	    /// <summary>
        /// 上传标志
        /// </summary>
                public string Upflag
        {
           get;
           set;
        }
            }
}


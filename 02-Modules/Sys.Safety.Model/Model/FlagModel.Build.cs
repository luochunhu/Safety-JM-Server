using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_Flag")]
    public partial class FlagModel
    {
        /// <summary>
        /// ID编号
        /// </summary>
        [Key]
        public string ID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 矿编号,单机默认0
        /// </summary>
                public int Sysid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 额定总人数
        /// </summary>
                public string Edzrs
        {
           get;
           set;
        }
        	    /// <summary>
        /// 井下总人数
        /// </summary>
                public string Jxzrs
        {
           get;
           set;
        }
        	    /// <summary>
        /// 井下报警人数
        /// </summary>
                public string Jxbjrs
        {
           get;
           set;
        }
        	    /// <summary>
        /// 今日下井人数
        /// </summary>
                public string Jrxjrs
        {
           get;
           set;
        }
        	    /// <summary>
        /// 刷新数据库时间
        /// </summary>
                public DateTime Timer
        {
           get;
           set;
        }
        	    /// <summary>
        /// 标记
        /// </summary>
                public string Flag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 默认为0或空，中心站正常退出为1
        /// </summary>
                public string IsExit
        {
           get;
           set;
        }
            }
}


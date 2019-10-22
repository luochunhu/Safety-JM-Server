using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("CF_Hour")]
    public partial class Jc_Ll_HModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        public string ID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 测点id编号
        /// </summary>
                public string PointID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 负压平均值
        /// </summary>
                public double FY
        {
           get;
           set;
        }
        	    /// <summary>
        /// 温度平均值
        /// </summary>
                public double WD
        {
           get;
           set;
        }
        	    /// <summary>
        /// 瓦斯平均值
        /// </summary>
                public double WS
        {
           get;
           set;
        }
        	    /// <summary>
        /// 一氧化碳平均值
        /// </summary>
                public double CO
        {
           get;
           set;
        }
        	    /// <summary>
        /// 标混平均值
        /// </summary>
                public double BH
        {
           get;
           set;
        }
        	    /// <summary>
        /// 工混平均值
        /// </summary>
                public double GH
        {
           get;
           set;
        }
        	    /// <summary>
        /// 标纯平均值
        /// </summary>
                public double BC
        {
           get;
           set;
        }
        	    /// <summary>
        /// 工纯平均值
        /// </summary>
                public double GC
        {
           get;
           set;
        }
        	    /// <summary>
        /// 标混累计值
        /// </summary>
                public double BHL
        {
           get;
           set;
        }
        	    /// <summary>
        /// 标纯累计值
        /// </summary>
                public double BCL
        {
           get;
           set;
        }
        	    /// <summary>
        /// 工混累计值
        /// </summary>
                public double GHL
        {
           get;
           set;
        }
        	    /// <summary>
        /// 工纯累计值
        /// </summary>
                public double GCL
        {
           get;
           set;
        }
        	    /// <summary>
        /// 存储时间
        /// </summary>
                public DateTime Timer
        {
           get;
           set;
        }
        	    /// <summary>
        /// 该小时运行时长
        /// </summary>
                public string Yxsc
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用1
        /// </summary>
                public string Bz1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用2
        /// </summary>
                public string Bz2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用3
        /// </summary>
                public string Bz3
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用4
        /// </summary>
                public string Bz4
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用5
        /// </summary>
                public string Bz5
        {
           get;
           set;
        }
        	    /// <summary>
        /// 上传标志0-未传1-已传
        /// </summary>
                public string Upflag
        {
           get;
           set;
        }
            }
}


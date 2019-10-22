using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    /// <summary>
    /// 未使用
    /// </summary>
    [Table("jc_ll")]
    public partial class Jc_LlModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public string ID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Fzh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Kh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Dzh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Devid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Wzid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Point
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Yy
        {
           get;
           set;
        }
        	    /// <summary>
        /// 累计数据
        /// </summary>
                public string Y
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Upflag
        {
           get;
           set;
        }
            }
}


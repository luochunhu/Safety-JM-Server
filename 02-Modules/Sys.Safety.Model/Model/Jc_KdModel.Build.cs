using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_FeedInfo")]
    public partial class Jc_KdModel
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
        /// Jc_b表中的ID编号
        /// </summary>
                public string BJID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 馈电id
        /// </summary>
                public string KDID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 时间
        /// </summary>
                public DateTime Timer
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


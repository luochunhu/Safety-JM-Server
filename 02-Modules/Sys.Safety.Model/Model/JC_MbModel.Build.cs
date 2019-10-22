using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_MultipleHistory")]
    public partial class JC_MbModel
    {
        	    /// <summary>
        /// 
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string PointID
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
                public string Devid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Wzid
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
                public string Type
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Bstj
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public double Bsz
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime Stime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime Etime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public double Ssz
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public double Zdz
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public double Pjz
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime Zdzs
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
        /// 
        /// </summary>
                public string Upflag
        {
           get;
           set;
        }
            }
}


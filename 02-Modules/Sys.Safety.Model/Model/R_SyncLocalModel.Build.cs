using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_TemporaryCollection")]
    public partial class R_SyncLocalModel
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
                public string Bh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Yid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Fzh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Kh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Zt
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime Rtime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Sysflag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Flag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Passup
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime Timer
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Tsgzflag
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


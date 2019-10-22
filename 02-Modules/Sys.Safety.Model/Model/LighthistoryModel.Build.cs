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
    [Table("lighthistory")]
    public partial class LighthistoryModel
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
                public double Ssz
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
            }
}


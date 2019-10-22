using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_MultipleSetting")]
    public partial class JC_MultiplesettingModel
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
                public string Devid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string MultipleText
        {
           get;
           set;
        }
            }
}


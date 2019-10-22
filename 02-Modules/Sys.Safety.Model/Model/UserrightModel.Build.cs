using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_UserRight")]
    public partial class UserrightModel
    {
        /// <summary>
        /// 用户权限ID
        /// </summary>
        [Key]
        public string UserRightID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 用户ID
        /// </summary>
                public string UserID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 权限ID
        /// </summary>
                public string RightID
        {
           get;
           set;
        }
            }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_RoleRight")]
    public partial class RolerightModel
    {
        /// <summary>
        /// 角色权限ID
        /// </summary>
        [Key]
        public string RoleRightID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 角色ID
        /// </summary>
                public string RoleID
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


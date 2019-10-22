using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_RoleWEBMenuAuth")]
    public partial class RolewebmenuauthModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public string ID
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
        /// 按钮权限ID
        /// </summary>
                public string MenuAuthID
        {
           get;
           set;
        }
            }
}


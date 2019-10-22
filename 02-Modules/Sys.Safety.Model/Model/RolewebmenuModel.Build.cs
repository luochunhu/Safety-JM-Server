using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_RoleWEBMenu")]
    public partial class RolewebmenuModel
    {
        /// <summary>
        /// 角色模块权限ID
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
        /// 模块ID
        /// </summary>
                public string ModuleID
        {
           get;
           set;
        }
            }
}


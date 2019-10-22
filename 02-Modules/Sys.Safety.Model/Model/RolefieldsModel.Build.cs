using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("bft_rolefields")]
    public partial class RolefieldsModel
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
        /// 字段所在模块ID
        /// </summary>
                public string ModuleID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 所属列表ID
        /// </summary>
                public string ListID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 列表显示项目ID
        /// </summary>
                public string ListDisplayID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注1
        /// </summary>
                public string Bz1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注2
        /// </summary>
                public string Bz2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注3
        /// </summary>
                public string Bz3
        {
           get;
           set;
        }
            }
}


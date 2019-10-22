using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_Role")]
    public partial class RoleModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        public string RoleID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 角色编号
        /// </summary>
                public string RoleCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 角色名称
        /// </summary>
                public string RoleName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 角色描述
        /// </summary>
                public string RoleDescription
        {
           get;
           set;
        }
        	    /// <summary>
        /// 角色使用标记
        /// </summary>
                public int RoleFlag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 创建时间
        /// </summary>
                public DateTime CreateTime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 创建人
        /// </summary>
                public string CreateName
        {
           get;
           set;
        }
            }
}


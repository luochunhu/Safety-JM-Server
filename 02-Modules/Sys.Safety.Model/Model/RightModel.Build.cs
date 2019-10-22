using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_Right")]
    public partial class RightModel
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        [Key]
        public string RightID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 权限编号
        /// </summary>
                public string RightCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 权限名称
        /// </summary>
                public string RightName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 权限描述
        /// </summary>
                public string RightDescription
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
        	    /// <summary>
        /// 权限类型
        /// </summary>
                public string RightType
        {
           get;
           set;
        }
            }
}


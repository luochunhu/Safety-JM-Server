using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_SysConfig")]
    public partial class ConfigModel
    {
        /// <summary>
        /// ID编号
        /// </summary>
         [Key]
        public string ID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 配置项名称
        /// </summary>
                public string Name
        {
           get;
           set;
        }
        	    /// <summary>
        /// 配置项值
        /// </summary>
                public string Text
        {
           get;
           set;
        }
        	    /// <summary>
        /// 标志
        /// </summary>
                public string Upflag
        {
           get;
           set;
        }
            }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_DataRight")]
    public partial class DatarightModel
    {
        /// <summary>
        /// 数据权限表ID
        /// </summary>
        [Key]
        public string DataRightID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 数据权限编码
        /// </summary>
                public string StrCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 数据权限名称
        /// </summary>
                public string StrName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 数据权限表达式
        /// </summary>
                public string StrContent
        {
           get;
           set;
        }
            }
}


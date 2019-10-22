using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_EnumCode")]
    public partial class EnumcodeModel
    {
        /// <summary>
        /// 枚举ID
        /// </summary>
        [Key]
        public string EnumCodeID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 枚举类型ID
        /// </summary>
                public string EnumTypeID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 枚举值
        /// </summary>
                public int LngEnumValue
        {
           get;
           set;
        }
        	    /// <summary>
        /// 枚举显示值
        /// </summary>
                public string StrEnumDisplay
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否为默认
        /// </summary>
                public string BlnDefault
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否为系统预制
        /// </summary>
                public string BlnPredefined
        {
           get;
           set;
        }
        	    /// <summary>
        /// 显示顺序
        /// </summary>
                public int LngRowIndex
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否有效
        /// </summary>
                public string BlnEnable
        {
           get;
           set;
        }
        	    /// <summary>
        /// 枚举扩展值1
        /// </summary>
                public DateTime LngEnumValue1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 枚举扩展值2
        /// </summary>
                public decimal LngEnumValue2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 枚举扩展值3
        /// </summary>
                public string LngEnumValue3
        {
           get;
           set;
        }
        	    /// <summary>
        /// 枚举扩展值4
        /// </summary>
                public string LngEnumValue4
        {
           get;
           set;
        }
        	    /// <summary>
        /// 开发描述信息
        /// </summary>
                public string StrDescription
        {
           get;
           set;
        }
            }
}


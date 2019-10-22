using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_FKLib")]
    public partial class FklibModel
    {
        /// <summary>
        /// FK参照ID
        /// </summary>
        [Key]
        public string FKLibID
        {
           get;
           set;
        }
        	    /// <summary>
        /// FK参照编码
        /// </summary>
                public string StrFKCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// FK参照名称
        /// </summary>
                public string StrFKName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 参照描述
        /// </summary>
                public string StrFkDesc
        {
           get;
           set;
        }
        	    /// <summary>
        /// FK参照的取数来源SQL
        /// </summary>
                public string StrSQL
        {
           get;
           set;
        }
        	    /// <summary>
        /// 父字段
        /// </summary>
                public string StrParentField
        {
           get;
           set;
        }
        	    /// <summary>
        /// FK参照选择返回结果的数据列名
        /// </summary>
                public string StrValueMember
        {
           get;
           set;
        }
        	    /// <summary>
        /// FK参照选择返回结果的显示数据列名
        /// </summary>
                public string StrDsiplayMember
        {
           get;
           set;
        }
        	    /// <summary>
        /// FK参照选择时显示的列描述，以列名，列标题，列，列标题方式成对罗列
        /// </summary>
                public string StrColumns
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否默认选择
        /// </summary>
                public string BlnDefaultSelection
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string StrEntityList
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否默认选择
        /// </summary>
                public string BlnEnable
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string BlnDynamicSql
        {
           get;
           set;
        }
        	    /// <summary>
        /// 树控件FK编码
        /// </summary>
                public string StrTreeFkCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 树控件过滤字段
        /// </summary>
                public string StrTreeFilterField
        {
           get;
           set;
        }
        	    /// <summary>
        /// Grid过滤字段
        /// </summary>
                public string StrGridFilterField
        {
           get;
           set;
        }
        	    /// <summary>
        /// 请求命令参数
        /// </summary>
                public string StrCommandParameter
        {
           get;
           set;
        }
            }
}


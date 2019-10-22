using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_ListDataEx")]
    public partial class ListdataexModel
    {
        /// <summary>
        /// 列表数据ID
        /// </summary>
        [Key]
        public int ListDataID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 所属列表ID
        /// </summary>
                public int ListID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int UserID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 列表取数的完整SQL
        /// </summary>
                public string StrListSQL
        {
           get;
           set;
        }
        	    /// <summary>
        /// 原始SQL{nowhere}或者{where}
        /// </summary>
                public string StrListSrcSQL
        {
           get;
           set;
        }
        	    /// <summary>
        /// 列表数据的显示名称（数据条件名称）
        /// </summary>
                public string StrListDataName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 列表数据的显示名称（数据条件名称）
        /// </summary>
                public string StrListDefaultField
        {
           get;
           set;
        }
        	    /// <summary>
        /// 分析表设置
        /// </summary>
                public string StrPivotSetting
        {
           get;
           set;
        }
        	    /// <summary>
        /// 图表设置
        /// </summary>
                public string StrChartSetting
        {
           get;
           set;
        }
        	    /// <summary>
        /// 小计
        /// </summary>
                public bool BlnSmlSum
        {
           get;
           set;
        }
        	    /// <summary>
        /// 小计类型
        /// </summary>
                public int LngSmlSumType
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
        /// 默认显示样式
        /// </summary>
                public string StrDefaultShowStyle
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否默认
        /// </summary>
                public bool BlnDefault
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否预置
        /// </summary>
                public bool BlnPredefine
        {
           get;
           set;
        }
        	    /// <summary>
        /// 上下文条件串
        /// </summary>
                public string StrConTextCondition
        {
           get;
           set;
        }
            }
}


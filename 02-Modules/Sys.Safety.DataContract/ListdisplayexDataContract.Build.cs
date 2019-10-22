using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class ListdisplayexInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 列表显示项目ID
        /// </summary>
        public int ListDisplayID
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
        /// 所属列表ID
        /// </summary>
        public int ListDataID
        {
           get;
           set;
        }
         	    /// <summary>
        /// （原始）字段名称
        /// </summary>
        public string StrListDisplayFieldName
        {
           get;
           set;
        }
         	    /// <summary>
        /// （原始）字段名称
        /// </summary>
        public string StrListDisplayFieldNameCHS
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否小计
        /// </summary>
        public bool BlnSummary
        {
           get;
           set;
        }
         	    /// <summary>
        /// （暂不处理） 数据显示类型 1 PK 不显示 2 FK 编码+名称 3 字符串 4 2位小数的货币 5 数字 6 日期 7 逻辑 8 二进制 9 4位小数的货币
        /// </summary>
        public int LngListDisplayFieldFormat
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段显示宽度
        /// </summary>
        public int LngDisplayWidth
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
        /// 分组类型(1计数,2汇总,3平均,4最大,5最小)
        /// </summary>
        public int LngSummaryType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 显示格式
        /// </summary>
        public string StrSummaryDisplayFormat
        {
           get;
           set;
        }
         	    /// <summary>
        /// 超链接类型 1卡片或单据，2列表
        /// </summary>
        public int LngHyperLinkType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 超级链接单据(parameters={targetColumnFiledName};RequestType[列如:Receipt,Entity=Receipt&ReceiptID={ReceiptID}&ReceiptTypeID={ReceiptTypeID}];Bill)
        /// </summary>
        public string StrHyperlink
        {
           get;
           set;
        }
         	    /// <summary>
        /// 超链接参数字段
        /// </summary>
        public string StrParaColName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 条件格式
        /// </summary>
        public string StrConditionFormat
        {
           get;
           set;
        }
         	    /// <summary>
        /// 计算列
        /// </summary>
        public bool IsCalcField
        {
           get;
           set;
        }
         	    /// <summary>
        /// 模糊查询条件
        /// </summary>
        public string StrBluerCondition
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int LngSortOrder
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否合并
        /// </summary>
        public bool BlnMerge
        {
           get;
           set;
        }
         	    /// <summary>
        /// 应用类型(1代表全部,2代表列表,3代表报表)
        /// </summary>
        public int LngApplyType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int LngFKType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public bool BlnMainMerge
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public bool BlnKeyWord
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int LngKeyGroup
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public bool BlnConstant
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public bool LngProivtType
        {
           get;
           set;
        }
            }
}



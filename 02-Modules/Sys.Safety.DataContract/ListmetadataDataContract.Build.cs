using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class ListmetadataInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 
        /// </summary>
        public int ID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 列表方案ID
        /// </summary>
        public int ListDataID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 元数据ID
        /// </summary>
        public int MetaDataID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 元数据字段ID
        /// </summary>
        public int MetaDataFieldID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 元数据名称
        /// </summary>
        public string MetaDataFieldName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 父元数据ID
        /// </summary>
        public int LngParentFieldID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 关联元数据ID
        /// </summary>
        public int LngRelativeFieldID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段全路径描述
        /// </summary>
        public string StrFullPath
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段全路径描述
        /// </summary>
        public string StrParentFullPath
        {
           get;
           set;
        }
         	    /// <summary>
        /// 构造sql别名
        /// </summary>
        public string StrTableAlias
        {
           get;
           set;
        }
         	    /// <summary>
        /// 表别名数量
        /// </summary>
        public int LngAliasCount
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段来源类型(0代表直接来源，2代表参照)
        /// </summary>
        public int LngSourceType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 父元数据ID
        /// </summary>
        public int LngParentID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段类型
        /// </summary>
        public string StrFieldType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 参照编码
        /// </summary>
        public string StrFkCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否为系统处理字段 用于建立关系自动生成的字段(ID外键字段，元数据blnPK为true的字段)
        /// </summary>
        public bool BlnSysProcess
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public bool BlnShow
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否为系统处理字段 用于建立关系自动生成的字段(ID外键字段，元数据blnPK为true的字段)
        /// </summary>
        public int LngKeyFieldType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否为计算字段
        /// </summary>
        public bool IsCalcField
        {
           get;
           set;
        }
         	    /// <summary>
        /// 计算公式
        /// </summary>
        public string StrFormula
        {
           get;
           set;
        }
         	    /// <summary>
        /// 引用栏目列表
        /// </summary>
        public string StrRefColList
        {
           get;
           set;
        }
         	    /// <summary>
        /// 排序顺序
        /// </summary>
        public int LngOrder
        {
           get;
           set;
        }
         	    /// <summary>
        /// 排序方式0 Asc 1 Desc
        /// </summary>
        public int LngOrderMethod
        {
           get;
           set;
        }
         	    /// <summary>
        /// 固定条件
        /// </summary>
        public string StrCondition
        {
           get;
           set;
        }
         	    /// <summary>
        /// 固定条件中文
        /// </summary>
        public string StrConditionCHS
        {
           get;
           set;
        }
         	    /// <summary>
        /// 固定条件中文
        /// </summary>
        public bool BlnFreCondition
        {
           get;
           set;
        }
         	    /// <summary>
        /// 常用条件顺序
        /// </summary>
        public int LngFreConIndex
        {
           get;
           set;
        }
         	    /// <summary>
        /// 常用条件串
        /// </summary>
        public string StrFreCondition
        {
           get;
           set;
        }
         	    /// <summary>
        /// 常用条件串中文
        /// </summary>
        public string StrFreConditionCHS
        {
           get;
           set;
        }
         	    /// <summary>
        /// 接收参数
        /// </summary>
        public bool BlnReceivePara
        {
           get;
           set;
        }
         	    /// <summary>
        /// 常用条件是否打印
        /// </summary>
        public bool BlnPrintFreCondition
        {
           get;
           set;
        }
            }
}



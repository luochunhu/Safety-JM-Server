using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class MetadatafieldsInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// ID
        /// </summary>
        public int ID
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
        /// 元数据表名(冗余字段)
        /// </summary>
        public string StrMetaDataTable
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段名
        /// </summary>
        public string StrFieldName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段中文名
        /// </summary>
        public string StrFieldChName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string StrFieldDesc
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
        /// 字段类型
        /// </summary>
        public string StrFieldDevDesc
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段长度
        /// </summary>
        public int LngFieldLen
        {
           get;
           set;
        }
         	    /// <summary>
        /// 小数位数
        /// </summary>
        public int DecimalNum
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否必录
        /// </summary>
        public bool BlnMust
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否为PK字段
        /// </summary>
        public bool BlnPK
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段默认值
        /// </summary>
        public string StrDefaultValue
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否必须隐藏 主要针对ID字段
        /// </summary>
        public bool BlnHidden
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否控制字段权限
        /// </summary>
        public bool BlnFieldRight
        {
           get;
           set;
        }
         	    /// <summary>
        /// 数据权限类型 (0不控制数据权限 1 机构 2 商品)
        /// </summary>
        public int LngDataRightType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段顺序
        /// </summary>
        public int LngRowIndex
        {
           get;
           set;
        }
         	    /// <summary>
        /// 字段关联档案来源类型
        /// </summary>
        public int LngSourceType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 关联元数据ID或者枚举类型ID
        /// </summary>
        public int LngRelativeID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 关联元数据字段ID或者枚举字段键值
        /// </summary>
        public int LngRelativeFieldID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 来源字段的展现形式
        /// </summary>
        public int LngFieldShowStyle
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string StrFkCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 显示格式
        /// </summary>
        public string StrDisplayFormat
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string StrLastUpdateTime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否作为日表拼sql(Union)依据
        /// </summary>
        public bool BlnDay
        {
           get;
           set;
        }
         	    /// <summary>
        /// 栏目顺序可编排
        /// </summary>
        public bool BlnDesignSort
        {
           get;
           set;
        }
            }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class MetadataInfo : Basic.Framework.Web.BasicInfo
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
        /// 元数据名称
        /// </summary>
        public string StrName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 表名
        /// </summary>
        public string StrTableName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 系统模块(如JC,XS,KC)
        /// </summary>
        public string StrBusinessModule
        {
           get;
           set;
        }
         	    /// <summary>
        /// 源元数据ID
        /// </summary>
        public int SourceMetaDataID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 来源类型(U数据表，V视图)
        /// </summary>
        public string StrSrcType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 类型ID(单据类型ID或者枚举类型ID)
        /// </summary>
        public int TypeID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 过滤条件
        /// </summary>
        public string StrFilter
        {
           get;
           set;
        }
         	    /// <summary>
        /// 描述
        /// </summary>
        public string StrDesc
        {
           get;
           set;
        }
         	    /// <summary>
        /// 关键字段属性名(暂时未用)
        /// </summary>
        public string StrKeyFieldPropName
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
        /// 缓存
        /// </summary>
        public bool BlnCache
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public bool BlnDay
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string StrDayType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string StrDayTableName
        {
           get;
           set;
        }
            }
}



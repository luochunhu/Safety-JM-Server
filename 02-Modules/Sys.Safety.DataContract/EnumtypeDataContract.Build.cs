using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class EnumtypeInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 
        /// </summary>
        public string ID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 主键ID
        /// </summary>
        public string EnumTypeID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 父ID
        /// </summary>
        public string ParentID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 枚举类型编码
        /// </summary>
        public string StrCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 枚举类型名称
        /// </summary>
        public string StrName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否预置
        /// </summary>
        public bool BlnPrefined
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否为末级
        /// </summary>
        public bool BlnDetail
        {
           get;
           set;
        }
         	    /// <summary>
        /// 启用扩展值1
        /// </summary>
        public bool BlnEnumValue1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 启用扩展值2
        /// </summary>
        public bool BlnEnumValue2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 启用扩展值3
        /// </summary>
        public bool BlnEnumValue3
        {
           get;
           set;
        }
            }
}



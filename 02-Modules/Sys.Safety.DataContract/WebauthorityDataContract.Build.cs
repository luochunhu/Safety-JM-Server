using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class WebauthorityInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// ID主键
        /// </summary>
        public string AuthID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 子权限类型
        /// </summary>
        public int AuthType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 子权限名称
        /// </summary>
        public string AuthName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 子权限按钮样式名称
        /// </summary>
        public string AuthBtnStyle
        {
           get;
           set;
        }
         	    /// <summary>
        /// 子权限按钮调用方法名称
        /// </summary>
        public string AuthBtnClass
        {
           get;
           set;
        }
         	    /// <summary>
        /// 子权限使用标志（0:禁用,1:启用）
        /// </summary>
        public string UseFlag
        {
           get;
           set;
        }
         	    /// <summary>
        /// 排序号
        /// </summary>
        public int SortNum
        {
           get;
           set;
        }
         	    /// <summary>
        /// 数据标记
        /// </summary>
        public string Upflag
        {
           get;
           set;
        }
         	    /// <summary>
        /// 权限ID[预留]
        /// </summary>
        public string RightID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用1
        /// </summary>
        public string By1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用2
        /// </summary>
        public string By2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用3
        /// </summary>
        public string By3
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用4
        /// </summary>
        public string By4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用5
        /// </summary>
        public string By5
        {
           get;
           set;
        }
            }
}



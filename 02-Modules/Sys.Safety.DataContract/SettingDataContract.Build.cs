using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class SettingInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 主键ID
        /// </summary>
        public string ID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 分组名称
        /// </summary>
        public string StrType
        {
           get;
           set;
        }
         	    /// <summary>
        /// key值
        /// </summary>
        public string StrKey
        {
           get;
           set;
        }
         	    /// <summary>
        /// 中文名
        /// </summary>
        public string StrKeyCHs
        {
           get;
           set;
        }
         	    /// <summary>
        /// 值
        /// </summary>
        public string StrValue
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备注
        /// </summary>
        public string StrDesc
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string Creator
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string LastUpdateDate
        {
           get;
           set;
        }
            }
}



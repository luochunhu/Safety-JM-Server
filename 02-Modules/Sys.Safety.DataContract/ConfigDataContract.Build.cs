using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class ConfigInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// ID编号
        /// </summary>
        public string ID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 配置项名称
        /// </summary>
        public string Name
        {
           get;
           set;
        }
         	    /// <summary>
        /// 配置项值
        /// </summary>
        public string Text
        {
           get;
           set;
        }
         	    /// <summary>
        /// 标志
        /// </summary>
        public string Upflag
        {
           get;
           set;
        }
            }
}



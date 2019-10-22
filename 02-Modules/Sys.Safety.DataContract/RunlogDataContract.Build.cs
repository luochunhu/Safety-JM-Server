using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class RunlogInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime CreateDate
        {
           get;
           set;
        }
         	    /// <summary>
        /// 线程号
        /// </summary>
        public string ThreadNumber
        {
           get;
           set;
        }
         	    /// <summary>
        /// 日志等级
        /// </summary>
        public string LogLevel
        {
           get;
           set;
        }
         	    /// <summary>
        /// 记录器
        /// </summary>
        public string Logger
        {
           get;
           set;
        }
         	    /// <summary>
        /// 日志内容
        /// </summary>
        public string MessageContent
        {
           get;
           set;
        }
            }
}



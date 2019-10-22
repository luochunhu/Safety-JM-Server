using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class DeletecheckInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 基础编码删除检查ID
        /// </summary>
        public string DeleteCheckID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 来源表
        /// </summary>
        public string SourceTable
        {
           get;
           set;
        }
         	    /// <summary>
        /// 来源字段
        /// </summary>
        public string SourceField
        {
           get;
           set;
        }
         	    /// <summary>
        /// 目标表
        /// </summary>
        public string TargetTable
        {
           get;
           set;
        }
         	    /// <summary>
        /// 目标字段
        /// </summary>
        public string TargetField
        {
           get;
           set;
        }
         	    /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
           get;
           set;
        }
            }
}


